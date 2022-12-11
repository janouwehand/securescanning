package nl.ou.securescan

import android.app.Service
import android.content.Intent
import android.os.Binder
import android.os.IBinder
import android.os.Parcel
import android.util.Log
import nl.ou.securescan.helpers.NotificationUtils

class SecureScanBluetoothService : Service() {

    override fun onBind(intent: Intent): IBinder? {
        return null
    }

    override fun onStartCommand(intent: Intent?, flags: Int, startId: Int): Int {
        Log.i("SecureScan", "SERVICE STARTED ********************************")

        return super.onStartCommand(intent, flags, startId)
    }

    private fun notifyMe() {

        val utils = NotificationUtils(this)

        val not = utils.getNotificationbuilder(
            "Secure Scan",
            "Request for access service over bluetooth started",
            NotificationUtils.CHANNEL_BLESERVICE
        )
            .build()

        utils.manager.notify(1001, not)
    }

    override fun onDestroy() {
        super.onDestroy()
        Log.i("SecureScan", "SERVICE DESTROYED ********************************")
    }
}