package nl.ou.securescan.crypto

import android.content.Context
import android.content.pm.PackageManager
import android.os.Build
import android.security.keystore.KeyGenParameterSpec
import android.security.keystore.KeyProperties
import androidx.annotation.RequiresApi
import nl.ou.securescan.abilities.Permissions
import java.security.KeyPairGenerator
import java.security.KeyStore
import java.security.cert.X509Certificate
import java.security.spec.RSAKeyGenParameterSpec
import java.time.LocalDate
import java.time.ZoneOffset
import java.util.*
import java.util.regex.Matcher
import java.util.regex.Pattern
import javax.security.auth.x500.X500Principal

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

    @RequiresApi(Build.VERSION_CODES.P)
    fun createCertificate(context: Context, name: String, email: String) {
        val startDate = Date()
        val endDate = Date.from(LocalDate.of(2099, 12, 31).atStartOfDay().toInstant(ZoneOffset.UTC))

        //1.2.840.113549.1.9.1

        var builder = KeyGenParameterSpec.Builder(
            ALIAS,
            KeyProperties.PURPOSE_ENCRYPT or KeyProperties.PURPOSE_DECRYPT or KeyProperties.PURPOSE_VERIFY or KeyProperties.PURPOSE_SIGN
        )
            .setDigests(KeyProperties.DIGEST_SHA256, KeyProperties.DIGEST_SHA512)
            .setEncryptionPaddings(KeyProperties.ENCRYPTION_PADDING_RSA_PKCS1)
            .setCertificateSubject(X500Principal("CN=$email, O=$name"))
            .setKeyValidityStart(startDate)
            .setCertificateNotBefore(startDate)
            .setCertificateNotAfter(endDate)
            .setKeySize(2048)
            .setAlgorithmParameterSpec(RSAKeyGenParameterSpec(2048, RSAKeyGenParameterSpec.F4))
            .setUserConfirmationRequired(true)

        if (Permissions().hasPermission(context, PackageManager.FEATURE_STRONGBOX_KEYSTORE)) {
            builder = builder.setIsStrongBoxBacked(true)
        }

        val spec = builder.build()

        val generator: KeyPairGenerator =
            KeyPairGenerator.getInstance(KeyProperties.KEY_ALGORITHM_RSA, ANDROIDKEYSTORE)
        generator.initialize(spec)
        generator.generateKeyPair()
    }
}