package nl.ou.securescan

import android.bluetooth.BluetoothGattCharacteristic
import android.bluetooth.BluetoothGattService
import java.util.*

object SecureScanGattProfile {
    val SECURESCANSERVICE: UUID = UUID.fromString("00000999-1999-1999-8999-009999999999")
    val SENDSECURECONTAINERHASH: UUID = UUID.fromString("00000999-1999-1999-7999-009999999999")
    val PUBLICCERT: UUID = UUID.fromString("00000999-1999-1999-6999-009999999999")
    val GETKEY: UUID = UUID.fromString("00000999-1999-1999-5999-009999999999")
    val GETSTATUS: UUID = UUID.fromString("00000999-1999-1999-4999-009999999999")
    val GETNAME: UUID = UUID.fromString("00000999-1999-1999-4959-009999999999")

    const val STATUS_IDLE: Byte = 0xA0.toByte()
    const val STATUS_DOCUMENT_AVAILABLE: Byte = 0xE1.toByte()
    const val STATUS_DOCUMENT_NOT_AVAILABLE: Byte = 0xE2.toByte()
    const val STATUS_REQUEST_WAITFORUSER: Byte = 0xA1.toByte()
    const val STATUS_REQUEST_ACCEPTED: Byte = 0xA2.toByte()
    const val STATUS_REQUEST_DENIED: Byte = 0xA3.toByte()

    fun getStatusDescription(status: Byte): String {
        return when (status) {
            STATUS_IDLE -> "Idle"
            STATUS_DOCUMENT_AVAILABLE -> "Document available"
            STATUS_DOCUMENT_NOT_AVAILABLE -> "Document not available"
            STATUS_REQUEST_WAITFORUSER -> "Waiting for user response"
            STATUS_REQUEST_ACCEPTED -> "User approved request"
            STATUS_REQUEST_DENIED -> "User denied request"
            else -> "Unknown!"
        }
    }

    fun createSecureScanService(): BluetoothGattService {
        val service =
            BluetoothGattService(SECURESCANSERVICE, BluetoothGattService.SERVICE_TYPE_PRIMARY)

        val initRequest = BluetoothGattCharacteristic(
            SENDSECURECONTAINERHASH,
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

        val getStatus = BluetoothGattCharacteristic(
            GETSTATUS,
            BluetoothGattCharacteristic.PROPERTY_READ,
            BluetoothGattCharacteristic.PERMISSION_READ
        )

        val getName = BluetoothGattCharacteristic(
            GETSTATUS,
            BluetoothGattCharacteristic.PROPERTY_READ,
            BluetoothGattCharacteristic.PERMISSION_READ
        )

        service.addCharacteristic(initRequest)
        service.addCharacteristic(publicCert)
        service.addCharacteristic(getKey)
        service.addCharacteristic(getStatus)
        service.addCharacteristic(getName)

        return service
    }
}