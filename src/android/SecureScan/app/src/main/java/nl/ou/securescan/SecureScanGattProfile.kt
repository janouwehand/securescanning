package nl.ou.securescan

import android.bluetooth.BluetoothGattCharacteristic
import android.bluetooth.BluetoothGattService
import java.util.*

object SecureScanGattProfile {
    val SECURESCANSERVICE: UUID = UUID.fromString("00000999-1999-1999-8999-009999999999")
    val INITREQUEST: UUID = UUID.fromString("00000999-1999-1999-7999-009999999999")
    val PUBLICCERT: UUID = UUID.fromString("00000999-1999-1999-6999-009999999999")
    val GETKEY: UUID = UUID.fromString("00000999-1999-1999-5999-009999999999")

    fun createSecureScanService(): BluetoothGattService {
        val service =
            BluetoothGattService(SECURESCANSERVICE, BluetoothGattService.SERVICE_TYPE_PRIMARY)

        val initRequest = BluetoothGattCharacteristic(
            INITREQUEST,
            BluetoothGattCharacteristic.PROPERTY_WRITE,
            BluetoothGattCharacteristic.PERMISSION_WRITE
        )

        val publicCert = BluetoothGattCharacteristic(
            PUBLICCERT,
            BluetoothGattCharacteristic.PROPERTY_WRITE,
            BluetoothGattCharacteristic.PERMISSION_WRITE
        )

        val getKey = BluetoothGattCharacteristic(
            GETKEY,
            BluetoothGattCharacteristic.PROPERTY_READ,
            BluetoothGattCharacteristic.PERMISSION_READ
        )

        service.addCharacteristic(initRequest)
        service.addCharacteristic(publicCert)
        service.addCharacteristic(getKey)

        return service
    }
}