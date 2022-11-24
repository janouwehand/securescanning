package nl.ou.securescan

import android.content.Context
import android.os.Environment
import org.bouncycastle.asn1.x500.X500Name
import org.bouncycastle.asn1.x509.Extension
import org.bouncycastle.asn1.x509.KeyUsage
import org.bouncycastle.asn1.x509.SubjectPublicKeyInfo
import org.bouncycastle.cert.X509v3CertificateBuilder
import org.bouncycastle.cert.jcajce.JcaX509CertificateConverter
import org.bouncycastle.jce.provider.BouncyCastleProvider
import org.bouncycastle.operator.ContentSigner
import org.bouncycastle.operator.jcajce.JcaContentSignerBuilder
import java.io.ByteArrayOutputStream
import java.io.FileInputStream
import java.math.BigInteger
import java.security.*
import java.security.cert.X509Certificate
import java.time.LocalDate
import java.time.ZoneOffset
import java.util.*
import java.util.zip.GZIPInputStream
import java.util.zip.GZIPOutputStream


class CreateX509 {

    fun execute(): X509Certificate {
        var sort = "tls"
        //val keyFile = File("key-$sort.pem")
        //val certFile = File("cert-$sort.pem")
        val keyPair = generateKeyPair()
        val certificate = createSelfSignedCertificate(keyPair)
        //keyFile.writeText(privateKeyToPem(keyPair.private))
        //println("Created ${keyFile.absoluteFile}")
        //certFile.writeText(certificateToPem(certificate))
        //println("Created ${certFile.absoluteFile}")

        //var map = Environment.getExternalStorageDirectory()
        //var bestand = File(map, "SecureScan.pfx")
        //var bs = certificate.encoded
        //bestand.writeBytes(bs)

        return certificate
    }

    fun generateKeyPair(): KeyPair {
        val kpg = KeyPairGenerator.getInstance("RSA")
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

    /*fun ungzip(content: ByteArray): String =
        GZIPInputStream(content.inputStream()).bufferedReader(UTF_8).use { it.readText() }*/

    private fun createSelfSignedCertificate(keyPair: KeyPair): X509Certificate {
        val name = X500Name("E=jan@softable.nl, CN=J.L. Ouwehand")
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

        val usage = KeyUsage(KeyUsage.digitalSignature or KeyUsage.keyEncipherment)
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

    /*fun createSelfSignedCertificate(keyPair: KeyPair): X509Certificate {
        // inspired by https://hecpv.wordpress.com/2017/03/18/how-to-generate-x-509-certificate-in-java-1-8/

        val commonName = "INGTestAPI"
        val organizationalUnit = "Test"
        val organization = "Test"
        val country = "NL"

        val validDays = 365

        var er=X500Principal("")

        val distinguishedName = X500Name(commonName, organizationalUnit, organization, country)

        var f=CertificateBuilder()

        val info = X509CertInfo()

        val since = Date() // Since Now
        val until = Date(since.time + validDays * 86400000L) // Until x days (86400000 milliseconds in one day)

        val sn = BigInteger(64, SecureRandom())

        info.set(X509CertInfo.VALIDITY, CertificateValidity(since, until))
        info.set(X509CertInfo.SERIAL_NUMBER, CertificateSerialNumber(sn))
        info.set(X509CertInfo.SUBJECT, distinguishedName)
        info.set(X509CertInfo.ISSUER, distinguishedName)
        info.set(X509CertInfo.KEY, CertificateX509Key(keyPair.public))
        info.set(X509CertInfo.VERSION, CertificateVersion(CertificateVersion.V3))

        var algo = AlgorithmId(AlgorithmId.md5WithRSAEncryption_oid)
        info.set(X509CertInfo.ALGORITHM_ID, CertificateAlgorithmId(algo))

        // Sign the cert to identify the algorithm that is used.
        var cert = X509CertImpl(info)
        cert.sign(keyPair.private, "SHA256withRSA")

        // Update the algorithm and sign again
        algo = cert.get(X509CertImpl.SIG_ALG) as AlgorithmId
        info.set(CertificateAlgorithmId.NAME + "." + CertificateAlgorithmId.ALGORITHM, algo)

        cert = X509CertImpl(info)
        cert.sign(keyPair.private, "SHA256withRSA")

        return cert
    }*/

    private val CUSTOMER_CERTIFICATE_STORE = "CustomerKeyStore.keystore"
    private val CUSTOMER_CERTIFICATE_ALIAS = "CZ1212121218"
    private val CUSTOMER_KS_PASSWORD = "eet"

    private val SERVER_CERTIFICATE_STORE = "ServerKeyStore.keystore"
    private val SERVER_CERTIFICATE_ALIAS = "ca"
    private val SERVER_KS_PASSWORD = ""

    private fun getCustomerKeystore(context: Context): KeyStore? {
        return try {
            val keyStore = KeyStore.getInstance("PKCS12")
            // Load Keystore form internal storage
            val fis: FileInputStream = context.openFileInput(CUSTOMER_CERTIFICATE_STORE)
            keyStore.load(fis, CUSTOMER_KS_PASSWORD.toCharArray())
            keyStore
        } catch (e: Exception) {
            e.printStackTrace()
            throw RuntimeException("Keystore not found.")
        }
    }

    fun SaveX509InStore(context: Context, cert: X509Certificate) {

    }
}