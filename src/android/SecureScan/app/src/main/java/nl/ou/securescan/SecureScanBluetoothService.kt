package nl.ou.securescan

import android.Manifest
import android.app.Service
import android.bluetooth.BluetoothAdapter
import android.bluetooth.BluetoothDevice
import android.bluetooth.BluetoothManager
import android.bluetooth.le.*
import android.content.Context
import android.content.Intent
import android.content.pm.PackageManager
import android.os.Build
import android.os.IBinder
import android.os.ParcelUuid
import android.util.Log
import android.widget.Toast
import androidx.core.app.ActivityCompat
import nl.ou.securescan.helpers.NotificationUtils
import java.util.*


class SecureScanBluetoothService : Service() {

    val serviceUUID: UUID = UUID.fromString("E7789128-CB1D-443C-9CFE-A25045D2465D")
    val TAG: String = "SecureScanLAMA"
    var currentAdvertisingSet: AdvertisingSet? = null

    override fun onBind(intent: Intent): IBinder? {
        return null
    }

    override fun onStartCommand(intent: Intent?, flags: Int, startId: Int): Int {
        Log.i("SecureScan", "SERVICE STARTED ********************************")
        startAdvertising3()
        return super.onStartCommand(intent, flags, startId)
    }

    private fun startAdvertising3() {
        val manager = this.getSystemService(Context.BLUETOOTH_SERVICE) as BluetoothManager
        val adapter = manager.adapter

        if (ActivityCompat.checkSelfPermission(
                this,
                Manifest.permission.BLUETOOTH_CONNECT
            ) != PackageManager.PERMISSION_GRANTED
        ) {
            // TODO: Consider calling
            //    ActivityCompat#requestPermissions
            // here to request the missing permissions, and then overriding
            //   public void onRequestPermissionsResult(int requestCode, String[] permissions,
            //                                          int[] grantResults)
            // to handle the case where the user grants the permission. See the documentation
            // for ActivityCompat#requestPermissions for more details.
            return
        }
        adapter.name = "SecureScan"
        var adv = adapter.bluetoothLeAdvertiser

        val advertiser: BluetoothLeAdvertiser = adapter.bluetoothLeAdvertiser
        val settingsBuilder = AdvertiseSettings.Builder()
            .setAdvertiseMode(AdvertiseSettings.ADVERTISE_MODE_BALANCED)
            .setTxPowerLevel(AdvertiseSettings.ADVERTISE_TX_POWER_LOW)
            .setConnectable(true)

        val pUuid = ParcelUuid(serviceUUID)

        val data = AdvertiseData.Builder()
            .setIncludeDeviceName(false)
            .addServiceUuid(pUuid)
            .setIncludeTxPowerLevel(true).build()

        val rdata = AdvertiseData.Builder()
            .setIncludeDeviceName(true)
            .addServiceData(pUuid, arrayOf<Byte>().toByteArray())
            .build()

        var callback: AdvertiseCallback = object :  AdvertiseCallback() {
            override fun onStartSuccess(settingsInEffect: AdvertiseSettings?) {
                super.onStartSuccess(settingsInEffect)

            }

            override fun onStartFailure(errorCode: Int) {
                super.onStartFailure(errorCode)
            }
        }

        advertiser.startAdvertising(settingsBuilder.build(), data, rdata, callback)
    }

    fun startAdvertising5(){
        val manager = this.getSystemService(Context.BLUETOOTH_SERVICE) as BluetoothManager
        val adapter = manager.adapter


    }

    fun startAdvertising4() {
        val advertiser = BluetoothAdapter.getDefaultAdapter().bluetoothLeAdvertiser

        BluetoothAdapter.getDefaultAdapter().name = "SecureScan"

        var parameters: AdvertisingSetParameters? = null
        if (Build.VERSION.SDK_INT >= Build.VERSION_CODES.O) {
            parameters = AdvertisingSetParameters.Builder()
                .setLegacyMode(true) // True by default, but set here as a reminder.
                .setConnectable(false)
                .setInterval(AdvertisingSetParameters.INTERVAL_HIGH)
                .setTxPowerLevel(AdvertisingSetParameters.TX_POWER_MEDIUM)
                .build()
        }
        val manufacturerData = byteArrayOf( //                0x12, 0x34,
            //                0x56, 0x66,
            0x41, 0x4d, 0x4f, 0x4c
        )
        val testData = "abcdefghij"
        val testData1 = testData.toByteArray()
        val pUuid = ParcelUuid(serviceUUID)
        val data = AdvertiseData.Builder()
            .addManufacturerData(1, testData1)
            .setIncludeDeviceName(true)
            .build()

        val that = this

        //.addServiceData( pUuid, "Data".getBytes(Charset.forName("UTF-8") ) )
        var callback: AdvertisingSetCallback? = null
        if (Build.VERSION.SDK_INT >= Build.VERSION_CODES.O) {
            callback = object : AdvertisingSetCallback() {
                override fun onAdvertisingSetStarted(
                    advertisingSet: AdvertisingSet,
                    txPower: Int,
                    status: Int
                ) {
                    super.onAdvertisingSetStarted(advertisingSet, txPower, status)
                    Log.d(
                        TAG, "onAdvertisingSetStarted(): txPower:" + txPower + " , status: "
                                + status
                    )

                    // After onAdvertisingSetStarted callback is called, you can modify the
                    // advertising data and scan response data:
                    if (ActivityCompat.checkSelfPermission(
                            that,
                            Manifest.permission.BLUETOOTH_ADVERTISE
                        ) != PackageManager.PERMISSION_GRANTED
                    ) {
                        // TODO: Consider calling
                        //    ActivityCompat#requestPermissions
                        // here to request the missing permissions, and then overriding
                        //   public void onRequestPermissionsResult(int requestCode, String[] permissions,
                        //                                          int[] grantResults)
                        // to handle the case where the user grants the permission. See the documentation
                        // for ActivityCompat#requestPermissions for more details.
                        return
                    }
                    advertisingSet.setAdvertisingData(
                        AdvertiseData.Builder().addManufacturerData(67, testData1)
                            .setIncludeDeviceName(true)
                            .addServiceUuid(pUuid)
                            .setIncludeTxPowerLevel(true).build()
                    )
                    // Wait for onAdvertisingDataSet callback...
                    val pUuid = ParcelUuid(serviceUUID)
                    advertisingSet.setScanResponseData(
                        AdvertiseData.Builder().addServiceUuid(pUuid).build()
                    )
                    Log.d(TAG, "UUID$pUuid")
                    //
                    // Wait for onScanResponseDataSet callback...
                    Log.d(TAG, data.toString() + status)

//                    currentAdvertisingSet.setScanResponseData(new AdvertiseData.Builder());
                }

                override fun onAdvertisingSetStopped(advertisingSet: AdvertisingSet) {
                    super.onAdvertisingSetStopped(advertisingSet)
                    Log.d(TAG, "onAdvertisingSetStopped():")
                }

                override fun onAdvertisingEnabled(
                    advertisingSet: AdvertisingSet,
                    enable: Boolean,
                    status: Int
                ) {
                    super.onAdvertisingEnabled(advertisingSet, enable, status)
                }

                override fun onAdvertisingDataSet(advertisingSet: AdvertisingSet, status: Int) {
                    super.onAdvertisingDataSet(advertisingSet, status)
                    Log.d(TAG, "onAdvertisingDataSet() :status:$status")
                }

                override fun onScanResponseDataSet(advertisingSet: AdvertisingSet, status: Int) {
                    super.onScanResponseDataSet(advertisingSet, status)
                    Log.d(TAG, "onScanResponseDataSet(): status:$status")
                }
            }
        }
        if (Build.VERSION.SDK_INT >= Build.VERSION_CODES.O) {
            if (ActivityCompat.checkSelfPermission(
                    this,
                    Manifest.permission.BLUETOOTH_ADVERTISE
                ) != PackageManager.PERMISSION_GRANTED
            ) {
                // TODO: Consider calling
                //    ActivityCompat#requestPermissions
                // here to request the missing permissions, and then overriding
                //   public void onRequestPermissionsResult(int requestCode, String[] permissions,
                //                                          int[] grantResults)
                // to handle the case where the user grants the permission. See the documentation
                // for ActivityCompat#requestPermissions for more details.
                return
            }
            advertiser.startAdvertisingSet(parameters, data, null, null, null, callback)
            Toast.makeText(this, "Data$data", Toast.LENGTH_LONG).show()
        }

        // When done with the advertising:
//        if (Build.VERSION.SDK_INT >= Build.VERSION_CODES.O) {
//            advertiser.stopAdvertisingSet(callback);
//       }

        //
    }

    private fun startAdvertising2() {
        val bluetoothAdapter = BluetoothAdapter.getDefaultAdapter()

        val advertiser: BluetoothLeAdvertiser = bluetoothAdapter.bluetoothLeAdvertiser

        val dataBuilder = AdvertiseData.Builder()
        //Define a service UUID according to your needs
        //Define a service UUID according to your needs
        dataBuilder.addServiceUuid(ParcelUuid(serviceUUID))
        //dataBuilder.setIncludeDeviceName(true)

        val settingsBuilder = AdvertiseSettings.Builder()
        settingsBuilder.setAdvertiseMode(AdvertiseSettings.ADVERTISE_MODE_BALANCED)
        settingsBuilder.setTimeout(0)

        //Use the connectable flag if you intend on opening a Gatt Server
        //to allow remote connections to your device.

        //Use the connectable flag if you intend on opening a Gatt Server
        //to allow remote connections to your device.
        settingsBuilder.setConnectable(true)

        val advertiseCallback: AdvertiseCallback = object : AdvertiseCallback() {
            override fun onStartSuccess(settingsInEffect: AdvertiseSettings) {
                super.onStartSuccess(settingsInEffect)
                Log.i("SecureScan", "onStartSuccess: ")
            }

            override fun onStartFailure(errorCode: Int) {
                super.onStartFailure(errorCode)
                Log.e("SecureScan", "onStartFailure: $errorCode")
            }
        }
        if (ActivityCompat.checkSelfPermission(
                this,
                Manifest.permission.BLUETOOTH_ADVERTISE
            ) != PackageManager.PERMISSION_GRANTED
        ) {
            // TODO: Consider calling
            //    ActivityCompat#requestPermissions
            // here to request the missing permissions, and then overriding
            //   public void onRequestPermissionsResult(int requestCode, String[] permissions,
            //                                          int[] grantResults)
            // to handle the case where the user grants the permission. See the documentation
            // for ActivityCompat#requestPermissions for more details.
            return
        }

        advertiser.startAdvertising(
            settingsBuilder.build(),
            dataBuilder.build(),
            advertiseCallback
        )
    }

    private fun startAdvertising() {
        val adapter = BluetoothAdapter.getDefaultAdapter()
        // Check if all features are supported
        if (!adapter.isLe2MPhySupported) {
            Log.e("SecureScan", "2M PHY not supported!");
            return
        }

        if (!adapter.isLeExtendedAdvertisingSupported) {
            Log.e("SecureScan", "LE Extended Advertising not supported!");
            return
        }

        val advertiser = adapter.bluetoothLeAdvertiser
        val maxDataLength = adapter.leMaximumAdvertisingDataLength

        val parameters = AdvertisingSetParameters.Builder()
            .setLegacyMode(false)
            .setAnonymous(true)
            .setInterval(AdvertisingSetParameters.INTERVAL_HIGH)
            .setTxPowerLevel(AdvertisingSetParameters.TX_POWER_MAX)
            .setPrimaryPhy(BluetoothDevice.PHY_LE_1M)
            .setSecondaryPhy(BluetoothDevice.PHY_LE_2M)

        val responseData = AdvertiseData.Builder()
            .setIncludeDeviceName(true);

        val data = AdvertiseData.Builder()
            .addServiceUuid(ParcelUuid(serviceUUID))
            .addManufacturerData(4666, "HELLO".toByteArray())
            .addServiceData(
                ParcelUuid(serviceUUID),
                "You should be able to fit large amounts of data up to maxDataLength. This goes up to 1650 bytes. For legacy advertising this would not work".toByteArray()
            )
            .setIncludeDeviceName(true)
            .build()

        Log.i("SecureScanADDRESS", "$*************** ADDRESS: ${adapter.address}")

        val callback: AdvertisingSetCallback = object : AdvertisingSetCallback() {
            override fun onAdvertisingSetStarted(
                advertisingSet: AdvertisingSet,
                txPower: Int,
                status: Int
            ) {
                Log.i(
                    "SecureScan", "onAdvertisingSetStarted(): txPower:" + txPower + " , status: "
                            + status
                )

                currentAdvertisingSet = advertisingSet
            }

            override fun onAdvertisingSetStopped(advertisingSet: AdvertisingSet) {
                Log.i("SecureScan", "onAdvertisingSetStopped():")
            }
        }

        //adapter.setName("JANO!!")

        if (ActivityCompat.checkSelfPermission(
                this,
                Manifest.permission.BLUETOOTH_ADVERTISE
            ) != PackageManager.PERMISSION_GRANTED
        ) {
            Log.i("SecureScan", "*** NO BLUETOOTH PERMISSION !!")
        } else {
            advertiser.startAdvertisingSet(
                parameters.build(),
                data,
                responseData.build(),
                null,
                null,
                callback
            )
            notifyMe()
        }
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