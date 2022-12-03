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
        val kpg = KeyPairGenerator.getInstance(KeyProperties.KEY_ALGORITHM_RSA, "AndroidKeyStore")
        kpg.initialize(2048)
        return kpg.genKeyPair()
    }

    private fun createSelfSignedCertificate(
        keyPair: KeyPair,
        name: String,
        email: String
    ): X509Certificate {
        val x500name = X500Name("CN=$email, O=$name")
        val subPubKeyInfo = SubjectPublicKeyInfo.getInstance(keyPair.public.encoded)
        val start = Date()
        val until = Date.from(LocalDate.of(2100, 12, 31).atStartOfDay().toInstant(ZoneOffset.UTC))

        val builder = X509v3CertificateBuilder(
            x500name,
            BigInteger(128, SecureRandom()),
            start,
            until,
            x500name,
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
        return converter.getCertificate(holder)
    }

}