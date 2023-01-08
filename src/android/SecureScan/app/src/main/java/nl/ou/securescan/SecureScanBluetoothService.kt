package nl.ou.securescan

import android.annotation.SuppressLint
import android.app.PendingIntent
import android.app.Service
import android.app.TaskStackBuilder
import android.bluetooth.*
import android.bluetooth.BluetoothDevice.BOND_BONDED
import android.bluetooth.le.AdvertiseCallback
import android.bluetooth.le.AdvertiseData
import android.bluetooth.le.AdvertiseSettings
import android.bluetooth.le.BluetoothLeAdvertiser
import android.content.BroadcastReceiver
import android.content.Context
import android.content.Intent
import android.content.IntentFilter
import android.os.Binder
import android.os.IBinder
import android.os.ParcelUuid
import android.util.Log
import androidx.appcompat.app.AppCompatActivity
import kotlinx.coroutines.runBlocking
import nl.ou.securescan.crypto.extensions.getNameAndEmail
import nl.ou.securescan.crypto.extensions.toHexString
import nl.ou.securescan.data.DocumentDatabase
import nl.ou.securescan.helpers.NotificationUtils
import nl.ou.securescan.helpers.Status
import java.io.File
import java.io.FileOutputStream

@SuppressLint("MissingPermission")
class SecureScanBluetoothService : Service() {

    companion object {
        var documentAccessRequest: AppCompatActivity? = null
        var instance: SecureScanBluetoothService? = null
        fun isAlive(): Boolean = instance != null

        var status: Status = Status()
        var accessRequestIntent: Intent? = null

        private var accessRequest: AccessRequest? = null
        private var certBytes: List<Byte> = listOf()
    }

    init {
        instance = this
    }

    private val TAG: String = "SecureScanBT"

    private lateinit var bluetoothManager: BluetoothManager
    private var bluetoothGattServer: BluetoothGattServer? = null

    /* Collection of notification subscribers */
    private val registeredDevices = mutableSetOf<BluetoothDevice>()

    /**
     * Listens for Bluetooth adapter events to enable/disable
     * advertising and server functionality.
     */
    private val bluetoothReceiver = object : BroadcastReceiver() {
        override fun onReceive(context: Context, intent: Intent) {

            when (intent.getIntExtra(BluetoothAdapter.EXTRA_STATE, BluetoothAdapter.STATE_OFF)) {
                BluetoothAdapter.STATE_ON -> {
                    startAdvertising()
                    startServer()
                }
                BluetoothAdapter.STATE_OFF -> {
                    stopServer()
                    stopAdvertising()
                }
            }
        }
    }

    /**
     * Callback to receive information about the advertisement process.
     */
    private val advertiseCallback = object : AdvertiseCallback() {
        override fun onStartSuccess(settingsInEffect: AdvertiseSettings) {
            Log.i(TAG, "LE Advertise Started.")
            Log.i(
                TAG,
                "**** BluetoothGattDescriptor.ENABLE_NOTIFICATION_VALUE: ${BluetoothGattDescriptor.ENABLE_NOTIFICATION_VALUE.toHex()} ************"
            )
        }

        override fun onStartFailure(errorCode: Int) {
            Log.w(TAG, "LE Advertise Failed: $errorCode")
        }
    }

    fun ByteArray.toHex(): String =
        joinToString(separator = "") { eachByte -> "%02x".format(eachByte) }

    fun sendSecurecontainerHash(
        device: BluetoothDevice,
        requestId: Int,
        responseNeeded: Boolean,
        value: ByteArray
    ) {
        try {
            // code that may throw an exception

            certBytes = listOf()
            accessRequest = AccessRequest()
            accessRequest!!.setSecureContainerSha1Hash(value)
            documentAccessRequest?.let { documentAccessRequest!!.finishAndRemoveTask() }

            val db = DocumentDatabase.getDatabase(baseContext)
            val dao = db.documentDao()
            runBlocking {
                Log.i(TAG, "Received secure document hash: ${value.toHexString()}")
                val doc = dao.getByHash(value)
                if (doc != null) {
                    accessRequest!!.setDocument(doc)
                    status.setStatus(SecureScanGattProfile.STATUS_DOCUMENT_AVAILABLE)
                } else {
                    status.setStatus(SecureScanGattProfile.STATUS_DOCUMENT_NOT_AVAILABLE)
                }

                Log.i(
                    TAG,
                    "Status prepared successfully for secure document hash: ${value.toHexString()}"
                )
            }

        } catch (e: Exception) {
            Log.e(TAG, "Error: ${e.message}")
            Log.e(TAG, "Error: $e")
            val logFile = File(getExternalFilesDir(null), "error-log.txt")
            val fos = FileOutputStream(logFile, true)
            fos.write("Error: $e\n".toByteArray())
            fos.close()
        }

        if (responseNeeded) {
            bluetoothGattServer?.sendResponse(
                device,
                requestId,
                BluetoothGatt.GATT_SUCCESS,
                0, null
            )
        }
    }

    fun sendPublicCertPart(
        device: BluetoothDevice,
        requestId: Int,
        responseNeeded: Boolean,
        value: ByteArray
    ) {
        if (value.isNotEmpty()) {
            Log.i("SecureScan", "Receive pub cert part")
            certBytes = certBytes.plus(value.asList())
        } else {
            Log.i("SecureScan", "Receive pub cert part: all is received")
            accessRequest!!.setPublicCertificate(certBytes.toByteArray())
            status.setStatus(SecureScanGattProfile.STATUS_REQUEST_WAITFORUSER)
            runBlocking {
                notifyUserForRequestedDocument()
            }
        }

        if (responseNeeded) {
            bluetoothGattServer?.sendResponse(
                device,
                requestId,
                BluetoothGatt.GATT_SUCCESS,
                0, null
            )
        }
    }

    fun getKey(
        device: BluetoothDevice,
        requestId: Int
    ) {
        var returnValue = "".toByteArray()

        if (status.getStatus() == SecureScanGattProfile.STATUS_REQUEST_ACCEPTED && accessRequest != null) {
            returnValue = accessRequest!!.getKey()
        }

        bluetoothGattServer?.sendResponse(
            device,
            requestId,
            BluetoothGatt.GATT_SUCCESS,
            0,
            returnValue
        )


        accessRequest = null
        status.setStatus(SecureScanGattProfile.STATUS_IDLE)
    }

    fun getStatus(
        device: BluetoothDevice,
        requestId: Int
    ) {
        val returnValue = arrayOf(status.getStatus()).toByteArray()
        Log.i(TAG, "GetStatus : ${returnValue.toHexString()} ... ")

        bluetoothGattServer!!.sendResponse(
            device,
            requestId,
            BluetoothGatt.GATT_SUCCESS,
            0,
            returnValue
        )

        Log.i(TAG, "Status returned ")
    }

    fun getName(
        device: BluetoothDevice,
        requestId: Int
    ) {
        Log.i(TAG, "getName ... ")

        val returnValue = arrayOf(status.getStatus()).toByteArray()

        bluetoothGattServer!!.sendResponse(
            device,
            requestId,
            BluetoothGatt.GATT_SUCCESS,
            0,
            returnValue
        )

        Log.i(TAG, "Status returned ")
    }

    private val gattServerCallback = object : BluetoothGattServerCallback() {

        override fun onConnectionStateChange(
            device: BluetoothDevice,
            status: Int,
            newState: Int
        ) {
            if (newState == BluetoothProfile.STATE_CONNECTED) {
                Log.i(TAG, "BluetoothDevice CONNECTED: $device")
            } else if (newState == BluetoothProfile.STATE_DISCONNECTED) {
                Log.i(TAG, "BluetoothDevice DISCONNECTED: $device")
                //Remove device from any active subscriptions
                registeredDevices.remove(device)
            }
        }

        override fun onCharacteristicReadRequest(
            device: BluetoothDevice, requestId: Int, offset: Int,
            characteristic: BluetoothGattCharacteristic
        ) {
            if (device.bondState != BOND_BONDED) {
                Log.e("SecureScan", "** not paired!!!")
                bluetoothGattServer?.sendResponse(
                    device,
                    requestId,
                    BluetoothGatt.GATT_FAILURE,
                    0,
                    "Not paired!".toByteArray()
                )
                return
            }

            try {
                if (SecureScanGattProfile.GETKEY == characteristic.uuid) {
                    getKey(device, requestId)
                } else if (SecureScanGattProfile.GETSTATUS == characteristic.uuid) {
                    getStatus(device, requestId)
                } else if (SecureScanGattProfile.GETNAME == characteristic.uuid) {
                    getName(device, requestId)
                } else {
                    bluetoothGattServer?.sendResponse(
                        device,
                        requestId,
                        BluetoothGatt.GATT_FAILURE,
                        0,
                        arrayOf(0x00.toByte()).toByteArray()
                    )
                }
            } catch (e: Exception) {
                bluetoothGattServer?.sendResponse(
                    device,
                    requestId,
                    BluetoothGatt.GATT_FAILURE,
                    0,
                    arrayOf(0x00.toByte()).toByteArray()
                )
            }
        }

        override fun onCharacteristicWriteRequest(
            device: BluetoothDevice?,
            requestId: Int,
            characteristic: BluetoothGattCharacteristic?,
            preparedWrite: Boolean,
            responseNeeded: Boolean,
            offset: Int,
            value: ByteArray?
        ) {
            if (device != null && device?.bondState != BOND_BONDED) {
                Log.e("SecureScan", "** not paired!!!")
                bluetoothGattServer?.sendResponse(
                    device,
                    requestId,
                    BluetoothGatt.GATT_FAILURE,
                    0,
                    "Not paired!".toByteArray()
                )
                return
            }

            if (SecureScanGattProfile.SENDSECURECONTAINERHASH == characteristic!!.uuid) {
                sendSecurecontainerHash(device!!, requestId, responseNeeded, value!!)
            } else if (SecureScanGattProfile.PUBLICCERT == characteristic!!.uuid) {
                sendPublicCertPart(device!!, requestId, responseNeeded, value!!)
            }
        }
    }

    private fun startServer() {
        bluetoothGattServer = bluetoothManager.openGattServer(this, gattServerCallback)

        bluetoothGattServer?.addService(SecureScanGattProfile.createSecureScanService())
            ?: Log.w(TAG, "Unable to create GATT server")
    }

    private fun stopServer() {
        bluetoothGattServer?.close()
    }

    private fun stopAdvertising() {
        val bluetoothLeAdvertiser: BluetoothLeAdvertiser? =
            bluetoothManager.adapter.bluetoothLeAdvertiser
        bluetoothLeAdvertiser?.let {
            it.stopAdvertising(advertiseCallback)
        } ?: Log.w(TAG, "Failed to create advertiser")
    }

    private fun startAdvertising() {
        val bluetoothLeAdvertiser: BluetoothLeAdvertiser? =
            bluetoothManager.adapter.bluetoothLeAdvertiser

        bluetoothLeAdvertiser?.let {
            val settings = AdvertiseSettings.Builder()
                .setAdvertiseMode(AdvertiseSettings.ADVERTISE_MODE_BALANCED)
                .setConnectable(true)
                .setTimeout(0)
                .setTxPowerLevel(AdvertiseSettings.ADVERTISE_TX_POWER_MEDIUM)
                .build()

            val data = AdvertiseData.Builder()
                //.setIncludeDeviceName(true)
                //.setIncludeTxPowerLevel(false)
                .addServiceUuid(ParcelUuid(SecureScanGattProfile.SECURESCANSERVICE))
                .build()

            val rdata = AdvertiseData.Builder()
                .setIncludeDeviceName(true)
                //.setIncludeTxPowerLevel(true)
                //.addServiceUuid(ParcelUuid(SecureScanGattProfile.SECURESCANSERVICE))
                .build()

            it.startAdvertising(settings, data, rdata, advertiseCallback)
        } ?: Log.w(TAG, "Failed to create advertiser")
    }

    class MyBinder(private val service: SecureScanBluetoothService) : Binder() {
        fun getService(): SecureScanBluetoothService = service
    }

    val binder = MyBinder(this)

    override fun onBind(intent: Intent): IBinder {
        return binder
    }

    override fun onStartCommand(intent: Intent?, flags: Int, startId: Int): Int {
        Log.i("SecureScan", "SERVICE STARTED ********************************")


        bluetoothManager = getSystemService(Context.BLUETOOTH_SERVICE) as BluetoothManager
        val bluetoothAdapter = bluetoothManager.adapter

        // Register for system Bluetooth events
        val filter = IntentFilter(BluetoothAdapter.ACTION_STATE_CHANGED)
        registerReceiver(bluetoothReceiver, filter)
        if (!bluetoothAdapter.isEnabled) {
            Log.d(TAG, "Bluetooth is currently disabled...enabling")
            bluetoothAdapter.enable()
        } else {
            Log.d(TAG, "Bluetooth enabled...starting services")
            startAdvertising()
            startServer()
        }

        return super.onStartCommand(intent, flags, startId)
    }


    private fun notifyMe() {

        val utils = NotificationUtils(this)

        val not = utils.getNotificationbuilder(
            "Secure Scan",
            "Request for access service over bluetooth started",
            NotificationUtils.CHANNEL_BLESERVICE
        )
            .build()

        utils.manager.notify(1001, not)
    }

    override fun onDestroy() {
        super.onDestroy()
        Log.i("SecureScan", "SERVICE DESTROYED ********************************")
    }

    private fun notifyUserForRequestedDocument() {
        val resultIntent = Intent(this, DocumentAccessRequest::class.java)

        var certInfo = accessRequest!!.certificate!!.getNameAndEmail()
        var doc = accessRequest!!.document!!

        resultIntent.putExtra("email", certInfo.email)
        resultIntent.putExtra("name", certInfo.name)
        resultIntent.putExtra("hostName", certInfo.hostName)
        resultIntent.putExtra("documentName", doc.name)
        resultIntent.putExtra("documentId", doc.id)
        resultIntent.putExtra("documentHash", doc.documentHash)

        accessRequestIntent = resultIntent

        val mainActivityIntent = Intent(this, MainActivity::class.java)

        // Create the TaskStackBuilder
        val resultPendingIntent: PendingIntent? = TaskStackBuilder.create(this).run {
            // Add the intent, which inflates the back stack
            addNextIntentWithParentStack(mainActivityIntent)

            // Get the PendingIntent containing the entire back stack
            getPendingIntent(
                0,
                PendingIntent.FLAG_UPDATE_CURRENT or PendingIntent.FLAG_IMMUTABLE
            )
        }

        var utils = NotificationUtils(this)
        val not = utils.getNotificationbuilder(
            "Secure Scan document access request",
            "${certInfo.name} (${certInfo.email}) from host '${certInfo.hostName}' is requesting access to document '$doc'. Click this notification to allow or deny the request.",
            NotificationUtils.CHANNEL_REQUESTACCESS
        )
            .setContentIntent(resultPendingIntent)
            .build()
        utils.manager.notify(1001, not)
    }

    fun setApproval(approved: Boolean) {
        Log.i(TAG, "Approved: $approved")
        if (approved) {
            status.setStatus(SecureScanGattProfile.STATUS_REQUEST_ACCEPTED)
        } else {
            status.setStatus(SecureScanGattProfile.STATUS_REQUEST_DENIED)
        }

        val returnValue = arrayOf(status.getStatus()).toByteArray()
        Log.i(TAG, " ********* APROVED GetStatus : ${returnValue.toHexString()} ... ")
    }
}