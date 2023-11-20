package nl.ou.securescan.crypto

import nl.ou.securescan.crypto.newcertificate.GenerateCertificateNoBC
import java.io.ByteArrayInputStream
import java.security.KeyStore
import java.security.cert.CertificateFactory
import java.security.cert.X509Certificate

class CertificateManager {

    companion object {
        const val ANDROIDKEYSTORE = "AndroidKeyStore"
        const val ALIAS = "SecureScanMasterKey"
    }

    fun hasCertificate(): Boolean {
        val keyStore = KeyStore.getInstance(ANDROIDKEYSTORE)
        keyStore.load(null)
        return keyStore.containsAlias(ALIAS)
    }

    fun getCertificate(): X509Certificate? {
        val keyStore = KeyStore.getInstance(ANDROIDKEYSTORE)
        keyStore.load(null)

        return if (keyStore.containsAlias(ALIAS)) {
            val cert = keyStore.getCertificate(ALIAS)
            if (cert is X509Certificate) {
                return cert
            } else {
                null
            }
        } else {
            null
        }
    }

    fun removeCertificate() {
        val keyStore = KeyStore.getInstance(ANDROIDKEYSTORE)
        keyStore.load(null)
        if (keyStore.containsAlias(ALIAS)) {
            keyStore.deleteEntry(ALIAS)
        }
    }

    fun createCertificate(name: String, email: String) {
        val keyStore = KeyStore.getInstance(ANDROIDKEYSTORE)
        keyStore.load(null)
        GenerateCertificateNoBC().execute(name, email)
    }

    fun getCertificateFromByteArray(data: ByteArray): X509Certificate {
        val factory = CertificateFactory.getInstance("X.509")
        var cert = factory.generateCertificate(ByteArrayInputStream(data))
        return cert as X509Certificate
    }
}