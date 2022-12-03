package nl.ou.securescan.crypto

import nl.ou.securescan.crypto.newcertificate.GenerateCertificateBC
import nl.ou.securescan.crypto.newcertificate.GenerateCertificateNoBC
import java.security.KeyStore
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

        //val cmc = GenerateCertificateBC()
        //val keypair = cmc.generateKeyPair()
        GenerateCertificateNoBC().execute(name, email)

        //keyStore.setCertificateEntry(ALIAS, cert)
        //keyStore.setKeyEntry(ALIAS, keypair.private, null, arrayOf(cert))
    }

    /*fun getSecretKey(): SecretKey? {
        val keyStore = KeyStore.getInstance(ANDROIDKEYSTORE)
        keyStore.load(null)
        val existingKey = keyStore.getEntry(ALIAS, null) as? KeyStore.SecretKeyEntry
        //return existingKey?.secretKey ?: createKey()
        return existingKey?.secretKey
    }*/
}