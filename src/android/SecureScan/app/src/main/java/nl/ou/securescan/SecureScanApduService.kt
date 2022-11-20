package nl.ou.securescan

import android.nfc.cardemulation.HostApduService
import android.os.Bundle
import android.util.Log

class SecureScanApduService : HostApduService() {

    private fun isSelectApdu(apdu: ByteArray?): Boolean {
        return apdu!!.size >= 2 && apdu!![0] === 0.toByte() && apdu!![1] === 0xa4.toByte()
    }

    override fun processCommandApdu(apdu: ByteArray?, extra: Bundle?): ByteArray {

        var str = apdu!!.toHexString()
        Log.i("SecureScan", "Received APDU: $str")

        if (isSelectApdu(apdu)) {
            var ret = "APPV://1.0.0"
            Log.i("SecureScan", ret)
            var bs = combineResult(ret.encodeToByteArray())
            return bs
        } else {
            var (bs, sw1, sw2) = process(apdu)
            var result = bs.plus(sw1).plus(sw2)
            return result
        }
    }

    data class ProcessResult(val Data: ByteArray, val sw1: Byte, val sw2: Byte)

    private fun process(apdu: ByteArray): ProcessResult {
        var instruction = apdu[1]
        return when (instruction) {
            0x10.toByte() -> {
                ProcessResult(processGetName(), instruction, 0x00)
            }
            else -> {
                ProcessResult(byteArrayOf(), 0x00, 0x00)
            }
        }
    }

    private fun processGetName(): ByteArray {
        return "J.L. Ouwehand".encodeToByteArray()
    }

    private fun combineResult(data: ByteArray, sw1: Byte = 0x00, sw2: Byte = 0x00): ByteArray {
        return data.plus(sw1).plus(sw2)
    }

    override fun onDeactivated(p0: Int) {
        Log.i("SecureScan", "deactivated!")
    }
}

@ExperimentalUnsignedTypes // just to make it clear that the experimental unsigned types are used
fun ByteArray.toHexString() = asUByteArray().joinToString("") { it.toString(16).padStart(2, '0') }
