package nl.ou.securescan.helpers

import android.app.Notification
import android.content.ContextWrapper
import android.app.NotificationManager
import android.app.NotificationChannel
import android.content.Context
import android.graphics.Color
import nl.ou.securescan.R

class NotificationUtils(base: Context?) : ContextWrapper(base) {
    private var mManager: NotificationManager? = null

    init {
        createNfcServiceChannel()
        createBleServiceChannel()
        createRequestAccessChannel()
    }

    private fun createBleServiceChannel() {
        val channel = NotificationChannel(
            CHANNEL_BLESERVICE,
            CHANNEL_BLESERVICE_NAME, NotificationManager.IMPORTANCE_DEFAULT
        )

        // Sets whether notifications posted to this channel should display notification lights
        channel.enableLights(true)

        // Sets whether notification posted to this channel should vibrate.
        channel.enableVibration(true)

        // Sets the notification light color for notifications posted to this channel
        channel.lightColor = Color.GREEN

        // Sets whether notifications posted to this channel appear on the lockscreen or not
        channel.lockscreenVisibility = Notification.VISIBILITY_PUBLIC
        manager.createNotificationChannel(channel)
    }

    private fun createNfcServiceChannel() {
        val channel = NotificationChannel(
            CHANNEL_NFC,
            CHANNEL_NFC_NAME, NotificationManager.IMPORTANCE_DEFAULT
        )

        // Sets whether notifications posted to this channel should display notification lights
        channel.enableLights(true)

        // Sets whether notification posted to this channel should vibrate.
        channel.enableVibration(true)

        // Sets the notification light color for notifications posted to this channel
        channel.lightColor = Color.GREEN

        // Sets whether notifications posted to this channel appear on the lockscreen or not
        channel.lockscreenVisibility = Notification.VISIBILITY_PUBLIC
        manager.createNotificationChannel(channel)
    }

    private fun createRequestAccessChannel() {
        val channel = NotificationChannel(
            CHANNEL_REQUESTACCESS,
            CHANNEL_REQUEST_ACCESS_NAME, NotificationManager.IMPORTANCE_HIGH
        )

        // Sets whether notifications posted to this channel should display notification lights
        channel.enableLights(true)

        // Sets whether notification posted to this channel should vibrate.
        channel.enableVibration(true)

        // Sets the notification light color for notifications posted to this channel
        channel.lightColor = Color.GREEN

        // Sets whether notifications posted to this channel appear on the lockscreen or not
        channel.lockscreenVisibility = Notification.VISIBILITY_PUBLIC
        manager.createNotificationChannel(channel)
    }

    val manager: NotificationManager
        get() {
            if (mManager == null) {
                mManager = getSystemService(NOTIFICATION_SERVICE) as NotificationManager
            }
            return mManager!!
        }

    fun getNotificationbuilder(title: String, body: String, channel: String): Notification.Builder {
        return Notification.Builder(applicationContext, channel)
            .setContentTitle(title)
            .setContentText(body)
            .setSmallIcon(R.drawable.ic_launcher_foreground)
    }

    companion object {
        const val CHANNEL_REQUESTACCESS = "nl.softable.channel.requestDocumentAccess"
        const val CHANNEL_NFC = "nl.softable.channel.nfcServiceActive"
        const val CHANNEL_BLESERVICE = "nl.softable.channel.bleServiceActive"
        const val CHANNEL_REQUEST_ACCESS_NAME = "Request Document Access"
        const val CHANNEL_NFC_NAME = "NFC APDU service"
        const val CHANNEL_BLESERVICE_NAME = "Bluetooth request access service"
    }
}