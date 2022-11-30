package nl.ou.securescan.crypto.newcertificate

import android.security.keystore.KeyGenParameterSpec
import android.security.keystore.KeyProperties
import nl.ou.securescan.crypto.CertificateManager
import java.security.KeyPair
import java.security.KeyPairGenerator
import java.security.cert.X509Certificate
import java.security.spec.RSAKeyGenParameterSpec
import java.time.LocalDate
import java.time.ZoneOffset
import java.util.*
import javax.security.auth.x500.X500Principal

class GenerateCertificateNoBC {

    private fun execute(name: String, email: String, keyPair: KeyPair): X509Certificate? {
        val startDate = Date()
        val endDate = Date.from(LocalDate.of(2099, 12, 31).atStartOfDay().toInstant(ZoneOffset.UTC))

        //1.2.840.113549.1.9.1

        val builder = KeyGenParameterSpec.Builder(
            CertificateManager.ALIAS,
            KeyProperties.PURPOSE_ENCRYPT or KeyProperties.PURPOSE_DECRYPT or KeyProperties.PURPOSE_VERIFY or KeyProperties.PURPOSE_SIGN
        )
            .setDigests(KeyProperties.DIGEST_SHA256)
            .setEncryptionPaddings(KeyProperties.ENCRYPTION_PADDING_RSA_PKCS1)
            .setCertificateSubject(X500Principal("CN=$email, O=$name"))
            .setKeyValidityStart(startDate)
            .setCertificateNotBefore(startDate)
            .setCertificateNotAfter(endDate)
            .setKeySize(2048)
            .setAttestationChallenge("hello world".toByteArray())
            .setBlockModes(KeyProperties.BLOCK_MODE_ECB)
            .setAlgorithmParameterSpec(RSAKeyGenParameterSpec(2048, RSAKeyGenParameterSpec.F4))
        //.setUserConfirmationRequired(true)

        /*if (Permissions().hasPermission(context, PackageManager.FEATURE_STRONGBOX_KEYSTORE)) {
            builder = builder.setIsStrongBoxBacked(true)
        }*/

        val spec = builder.build()

        val generator: KeyPairGenerator =
            KeyPairGenerator.getInstance(
                KeyProperties.KEY_ALGORITHM_RSA,
                CertificateManager.ANDROIDKEYSTORE
            )
        generator.initialize(spec)
        val keypair = generator.generateKeyPair()
        val privateKey = keypair.private

        /* var cert = getCertificate()
         removeCertificate()

         val keyStore = KeyStore.getInstance(CertificateManager.ANDROIDKEYSTORE)
         keyStore.load(null)
         keyStore.setKeyEntry(CertificateManager.ALIAS, privateKey, null, arrayOf(cert!!.certificate))*/

        return null
    }
}