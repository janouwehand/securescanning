package nl.ou.securescan.crypto.extensions

import java.security.SecureRandom
import javax.crypto.Cipher
import javax.crypto.spec.GCMParameterSpec
import javax.crypto.spec.SecretKeySpec

@OptIn(ExperimentalUnsignedTypes::class)
fun ByteArray.toHexString() =
    asUByteArray().joinToString("") { it.toString(16).padStart(2, '0') }

private fun generateRandomIV(): ByteArray {
    val iv = ByteArray(12) // IV is always 12 bytes for GCM mode
    val random = SecureRandom()
    random.nextBytes(iv)
    return iv
}

fun ByteArray.encryptAES256GCM(encryptionKey: ByteArray): ByteArray {
    val cipher = Cipher.getInstance("AES/GCM/NoPadding")
    val keySpec = SecretKeySpec(encryptionKey, "AES")
    val iv = generateRandomIV()

    val gcmParameterSpec = GCMParameterSpec(128 /* note: this is authorization size, not keysize! */, iv)
    cipher.init(Cipher.ENCRYPT_MODE, keySpec, gcmParameterSpec)
    var encryptedData = cipher.doFinal(this)

    val ivAndEncryptedData = ByteArray(iv.size + encryptedData.size)
    System.arraycopy(iv, 0, ivAndEncryptedData, 0, iv.size)
    System.arraycopy(encryptedData, 0, ivAndEncryptedData, iv.size, encryptedData.size)

    return ivAndEncryptedData
}

fun ByteArray.decryptAES256GCM(encryptionKey: ByteArray): ByteArray {
    val cipher = Cipher.getInstance("AES/GCM/NoPadding")
    val keySpec = SecretKeySpec(encryptionKey, "AES")

    val ivSize = 12
    val iv = ByteArray(ivSize)
    val encryptedBytes = ByteArray(this.size - ivSize)

    // Extract IV and encrypted data from the combined input
    System.arraycopy(this, 0, iv, 0, ivSize)
    System.arraycopy(this, ivSize, encryptedBytes, 0, this.size - ivSize)

    val gcmParameterSpec = GCMParameterSpec(128, iv)
    cipher.init(Cipher.DECRYPT_MODE, keySpec, gcmParameterSpec)
    return cipher.doFinal(encryptedBytes)
}