package nl.ou.securescan.crypto

import android.security.keystore.KeyGenParameterSpec
import android.security.keystore.KeyProperties
import android.util.Log
import java.security.KeyPair
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

    fun HasCertificate(): Boolean {
        val keyStore = KeyStore.getInstance(ANDROIDKEYSTORE)
        keyStore.load(null)
        return keyStore.containsAlias(ALIAS)
    }

    fun GetCertificate(): Certificate? {
        val keyStore = KeyStore.getInstance(ANDROIDKEYSTORE)
        keyStore.load(null)
        return if (keyStore.containsAlias(ALIAS)) {
            keyStore.getCertificate(ALIAS)
        } else {
            null
        }
    }

    fun CreateCertificate(name: String, email: String) {
        val startDate = Date()
        val endDate = Date.from(LocalDate.of(2099, 12, 31).atStartOfDay().toInstant(ZoneOffset.UTC))

        val spec = KeyGenParameterSpec.Builder(
            ALIAS,
            KeyProperties.PURPOSE_ENCRYPT or KeyProperties.PURPOSE_DECRYPT or KeyProperties.PURPOSE_VERIFY or KeyProperties.PURPOSE_SIGN
        )
            //.setDigests(KeyProperties.DIGEST_SHA256, KeyProperties.DIGEST_SHA512)
            .setDigests(KeyProperties.DIGEST_SHA256)
            .setEncryptionPaddings(KeyProperties.ENCRYPTION_PADDING_RSA_PKCS1)
            .setCertificateSubject(X500Principal("EMAILADDRESS=$email, CN=$name"))
            .setKeyValidityStart(startDate)
            .setCertificateNotBefore(startDate)
            .setCertificateNotAfter(endDate)
            .setIsStrongBoxBacked(true)
            .setKeySize(2048)
            .setAlgorithmParameterSpec(RSAKeyGenParameterSpec(2048, RSAKeyGenParameterSpec.F4))
            .build()

        /*val spec = KeyPairGeneratorSpec.Builder(context)
            .setAlias("SSKEY1")
            .setKeySize(2048)
            .setKeyType(KeyProperties.KEY_ALGORITHM_RSA)
            .setSubject(X500Principal("CN=SSKEY1"))
            .setSerialNumber(BigInteger.TEN)
            .setStartDate(startDate)
            .setEndDate(endDate)
            .build()*/

        val generator: KeyPairGenerator =
            KeyPairGenerator.getInstance(KeyProperties.KEY_ALGORITHM_RSA, ANDROIDKEYSTORE)
        generator.initialize(spec)
        generator.generateKeyPair()
    }
}