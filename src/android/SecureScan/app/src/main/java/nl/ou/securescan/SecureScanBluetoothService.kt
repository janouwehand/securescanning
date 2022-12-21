package nl.ou.securescan

import android.Manifest.permission.BLUETOOTH_ADVERTISE
import android.Manifest.permission.BLUETOOTH_CONNECT
import android.app.Service
import android.bluetooth.*
import android.bluetooth.le.AdvertiseCallback
import android.bluetooth.le.AdvertiseData
import android.bluetooth.le.AdvertiseSettings
import android.bluetooth.le.BluetoothLeAdvertiser
import android.content.BroadcastReceiver
import android.content.Context
import android.content.Intent
import android.content.IntentFilter
import android.content.pm.PackageManager
import android.os.IBinder
import android.os.ParcelUuid
import android.util.Log
import nl.ou.securescan.helpers.NotificationUtils


class SecureScanBluetoothService : Service() {

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

    private var accessRequest: AccessRequest? = null

    fun initAccessRequest(
        device: BluetoothDevice,
        requestId: Int,
        responseNeeded: Boolean,
        value: ByteArray
    ) {
        accessRequest = AccessRequest()
        accessRequest!!.setSecureContainerSha1Hash(value)

        if (responseNeeded) {
            if (checkSelfPermission(BLUETOOTH_CONNECT) == PackageManager.PERMISSION_GRANTED) {
                bluetoothGattServer?.sendResponse(
                    device,
                    requestId,
                    BluetoothGatt.GATT_SUCCESS,
                    0, null
                )
            }
        }
    }

    fun getKey(
        device: BluetoothDevice,
        requestId: Int
    ) {

        val returnValue = "".toByteArray()

        if (checkSelfPermission(BLUETOOTH_CONNECT) == PackageManager.PERMISSION_GRANTED) {
            bluetoothGattServer?.sendResponse(
                device,
                requestId,
                BluetoothGatt.GATT_SUCCESS,
                0,
                returnValue
            )
        }

        accessRequest = null
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
            if (SecureScanGattProfile.GETKEY == characteristic.uuid) {
                getKey(device, requestId)
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
            if (SecureScanGattProfile.INITREQUEST == characteristic!!.uuid) {
                initAccessRequest(device!!, requestId, responseNeeded, value!!)
            }
        }
    }

    private fun startServer() {
        if (checkSelfPermission(BLUETOOTH_CONNECT) != PackageManager.PERMISSION_GRANTED) {
            Log.w(TAG, "** NO PERMISSION! BLUETOOTH_CONNECT")
            return
        }
        bluetoothGattServer = bluetoothManager.openGattServer(this, gattServerCallback)

        bluetoothGattServer?.addService(SecureScanGattProfile.createSecureScanService())
            ?: Log.w(TAG, "Unable to create GATT server")
    }

    private fun stopServer() {
        if (checkSelfPermission(BLUETOOTH_CONNECT) != PackageManager.PERMISSION_GRANTED) {
            Log.w(TAG, "** NO PERMISSION! BLUETOOTH_CONNECT")
            return
        }
        bluetoothGattServer?.close()
    }

    private fun stopAdvertising() {
        val bluetoothLeAdvertiser: BluetoothLeAdvertiser? =
            bluetoothManager.adapter.bluetoothLeAdvertiser
        bluetoothLeAdvertiser?.let {
            if (checkSelfPermission(BLUETOOTH_ADVERTISE) != PackageManager.PERMISSION_GRANTED) {
                Log.w(TAG, "** NO PERMISSION! BLUETOOTH_ADVERTISE")
                return
            }
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

            if (checkSelfPermission(BLUETOOTH_ADVERTISE) != PackageManager.PERMISSION_GRANTED) {
                Log.w(TAG, "** NO PERMISSION! BLUETOOTH_ADVERTISE")
                return
            }
            it.startAdvertising(settings, data, advertiseCallback)
        } ?: Log.w(TAG, "Failed to create advertiser")
    }


    override fun onBind(intent: Intent): IBinder? {
        return null
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
            if (checkSelfPermission(BLUETOOTH_CONNECT) != PackageManager.PERMISSION_GRANTED) {
                Log.w(TAG, "** NO PERMISSION! BLUETOOTH_CONNECT")
            } else
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
}