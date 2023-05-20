package nl.ou.securescan

import android.nfc.cardemulation.HostApduService
import android.os.Bundle
import android.util.Log
import kotlinx.coroutines.runBlocking
import nl.ou.securescan.crypto.CertificateManager
import nl.ou.securescan.crypto.extensions.*
import nl.ou.securescan.data.Document
import nl.ou.securescan.data.DocumentDatabase
import nl.ou.securescan.state.EnrollingState
import java.security.Signature
import java.security.cert.X509Certificate
import java.time.ZonedDateTime


class SecureScanApduService : HostApduService() {

    private val EnrollMessage1SendX509OfMFP: Byte = 0x51
    private val EnrollMessage2RetrieveX509OfSmartphone: Byte = 0x52
    private val EnrollMessage3SendBindingSignatureToMFP: Byte = 0x53
    private val EnrollMessage4RetrieveBindingSignatureFromSmartphone: Byte = 0x54

    private var challengeSignature: ByteArray? = null
    private val hasCertificate: Boolean
    private val x509: X509Certificate?

    private var securecontainerhash: ByteArray? = null
    private var securecontainerpassword: ByteArray? = null
    private var symmetricCipherTextPart: ByteArray? = null
    private var signaturePart: ByteArray? = null

    private var x509Encrypted: ByteArray? = null

    init {
        val cm = CertificateManager()
        hasCertificate = cm.hasCertificate()
        x509 = if (hasCertificate) cm.getCertificate()!! else null
    }

    private fun isSelectApdu(apdu: ByteArray?): Boolean =
        apdu!!.size >= 2 && apdu[0] == 0.toByte() && apdu[1] == 0xa4.toByte()

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
        val blockInfo = apdu[3]
        val data = getDataFromAPDU(apdu)

        return when (instruction) {
            EnrollMessage1SendX509OfMFP -> ProcessResult(
                processEnrollMessage1SendX509OfMFP(data!!, blockInfo),
                instruction,
                block
            )
            EnrollMessage2RetrieveX509OfSmartphone -> ProcessResult(
                processEnrollMessage2RetrieveX509OfSmartphone(block.toInt()),
                instruction,
                block
            )
            EnrollMessage3SendBindingSignatureToMFP -> ProcessResult(
                processEnrollMessage3SendBindingSignatureToMFP(data!!, blockInfo),
                instruction,
                block
            )
            EnrollMessage4RetrieveBindingSignatureFromSmartphone -> ProcessResult(
                processEnrollMessage4RetrieveBindingSignatureFromSmartphone(data!!, blockInfo),
                instruction,
                block
            )
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
            0x91.toByte() -> ProcessResult(
                processStoreDocument(),
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
            val privateKey = x509!!.getPrivateKey()

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

        if (toIndex >= data.size) {
            toIndex = data.size - 1
        }

        if (toIndex <= fromIndex) {
            Log.i("SecureScan", "Geen bytes meer over")
            return arrayOf<Byte>().toByteArray()
        }
        Log.i("SecureScan", "Sending bytes $fromIndex to $toIndex (size: ${data.size})")

        return data.sliceArray(fromIndex..toIndex)
    }

    private fun processGetKey(block: Int): ByteArray {
        if (block == 1) {
            x509Encrypted = x509!!.encoded.encryptAES256GCM(EnrollingState.qrCodeKey!!)
        }
        return sliceData(x509Encrypted!!, block)
    }

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

        return arrayOf<Byte>().toByteArray()
    }

    private fun processEnrollMessage1SendX509OfMFP(
        symmetricCipherTextPart: ByteArray,
        blockInfo: Byte
    ): ByteArray {

        Log.i(
            "SecureScan",
            "Receive symmetricCipherTextPart block ${symmetricCipherTextPart.toHexString()}"
        )

        if (this.symmetricCipherTextPart == null) {
            this.symmetricCipherTextPart = symmetricCipherTextPart
        } else {
            this.symmetricCipherTextPart =
                this.symmetricCipherTextPart!!.plus(symmetricCipherTextPart)
        }

        if (blockInfo == 0xFF.toByte()) {
            Log.i(
                "SecureScan",
                "This was the last block. Value:  ${this.symmetricCipherTextPart!!.toHexString()}"
            )

            var x509OfMFP =
                this.symmetricCipherTextPart!!.decryptAES256GCM(EnrollingState.qrCodeKey!!)
            Log.i("SecureScan", "Decrypted:  ${x509OfMFP!!.toHexString()}")

            var cert = CertificateManager().getCertificateFromByteArray(x509OfMFP)
            var subject = cert.subjectX500Principal.toString()

            EnrollingState.mfpCertificate = cert

            Log.i("SecureScan", "subject:  ${subject}")
        }

        return arrayOf<Byte>(0xAA.toByte()).toByteArray()
    }

    private fun processEnrollMessage2RetrieveX509OfSmartphone(block: Int): ByteArray {
        if (block == 1) {
            x509Encrypted = x509!!.encoded.encryptAES256GCM(EnrollingState.qrCodeKey!!)
        }
        return sliceData(x509Encrypted!!, block)
    }

    private fun processEnrollMessage3SendBindingSignatureToMFP(
        symmetricCipherTextPart: ByteArray,
        blockInfo: Byte
    ): ByteArray {

        if (this.signaturePart == null) {
            this.signaturePart = symmetricCipherTextPart
        } else {
            this.signaturePart =
                this.signaturePart!!.plus(symmetricCipherTextPart)
        }

        Log.i("SecureScan", "Part of signature received:  ${this.signaturePart!!.toHexString()}")

        if (blockInfo == 0xFF.toByte()) {
            Log.i("SecureScan", "Signature received:  ${this.signaturePart!!.toHexString()}")

            // Let's see if we can verify the signature.
            val data = EnrollingState.mfpCertificate!!.encoded.plus(x509!!.encoded)
            val valid = EnrollingState.mfpCertificate!!.verifySignature(data, this.signaturePart!!)

            Log.i("SecureScan", "VALID?:  $valid")

            return if (valid) arrayOf(constants.AFFIRMATIVE).toByteArray() else arrayOf(constants.NEGATIVE).toByteArray()
        }

        return arrayOf(constants.WAITINGFORMOREDATA).toByteArray()
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

        return arrayOf<Byte>().toByteArray()
    }

    private fun processStoreDocument(): ByteArray {
        val documentId = storeLicense()
        return documentId.toString().toByteArray()
    }

    private fun storeLicense(): Int {
        val pwd = x509!!.decryptData(securecontainerpassword!!)

        Log.i("SecureScan", "Storing license, password: ${pwd.toHexString()}")

        val db = DocumentDatabase.getDatabase(this.baseContext)
        val dao = db.documentDao()

        var documentId = 0

        runBlocking {
            dao.insert(
                Document(
                    null,
                    "Scanned document",
                    ZonedDateTime.now().toString(),
                    securecontainerhash,
                    securecontainerpassword
                )
            )

            documentId = dao.getLastDocumentId()
        }

        this.securecontainerhash = null
        this.securecontainerpassword = null

        return documentId
    }
}