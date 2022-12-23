package nl.ou.securescan

import nl.ou.securescan.crypto.CertificateManager
import nl.ou.securescan.crypto.extensions.decryptData
import nl.ou.securescan.crypto.extensions.encryptData
import nl.ou.securescan.data.Document
import java.io.ByteArrayInputStream
import java.security.cert.CertificateFactory
import java.security.cert.X509Certificate

class AccessRequest {

    var secureContainerHash: ByteArray? = null
        private set

    var document: Document? = null
        private set

    var certificate: X509Certificate? = null
        private set

    fun setSecureContainerSha1Hash(hash: ByteArray) {
        secureContainerHash = hash
    }

    fun setDocument(doc: Document) {
        document = doc
    }

    fun setPublicCertificate(byteArray: ByteArray) {
        val certificateFactory = CertificateFactory.getInstance("X.509")
        val inputStream = ByteArrayInputStream(byteArray)
        certificate = certificateFactory.generateCertificate(inputStream) as X509Certificate
    }

    fun getKey(): ByteArray {
        var ssCert = CertificateManager().getCertificate()
        val symmetricPassword = ssCert!!.decryptData(document!!.documentPassword!!)

        val key = certificate!!.encryptData(symmetricPassword)

        return key
    }
}