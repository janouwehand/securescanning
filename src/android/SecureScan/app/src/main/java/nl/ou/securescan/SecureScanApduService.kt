package nl.ou.securescan

import android.nfc.cardemulation.HostApduService
import android.os.Bundle
import android.util.Log
import nl.ou.securescan.crypto.CertificateManager
import nl.ou.securescan.crypto.extensions.getPrivateKey
import nl.ou.securescan.crypto.extensions.toHexString
import java.security.PrivateKey
import java.security.Signature
import java.security.cert.X509Certificate


class SecureScanApduService : HostApduService() {

    private var challengeSignature: ByteArray? = null
    private val hasCertificate: Boolean
    private val x509: X509Certificate?
    private val privateKey: PrivateKey?

    private var securecontainerhash: ByteArray? = null
    private var securecontainerpassword: ByteArray? = null

    init {
        val cm = CertificateManager()
        hasCertificate = cm.hasCertificate()
        x509 = if (hasCertificate) cm.getCertificate()!! else null
        privateKey = if (hasCertificate) x509!!.getPrivateKey() else null
    }

    private fun isSelectApdu(apdu: ByteArray?): Boolean =
        apdu!!.size >= 2 && apdu[0] === 0.toByte() && apdu[1] === 0xa4.toByte()

    override fun processCommandApdu(apdu: ByteArray?, extra: Bundle?): ByteArray {

        val str = apdu!!.toHexString()
        Log.i("SecureScan", "Received APDU: $str")

        return if (isSelectApdu(apdu)) {
            val ret = "APPV://1.0.0"
            Log.i("SecureScan", ret)
            val bs = combineResult(ret.encodeToByteArray())
            bs
        } else {
            val (bs, sw1, sw2) = process(apdu)
            val result = bs.plus(sw1).plus(sw2)
            result
        }
    }

    @Suppress("ArrayInDataClass")
    data class ProcessResult(val Data: ByteArray, val sw1: Byte, val sw2: Byte)

    private fun process(apdu: ByteArray): ProcessResult {
        val instruction = apdu[1]
        val block = apdu[2]
        var blockInfo = apdu[3]
        val data = getDataFromAPDU(apdu)

        return when (instruction) {
            0x50.toByte() -> ProcessResult(processGetKey(block.toInt()), instruction, block)
            0x60.toByte() -> ProcessResult(
                processGetChallengeResult(data!!, block.toInt()), instruction, block
            )
            0x80.toByte() -> ProcessResult(
                processRetrieveSecureContainerHash(data!!),
                instruction,
                block
            )
            0x90.toByte() -> ProcessResult(
                processRetrieveSecureContainerPassword(data!!, blockInfo),
                instruction,
                block
            )
            else -> ProcessResult(byteArrayOf(), 0x00, 0x00)
        }
    }

    private fun getDataFromAPDU(apdu: ByteArray): ByteArray? {
        if (apdu.size <= 9) {
            return null
        }

        // 00 50 04 00 00 00 00
        val data = apdu.copyOfRange(7, apdu.size - 2)
        Log.i("SecureScan", "Received data: ${data.toHexString()} (size: ${data.size} byes)")
        return data
    }

    @Suppress("ArrayInDataClass")
    private fun processGetChallengeResult(data: ByteArray, block: Int): ByteArray {
        Log.i(
            "SecureScan",
            "Executing challenge. Block $block. Data length: ${data.size}"
        )

        if (block == 1) {
            val privateKey = privateKey
            val privateSignature: Signature = Signature.getInstance("SHA256withRSA")
            privateSignature.initSign(privateKey)
            privateSignature.update(data)
            challengeSignature = privateSignature.sign()
            Log.i(
                "SecureScan",
                "Challenge result size: ${challengeSignature!!.size}. value: ${challengeSignature!!.toHexString()}"
            )
        }

        @Suppress("ArrayInDataClass")
        return sliceData(challengeSignature!!, block)
    }

    private fun sliceData(data: ByteArray, block: Int): ByteArray {
        val size = 250
        val fromIndex = (block - 1) * size

        var toIndex = fromIndex + size - 1

        if (toIndex > data.size) {
            toIndex = data.size - 1
        }

        if (toIndex <= fromIndex) {
            Log.i("SecureScan", "Geen bytes meer over")
            return arrayOf<Byte>().toByteArray()
        }
        Log.i("SecureScan", "Sending bytes $fromIndex to $toIndex (size: ${data.size})")

        return data.sliceArray(fromIndex..toIndex)
    }

    private fun processGetKey(block: Int): ByteArray = sliceData(x509!!.encoded, block)

    private fun combineResult(data: ByteArray, sw1: Byte = 0x00, sw2: Byte = 0x00): ByteArray =
        data.plus(sw1).plus(sw2)

    override fun onDeactivated(p0: Int) {
        Log.i("SecureScan", "deactivated!")

        this.securecontainerhash = null
        this.securecontainerpassword = null
    }

    private fun processRetrieveSecureContainerHash(hash: ByteArray): ByteArray {
        this.securecontainerhash = hash

        Log.i("SecureScan", "Hash of secure container received: ${hash.toHexString()}")

        if (this.securecontainerpassword != null) {
            storeLicense()
        }

        return arrayOf<Byte>().toByteArray()
    }

    private fun processRetrieveSecureContainerPassword(
        keyPart: ByteArray,
        blockInfo: Byte
    ): ByteArray {

        Log.i("SecureScan", "Receive password block ${keyPart.toHexString()}")

        if (this.securecontainerpassword == null) {
            this.securecontainerpassword = keyPart
        } else {
            this.securecontainerpassword = this.securecontainerpassword!!.plus(keyPart)
        }

        if (blockInfo == 0xFF.toByte() && this.securecontainerpassword != null) {
            storeLicense()
        }

        return arrayOf<Byte>().toByteArray()
    }

    private fun storeLicense() {
        this.securecontainerhash = null
        this.securecontainerpassword = null
    }
}