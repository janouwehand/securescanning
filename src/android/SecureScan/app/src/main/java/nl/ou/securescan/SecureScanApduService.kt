package nl.ou.securescan

import android.nfc.cardemulation.HostApduService
import android.os.Bundle
import android.util.Log
import java.security.cert.X509Certificate
import java.util.*


class SecureScanApduService : HostApduService() {

    private val x509: X509Certificate

    init {

        x509 = CreateX509().execute()

        /*val ks: KeyStore = KeyStore.getInstance("PKCS12").apply {
            load(null)
        }
        val aliases: Enumeration<String> = ks.aliases()


        var rs=aliases.toList()

        Log.i("SecureScan", "Aanal: " + rs.size.toString())

        for (alias in aliases)
            Log.i("SecureScan", alias)*/
    }

//    private fun gzip(content: ByteArray): ByteArray {
//       /* val bos = ByteArrayOutputStream()
//        val os=GZIPOutputStream(bos) //.bufferedWriter(UTF_8).use { it.write(content) }
//        os.write(content)
//        os.flush()
//        return bos.toByteArray()*/
//
//        val os = ByteArrayOutputStream(content.size)
//        val gos = GZIPOutputStream(os)
//        gos.write(content)
//        gos.close()
//        val compressed = os.toByteArray()
//        os.close()
//        return compressed
//    }

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
        var blockInfo = apdu[3]
        var data = getDataFromAPDU(apdu)

        return when (instruction) {
            0x50.toByte() -> ProcessResult(processGetKey(block.toInt()), instruction, block)
            0x60.toByte() -> ProcessResult(
                processGetChallengeResult(data!!, blockInfo),
                instruction,
                0x00
            )
            else -> ProcessResult(byteArrayOf(), 0x00, 0x00)
        }
    }

    private fun getDataFromAPDU(apdu: ByteArray): ByteArray? {
        if (apdu.size <= 9) {
            return null
        }

        // 00 50 04 00 00 00 00
        val data = apdu.copyOfRange(7, apdu.size - 2)
        Log.i("SecureScan", "Received data: ${data.toHexString()} (size: ${data.size} byes)")
        return data
    }

    private fun processGetChallengeResult(data: ByteArray, blockInfo: Byte): ByteArray {
        Log.i("SecureScan", "Block info: $blockInfo")
        return arrayOf(0xF0.toByte(), 0xF0.toByte()).toByteArray()
    }

    private fun processGetKey(block: Int): ByteArray {
        val size = 250
        val fromIndex = (block - 1) * size
        val bs = x509.encoded

        var toIndex = fromIndex + size - 1

        if (toIndex > bs.size) {
            toIndex = bs.size - 1
        }

        if (toIndex <= fromIndex) {
            Log.i("SecureScan", "Geen bytes meer over")
            return arrayOf<Byte>().toByteArray()
        }
        Log.i("SecureScan", "Sending bytes $fromIndex to $toIndex (size: ${bs.size})")

        val ret = bs.sliceArray(fromIndex..toIndex)
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
