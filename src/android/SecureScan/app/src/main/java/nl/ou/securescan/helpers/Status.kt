package nl.ou.securescan.helpers

import android.util.Log
import nl.ou.securescan.SecureScanGattProfile

class Status {

    private var statusValue: Byte = SecureScanGattProfile.STATUS_IDLE

    fun setStatus(value: Byte) {
        Log.w(
            "SecureScanBT",
            "Status change: from ${SecureScanGattProfile.getStatusDescription(statusValue)} to ${
                SecureScanGattProfile.getStatusDescription(value)
            }"
        )
        statusValue = value
    }

    fun getStatus(): Byte = statusValue


}