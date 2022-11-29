package nl.ou.securescan.crypto

import nl.ou.securescan.crypto.newcertificate.GenerateCertificateBC
import java.security.Key
import java.security.KeyStore
import java.security.cert.X509Certificate
import java.util.regex.Matcher
import java.util.regex.Pattern

data class CertificateInfo(val name: String, val email: String, val certificate: X509Certificate)

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

    private fun extractCertificateInfo(cert: X509Certificate): CertificateInfo {
        val subject = cert.subjectDN.name

        fun getName(): String {
            val pattern = Pattern.compile("O=(.*?)(?:,|\$)")
            val matcher: Matcher = pattern.matcher(subject)
            return if (matcher.find()) {
                matcher.group(1)!!
            } else {
                ""
            }
        }

        fun getEmail(): String {
            val pattern = Pattern.compile("CN=(.*?)(?:,|\$)")
            val matcher: Matcher = pattern.matcher(subject)
            return if (matcher.find()) {
                matcher.group(1)!!
            } else {
                ""
            }
        }

        var name = getName()
        var email = getEmail()

        return CertificateInfo(name, email, cert)
    }

    fun getPrivateKey(): Key? {
        val keyStore = KeyStore.getInstance(ANDROIDKEYSTORE)
        keyStore.load(null)

        if (keyStore.containsAlias(ALIAS)) {
            val key = keyStore.getKey(ALIAS, null)
            return key
        } else {
            return null
        }
    }

    fun getCertificateInfo(): CertificateInfo? {
        val keyStore = KeyStore.getInstance(ANDROIDKEYSTORE)
        keyStore.load(null)

        return if (keyStore.containsAlias(ALIAS)) {
            val cert = keyStore.getCertificate(ALIAS)
            if (cert is X509Certificate) {
                return extractCertificateInfo(cert)
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

        val cmc = GenerateCertificateBC()
        val keypair = cmc.generateKeyPair()
        val cert = GenerateCertificateBC().execute(keypair, name, email)

        keyStore.setCertificateEntry(ALIAS, cert)
        keyStore.setKeyEntry(ALIAS, keypair.private, null, arrayOf(cert))
    }

    /*fun getSecretKey(): SecretKey? {
        val keyStore = KeyStore.getInstance(ANDROIDKEYSTORE)
        keyStore.load(null)
        val existingKey = keyStore.getEntry(ALIAS, null) as? KeyStore.SecretKeyEntry
        //return existingKey?.secretKey ?: createKey()
        return existingKey?.secretKey
    }*/
}