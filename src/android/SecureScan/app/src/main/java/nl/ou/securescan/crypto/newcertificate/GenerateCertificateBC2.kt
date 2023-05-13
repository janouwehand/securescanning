package nl.ou.securescan.crypto.newcertificate

import android.security.keystore.KeyGenParameterSpec
import android.security.keystore.KeyProperties
import org.bouncycastle.asn1.ASN1ObjectIdentifier
import org.bouncycastle.asn1.DEROctetString
import org.bouncycastle.asn1.x500.X500Name
import org.bouncycastle.asn1.x509.SubjectPublicKeyInfo
import org.bouncycastle.cert.X509v3CertificateBuilder
import org.bouncycastle.operator.ContentSigner
import org.bouncycastle.operator.jcajce.JcaContentSignerBuilder
import java.io.ByteArrayInputStream
import java.math.BigInteger
import java.security.KeyPair
import java.security.KeyPairGenerator
import java.security.KeyStore
import java.security.cert.CertificateFactory
import java.security.cert.X509Certificate
import java.util.*
import java.util.concurrent.TimeUnit
import javax.security.auth.x500.X500Principal


class GenerateCertificateBC2 {

    fun execute(name: String, email: String): X509Certificate? {
        val keyPairGenerator: KeyPairGenerator =
            KeyPairGenerator.getInstance(KeyProperties.KEY_ALGORITHM_EC, "AndroidKeyStore")

        val keySpecBuilder = KeyGenParameterSpec.Builder(
            "customKey",
            KeyProperties.PURPOSE_SIGN or KeyProperties.PURPOSE_VERIFY
        )
            .setDigests(KeyProperties.DIGEST_SHA256, KeyProperties.DIGEST_SHA512)
            .setSignaturePaddings(KeyProperties.SIGNATURE_PADDING_RSA_PKCS1)
            .setCertificateSubject(X500Principal("CN=custom"))
            .setCertificateSerialNumber(BigInteger.valueOf(1))
            .setCertificateNotBefore(Calendar.getInstance().getTime())
            .setCertificateNotAfter(Calendar.getInstance().getTime())

        keyPairGenerator.initialize(keySpecBuilder.build())
        val keyPair: KeyPair = keyPairGenerator.generateKeyPair()


        val certBuilder = X509v3CertificateBuilder(
            X500Name("CN=custom"),
            BigInteger.valueOf(1),
            Date(),
            Date(System.currentTimeMillis() + TimeUnit.DAYS.toMillis(365)),
            X500Name("CN=custom"),
            SubjectPublicKeyInfo.getInstance(keyPair.public.encoded)
        )

// Add extension to the certificate

// Add extension to the certificate
        /*certBuilder.addExtension(
            ASN1ObjectIdentifier("3.1.1"),
            false,
            DEROctetString("customValue".toByteArray())
        )*/

// Sign the certificate with the private key

// Sign the certificate with the private key
        val signer: ContentSigner =
            JcaContentSignerBuilder("SHA256WithECDSA").build(keyPair.private)
        val certHolder = certBuilder.build(signer)

// Convert the certificate from X509CertificateHolder to X509Certificate

// Convert the certificate from X509CertificateHolder to X509Certificate
        val certFactory: CertificateFactory = CertificateFactory.getInstance("X.509")
        val certificate = certFactory.generateCertificate(ByteArrayInputStream(certHolder.encoded))

        val keyStore: KeyStore = KeyStore.getInstance("AndroidKeyStore")
        keyStore.load(null)
        keyStore.setCertificateEntry("customKey", certificate)

        return certificate as X509Certificate
    }

}