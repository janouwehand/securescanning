package nl.ou.securescan

import android.Manifest
import android.app.AlertDialog
import android.app.PendingIntent
import android.app.TaskStackBuilder
import android.bluetooth.BluetoothAdapter
import android.bluetooth.BluetoothDevice
import android.bluetooth.le.AdvertiseData
import android.bluetooth.le.AdvertisingSet
import android.bluetooth.le.AdvertisingSetCallback
import android.bluetooth.le.AdvertisingSetParameters
import android.content.Intent
import android.content.pm.PackageManager
import android.os.Bundle
import android.os.ParcelUuid
import android.util.Log
import android.view.Menu
import android.view.MenuItem
import android.widget.Toast
import androidx.appcompat.app.AppCompatActivity
import androidx.core.app.ActivityCompat
import androidx.core.content.ContextCompat
import androidx.recyclerview.widget.DividerItemDecoration
import androidx.recyclerview.widget.LinearLayoutManager
import kotlinx.coroutines.runBlocking
import nl.ou.securescan.crypto.CertificateManager
import nl.ou.securescan.crypto.extensions.decryptData
import nl.ou.securescan.crypto.extensions.encryptData
import nl.ou.securescan.crypto.extensions.getNameAndEmail
import nl.ou.securescan.crypto.extensions.toHexString
import nl.ou.securescan.data.DocumentDatabase
import nl.ou.securescan.databinding.ActivityMainBinding
import nl.ou.securescan.helpers.NotificationUtils
import java.util.*
import kotlin.random.Random


class MainActivity : AppCompatActivity() {

    private val certificateManager: CertificateManager = CertificateManager()
    private lateinit var binding: ActivityMainBinding

    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)

        binding = ActivityMainBinding.inflate(layoutInflater)
        setContentView(binding.root)

        setSupportActionBar(binding.toolbar)

        checkPermissions()

        binding.buttonEncrypt.setOnClickListener {
            /*val ciphertext = tryEncrypt()
            val str = tryDecrypt(ciphertext)
            binding.buttonEncrypt.text = str*/
            //StartBLE()


            // Create an Intent for the activity you want to start
            val resultIntent = Intent(this, DocumentAccessRequest::class.java)

            resultIntent.putExtra("DocumentId", 3445)

            // Create the TaskStackBuilder
            val resultPendingIntent: PendingIntent? = TaskStackBuilder.create(this).run {
                // Add the intent, which inflates the back stack
                addNextIntentWithParentStack(resultIntent)

                // Get the PendingIntent containing the entire back stack
                getPendingIntent(
                    0,
                    PendingIntent.FLAG_UPDATE_CURRENT or PendingIntent.FLAG_IMMUTABLE
                )
            }

            var utils = NotificationUtils(this)
            val not = utils.getNotificationbuilder(
                "Test eerste",
                "Bla die bla",
                NotificationUtils.CHANNEL_REQUESTACCESS
            )
                .setTicker("sdfsdgsgsdgsdg")
                .setContentIntent(resultPendingIntent)
                .build()
            utils.manager.notify(1001, not)
        }

        //DocumentDatabase.deleteDatabaseFile(baseContext)

        binding.swipe.setOnRefreshListener {
            refreshItems()
            binding.swipe.isRefreshing = false
        }

        binding.itemsList.addItemDecoration(
            DividerItemDecoration(
                this,
                LinearLayoutManager.VERTICAL
            )
        )

        refreshItems()

        val ext = BluetoothAdapter.getDefaultAdapter().isLeExtendedAdvertisingSupported
        binding.textView10.text = "isLeExtendedAdvertisingSupported: $ext"

        val intent = Intent(
            this, SecureScanBluetoothService::class.java
        )

        startForegroundService(intent)
    }

    var currentAdvertisingSet: AdvertisingSet? = null

    private fun checkPermissions() {
        if (ActivityCompat.checkSelfPermission(
                this,
                Manifest.permission.BLUETOOTH_ADVERTISE
            ) != PackageManager.PERMISSION_GRANTED
        ) {
            ActivityCompat.requestPermissions(
                this,
                arrayOf(Manifest.permission.BLUETOOTH_ADVERTISE),
                1
            )
            // here to request the missing permissions, and then overriding
            //   public void onRequestPermissionsResult(int requestCode, String[] permissions,
            //                                          int[] grantResults)
            // to handle the case where the user grants the permission. See the documentation
            // for ActivityCompat#requestPermissions for more details.
            return
        }

        if (ActivityCompat.checkSelfPermission(
                this,
                Manifest.permission.BLUETOOTH_CONNECT
            ) != PackageManager.PERMISSION_GRANTED
        ) {
            ActivityCompat.requestPermissions(
                this,
                arrayOf(Manifest.permission.BLUETOOTH_CONNECT),
                1
            )
            // here to request the missing permissions, and then overriding
            //   public void onRequestPermissionsResult(int requestCode, String[] permissions,
            //                                          int[] grantResults)
            // to handle the case where the user grants the permission. See the documentation
            // for ActivityCompat#requestPermissions for more details.
            return
        }

        if (ActivityCompat.checkSelfPermission(
                this,
                Manifest.permission.BLUETOOTH_ADMIN
            ) != PackageManager.PERMISSION_GRANTED
        ) {
            ActivityCompat.requestPermissions(
                this,
                arrayOf(Manifest.permission.BLUETOOTH_ADMIN),
                1
            )
            // here to request the missing permissions, and then overriding
            //   public void onRequestPermissionsResult(int requestCode, String[] permissions,
            //                                          int[] grantResults)
            // to handle the case where the user grants the permission. See the documentation
            // for ActivityCompat#requestPermissions for more details.
            return
        }
    }

    override fun onRequestPermissionsResult(
        requestCode: Int,
        permissions: Array<out String>,
        grantResults: IntArray
    ) {
        super.onRequestPermissionsResult(requestCode, permissions, grantResults)
        when (requestCode) {
            1 -> {
                if (grantResults.isNotEmpty() && grantResults[0] ==
                    PackageManager.PERMISSION_GRANTED
                ) {
                    if ((ContextCompat.checkSelfPermission(
                            this@MainActivity,
                            Manifest.permission.ACCESS_FINE_LOCATION
                        ) ==
                                PackageManager.PERMISSION_GRANTED)
                    ) {
                        Toast.makeText(this, "Permission Granted", Toast.LENGTH_SHORT).show()
                    }
                } else {
                    Toast.makeText(this, "Permission Denied", Toast.LENGTH_SHORT).show()
                }
                return
            }
        }
    }

    private fun refreshItems() {
        val db = DocumentDatabase.getDatabase(baseContext)
        val dao = db.documentDao()

        runBlocking {
            val all = dao.getAll()
            val documentsAdapter = DocumentItemsAdapter(all.toTypedArray())

            binding.itemsList.apply {
                // vertical layout
                layoutManager = LinearLayoutManager(applicationContext)

                // set adapter
                adapter = documentsAdapter
            }

        }
    }

    private val rand = Random(5000)

    private fun tryEncrypt(): ByteArray {
        val cert = certificateManager.getCertificate()!!

        val plaintext = "${rand.nextDouble()}".toByteArray()

        Log.i("SecureScan", "PlainText  : ${plaintext.toHexString()}")

        return cert.encryptData(plaintext)
    }

    private fun tryDecrypt(ciphertext: ByteArray): String {
        val cert = certificateManager.getCertificate()!!
        val plaintext = cert.decryptData(ciphertext)
        Log.i("SecureScan", "PlainText 2: ${plaintext.toHexString()}")
        return String(plaintext)
    }

    override fun onResume() {
        super.onResume()

        if (certificateManager.hasCertificate()) {
            val cert = certificateManager.getCertificate()!!
            val certInfo = cert.getNameAndEmail()
            binding.toolbar.subtitle = certInfo.email

            refreshItems()

        } else {
            handleNoCertificate()
        }
    }

    private fun handleNoCertificate() {
        val builder = AlertDialog.Builder(this@MainActivity)
        builder.setMessage("Secure Scan needs to create a personal certificate before you can use the application. Do you want to continue?")
            .setCancelable(false)
            .setTitle("Personal certificate")
            .setPositiveButton("Yes") { _, _ ->
                val intent = Intent(baseContext, CreateCertificateActivity::class.java)
                startActivity(intent)
            }
            .setNegativeButton("No") { dialog, _ ->
                dialog.dismiss()
                finish()
            }
        val alert = builder.create()
        alert.show()
    }

    override fun onCreateOptionsMenu(menu: Menu): Boolean {
        // Inflate the menu; this adds items to the action bar if it is present.
        menuInflater.inflate(R.menu.menu_main, menu)
        return true
    }

    override fun onOptionsItemSelected(item: MenuItem): Boolean {
        // Handle action bar item clicks here. The action bar will
        // automatically handle clicks on the Home/Up button, so long
        // as you specify a parent activity in AndroidManifest.xml.
        return when (item.itemId) {
            R.id.action_settings -> true
            R.id.action_certificateInfo -> showCertificateInfo()
            else -> super.onOptionsItemSelected(item)
        }
    }

    private fun showCertificateInfo(): Boolean {
        if (CertificateManager().hasCertificate()) {
            startActivity(Intent(baseContext, CertificateInfoActivity::class.java))
        }
        return true
    }

    override fun onSupportNavigateUp(): Boolean {
        /*val navController = findNavController(R.id.StartFragment)
        return navController.navigateUp(appBarConfiguration)
                || super.onSupportNavigateUp()*/
        return false
    }
}

