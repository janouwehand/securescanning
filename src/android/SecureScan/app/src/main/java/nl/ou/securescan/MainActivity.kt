package nl.ou.securescan

import android.app.AlertDialog
import android.content.Intent
import android.content.pm.PackageManager
import android.os.Build
import android.os.Bundle
import android.view.Menu
import android.view.MenuItem
import androidx.appcompat.app.AppCompatActivity
import androidx.recyclerview.widget.DividerItemDecoration
import androidx.recyclerview.widget.LinearLayoutManager
import kotlinx.coroutines.runBlocking
import nl.ou.securescan.crypto.CertificateManager
import nl.ou.securescan.crypto.extensions.getNameAndEmail
import nl.ou.securescan.data.DocumentDatabase
import nl.ou.securescan.databinding.ActivityMainBinding
import nl.ou.securescan.helpers.alert
import nl.ou.securescan.permissions.PermissionHandler


class MainActivity : AppCompatActivity() {

    private val certificateManager: CertificateManager = CertificateManager()
    private lateinit var binding: ActivityMainBinding
    private val permissionHandler = PermissionHandler(this)

    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)

        binding = ActivityMainBinding.inflate(layoutInflater)
        setContentView(binding.root)

        setSupportActionBar(binding.toolbar)
        
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

        if (permissionHandler.ensurePermissions(false)) {
            startBluetoothService()
        }
    }

    private fun startBluetoothService() {
        val intent = Intent(
            this, SecureScanBluetoothService::class.java
        )

        if (Build.VERSION.SDK_INT >= Build.VERSION_CODES.S) {
            startForegroundService(intent)
        } else {
            startService(intent)
        }
    }

    override fun onRequestPermissionsResult(
        requestCode: Int,
        permissions: Array<out String>,
        grantResults: IntArray
    ) {
        super.onRequestPermissionsResult(requestCode, permissions, grantResults)

        if (grantResults.isNotEmpty() && grantResults[0] == PackageManager.PERMISSION_DENIED
        ) {
            alert("No permission for ${permissions[0]}", "Permission") {
                finish()
            }
            return
        }

        /*when (requestCode) {
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
        }*/
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

    override fun onResume() {
        super.onResume()

        if (!permissionHandler.ensurePermissions()) {
            val builder = AlertDialog.Builder(this@MainActivity)
            builder.setMessage("No all permissions are currently enabled. Please restart the application to try again.")
                .setCancelable(false)
                .setTitle("Permissions")
                .setNeutralButton("OK") { dialog, _ ->
                    dialog.dismiss()
                    finish()
                }
            val alert = builder.create()
            alert.show()
            return
        }

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

