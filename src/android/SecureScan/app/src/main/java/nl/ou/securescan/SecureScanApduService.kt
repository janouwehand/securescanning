package nl.ou.securescan

import android.nfc.cardemulation.HostApduService
import android.os.Bundle
import android.util.Log
import java.security.KeyPair
import java.security.KeyPairGenerator
import java.security.KeyStore
import java.security.SecureRandom
import java.util.*

class SecureScanApduService : HostApduService() {

    private var keypair: KeyPair?

    init {
        var generator = KeyPairGenerator.getInstance("RSA")
        generator.initialize(4096, SecureRandom())
        keypair = generator.genKeyPair()

        val ks: KeyStore = KeyStore.getInstance("PKCS12").apply {
            load(null)
        }
        val aliases: Enumeration<String> = ks.aliases()


        var rs=aliases.toList()

        Log.i("SecureScan", "Aanal: " + rs.size.toString())

        for (alias in aliases)
            Log.i("SecureScan", alias)
    }

    private fun isSelectApdu(apdu: ByteArray?): Boolean =
        apdu!!.size >= 2 && apdu!![0] === 0.toByte() && apdu!![1] === 0xa4.toByte()

    override fun processCommandApdu(apdu: ByteArray?, extra: Bundle?): ByteArray {

        var str = apdu!!.toHexString()
        Log.i("SecureScan", "Received APDU: $str")

        return if (isSelectApdu(apdu)) {
            var ret = "APPV://1.0.0"
            Log.i("SecureScan", ret)
            var bs = combineResult(ret.encodeToByteArray())
            bs
        } else {
            var (bs, sw1, sw2) = process(apdu)
            var result = bs.plus(sw1).plus(sw2)
            result
        }
    }

    data class ProcessResult(val Data: ByteArray, val sw1: Byte, val sw2: Byte)

    private fun process(apdu: ByteArray): ProcessResult {
        var instruction = apdu[1]
        var block = apdu[2]

        return when (instruction) {
            0x10.toByte() -> ProcessResult(processGetName(), instruction, 0x00)
            0x20.toByte() -> ProcessResult(processGetEmail(), instruction, 0x00)
            0x50.toByte() -> ProcessResult(processGetKey(block.toInt()), instruction, block)
            else -> ProcessResult(byteArrayOf(), 0x00, 0x00)
        }
    }

    private fun processGetName(): ByteArray = "J.L. Ouwehand".encodeToByteArray()

    private fun processGetEmail(): ByteArray = "jan.ouwehand@mvgs.nl".encodeToByteArray()

    private fun processGetKey(block: Int): ByteArray {
        var fromIndex = (block - 1) * 110
        var size = 110
        var bs = keypair!!.public.encoded

        var ret = bs.sliceArray(IntRange(fromIndex, fromIndex + size - 1))
        return ret
    }

    private fun combineResult(data: ByteArray, sw1: Byte = 0x00, sw2: Byte = 0x00): ByteArray =
        data.plus(sw1).plus(sw2)

    override fun onDeactivated(p0: Int) {
        Log.i("SecureScan", "deactivated!")
    }
}

@ExperimentalUnsignedTypes // just to make it clear that the experimental unsigned types are used
fun ByteArray.toHexString() = asUByteArray().joinToString("") { it.toString(16).padStart(2, '0') }
