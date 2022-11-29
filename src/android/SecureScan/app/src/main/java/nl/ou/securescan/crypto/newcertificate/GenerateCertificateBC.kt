package nl.ou.securescan.crypto.newcertificate

import android.security.keystore.KeyProperties
import org.bouncycastle.asn1.x500.X500Name
import org.bouncycastle.asn1.x509.Extension
import org.bouncycastle.asn1.x509.KeyUsage
import org.bouncycastle.asn1.x509.SubjectPublicKeyInfo
import org.bouncycastle.cert.X509v3CertificateBuilder
import org.bouncycastle.cert.jcajce.JcaX509CertificateConverter
import org.bouncycastle.jce.provider.BouncyCastleProvider
import org.bouncycastle.operator.ContentSigner
import org.bouncycastle.operator.jcajce.JcaContentSignerBuilder
import java.math.BigInteger
import java.security.*
import java.security.cert.X509Certificate
import java.time.LocalDate
import java.time.ZoneOffset
import java.util.*


class GenerateCertificateBC {

    fun execute(keyPair: KeyPair, name: String, email: String): X509Certificate {
        return createSelfSignedCertificate(keyPair, name, email)
    }

    fun generateKeyPair(): KeyPair {
        val kpg = KeyPairGenerator.getInstance(KeyProperties.KEY_ALGORITHM_RSA)
        kpg.initialize(2048)
        return kpg.genKeyPair()
    }

    // transform to base64 with begin and end line
    fun publicKeyToPem(publicKey: PublicKey): String {
        val base64PubKey = Base64.getEncoder().encodeToString(publicKey.encoded)

        return "-----BEGIN PUBLIC KEY-----\n" +
                base64PubKey.replace("(.{64})".toRegex(), "$1\n") +
                "\n-----END PUBLIC KEY-----\n"
    }

    fun privateKeyToPem(privateKey: PrivateKey): String {
        val base64PubKey = Base64.getEncoder().encodeToString(privateKey.encoded)

        return "-----BEGIN PRIVATE KEY-----\n" +
                base64PubKey.replace("(.{64})".toRegex(), "$1\n") +
                "\n-----END PRIVATE KEY-----\n"
    }


    fun certificateToPem(certificate: X509Certificate): String {
        val base64PubKey = Base64.getEncoder().encodeToString(certificate.encoded)

        return "-----BEGIN CERTIFICATE-----\n" +
                base64PubKey.replace("(.{64})".toRegex(), "$1\n") +
                "\n-----END CERTIFICATE-----\n"
    }

    private fun createSelfSignedCertificate(
        keyPair: KeyPair,
        name: String,
        email: String
    ): X509Certificate {
        val name = X500Name("CN=$email, O=$name")
        val subPubKeyInfo = SubjectPublicKeyInfo.getInstance(keyPair.public.encoded)
        val start = Date()
        val until = Date.from(LocalDate.of(2100, 12, 31).atStartOfDay().toInstant(ZoneOffset.UTC))
        /*val until = Date.from(
            LocalDate.now().plus(365, ChronoUnit.DAYS).atStartOfDay().toInstant(ZoneOffset.UTC)
        )*/

        val builder = X509v3CertificateBuilder(
            name,
            BigInteger(128, SecureRandom()),
            start,
            until,
            name,
            subPubKeyInfo
        )

        val usage =
            KeyUsage(KeyUsage.digitalSignature or KeyUsage.keyEncipherment or KeyUsage.dataEncipherment)
        builder.addExtension(Extension.keyUsage, false, usage)

        val signer: ContentSigner =
            JcaContentSignerBuilder("SHA256WithRSA").setProvider(BouncyCastleProvider())
                .build(keyPair.private)
        val holder = builder.build(signer)

        val converter = JcaX509CertificateConverter()
        converter.setProvider(BouncyCastleProvider())
        val cert = converter.getCertificate(holder)
        return cert
    }

}