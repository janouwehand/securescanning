package nl.ou.securescan.crypto.extensions

import nl.ou.securescan.crypto.CertificateManager
import java.security.KeyStore
import java.security.MessageDigest
import java.security.PrivateKey
import java.security.cert.X509Certificate
import java.util.*
import java.util.regex.Matcher
import java.util.regex.Pattern
import javax.crypto.Cipher

data class NameAndEmail(val name: String, val email: String, val hostName: String)

fun X509Certificate.getNameAndEmail(): NameAndEmail {
    val subject = this.subjectDN.name

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

    fun getHostName(): String {
        val pattern = Pattern.compile("L=(.*?)(?:,|\$)")
        val matcher: Matcher = pattern.matcher(subject)
        return if (matcher.find()) {
            matcher.group(1)!!
        } else {
            ""
        }
    }

    val name = getName()
    val email = getEmail()
    val hostName = getHostName()

    return NameAndEmail(name, email, hostName)
}

@Suppress("unused")
fun X509Certificate.getPrivateKey(): PrivateKey? {
    val keyStore = KeyStore.getInstance(CertificateManager.ANDROIDKEYSTORE)
    keyStore.load(null)

    return if (keyStore.containsAlias(CertificateManager.ALIAS)) {
        val key = keyStore.getKey(CertificateManager.ALIAS, null)
        key as PrivateKey
    } else {
        null
    }
}

const val TRANSFORMATION = "RSA/ECB/PKCS1Padding"

fun X509Certificate.encryptData(plainText: ByteArray): ByteArray {
    val cipher = Cipher.getInstance(TRANSFORMATION)
    cipher.init(Cipher.ENCRYPT_MODE, this.publicKey)
    return cipher.doFinal(plainText)
}

fun X509Certificate.decryptData(cipherText: ByteArray): ByteArray {
    val cipher = Cipher.getInstance(TRANSFORMATION)
    cipher.init(Cipher.DECRYPT_MODE, this.getPrivateKey())
    return cipher.doFinal(cipherText)
}

fun X509Certificate.publicKeyToPem(): String {
    val base64PubKey = Base64.getEncoder().encodeToString(publicKey.encoded)
    return "-----BEGIN PUBLIC KEY-----\n" +
            base64PubKey.replace("(.{64})".toRegex(), "$1\n") +
            "\n-----END PUBLIC KEY-----\n"
}

fun X509Certificate.privateKeyToPem(): String {
    val pk = getPrivateKey()
    val base64PubKey = Base64.getEncoder().encodeToString(pk!!.encoded)
    return "-----BEGIN PRIVATE KEY-----\n" +
            base64PubKey.replace("(.{64})".toRegex(), "$1\n") +
            "\n-----END PRIVATE KEY-----\n"
}

fun X509Certificate.certificateToPem(): String {
    val base64PubKey = Base64.getEncoder().encodeToString(encoded)
    return "-----BEGIN CERTIFICATE-----\n" +
            base64PubKey.replace("(.{64})".toRegex(), "$1\n") +
            "\n-----END CERTIFICATE-----\n"
}

fun X509Certificate.getSHA1(): ByteArray {
    val messageDigest = MessageDigest.getInstance("SHA-1")
    messageDigest.update(encoded)
    return messageDigest.digest()
}