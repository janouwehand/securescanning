package nl.ou.securescan

import android.content.ComponentName
import android.content.Context
import android.content.Intent
import android.content.ServiceConnection
import android.os.Bundle
import android.os.IBinder
import androidx.appcompat.app.AppCompatActivity
import androidx.core.app.NotificationManagerCompat
import nl.ou.securescan.databinding.ActivityCreateCertificateBinding
import nl.ou.securescan.databinding.ActivityDocumentAccessRequestBinding
import nl.ou.securescan.databinding.ActivityDocumentInfoBinding


class DocumentAccessRequest : AppCompatActivity() {

    private lateinit var binding: ActivityDocumentAccessRequestBinding

    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)

        binding = ActivityDocumentAccessRequestBinding.inflate(layoutInflater)
        setContentView(binding.root)

        val email = intent.getStringExtra("email")!!
        val name = intent.getStringExtra("name")!!
        val hostName = intent.getStringExtra("hostName")!!

        val documentName = intent.getStringExtra("documentName")!!
        val documentId = intent.getIntExtra("documentId", 0)

        binding.textViewRequest.text =
            "User $name of mailbox '$email' from host $hostName is requesting access to document $documentId ($documentName)"

        // Hide notification
        val manager = NotificationManagerCompat.from(this)
        manager.cancel(1001)

        setSupportActionBar(binding.toolbar)

        binding.toolbar.title = "Secure Scan"
        binding.toolbar.subtitle = "Aprove document access request"

        binding.buttonApprove.setOnClickListener { setApproval(true) }
        binding.buttonDeny.setOnClickListener { setApproval(false) }
        
        SecureScanBluetoothService.documentAccessRequest = this
    }

    private fun setApproval(approved: Boolean) {
        val serviceIntent = Intent(this, SecureScanBluetoothService::class.java)
        serviceIntent.putExtra("LeDot", ".")

        val serviceConnection =
            object : ServiceConnection {
                override fun onServiceConnected(name: ComponentName?, service: IBinder?) {

                    // Cast the IBinder and get MyService instance
                    val binder = service as SecureScanBluetoothService.MyBinder
                    val myService = binder.getService()

                    // Call a method from the MyService class
                    myService.setApproval(approved)

                    unbindService(this)
                    finishAndRemoveTask()
                }

                override fun onServiceDisconnected(name: ComponentName?) {
                }
            }

        bindService(serviceIntent, serviceConnection, Context.BIND_AUTO_CREATE)
    }
}