package nl.ou.securescan.crypto

import android.content.Context
import android.content.pm.PackageManager
import android.security.keystore.KeyGenParameterSpec
import android.security.keystore.KeyProperties
import nl.ou.securescan.abilities.Permissions
import java.security.KeyPairGenerator
import java.security.KeyStore
import java.security.cert.Certificate
import java.security.spec.RSAKeyGenParameterSpec
import java.time.LocalDate
import java.time.ZoneOffset
import java.util.*
import javax.security.auth.x500.X500Principal

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

    fun getCertificate(): Certificate? {
        val keyStore = KeyStore.getInstance(ANDROIDKEYSTORE)
        keyStore.load(null)
        return if (keyStore.containsAlias(ALIAS)) {
            keyStore.getCertificate(ALIAS)
        } else {
            null
        }
    }

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