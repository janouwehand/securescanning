package nl.ou.securescan

import android.os.Bundle
import androidx.appcompat.app.AppCompatActivity
import androidx.core.app.NotificationManagerCompat


class DocumentAccessRequest : AppCompatActivity() {
    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)
        setContentView(R.layout.activity_document_access_request)

        val manager = NotificationManagerCompat.from(this)
        //manager.cancel(intent.getIntExtra("Notification_ID", -1))
        manager.cancel(1001)
    }
}