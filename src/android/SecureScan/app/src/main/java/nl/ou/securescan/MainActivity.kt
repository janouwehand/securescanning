package nl.ou.securescan

import android.Manifest
import android.app.AlertDialog
import android.content.Context
import android.content.Intent
import android.os.Build
import android.os.Bundle
import android.util.Log
import android.view.Menu
import android.view.MenuItem
import android.view.View
import androidx.annotation.RequiresApi
import androidx.appcompat.app.AppCompatActivity
import com.google.android.material.snackbar.Snackbar
import nl.ou.securescan.abilities.Permissions
import nl.ou.securescan.crypto.CertificateManager
import nl.ou.securescan.databinding.ActivityMainBinding
import java.security.cert.X509Certificate


class MainActivity : AppCompatActivity() {

    private val certman: CertificateManager = CertificateManager()
    private lateinit var binding: ActivityMainBinding

    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)

        binding = ActivityMainBinding.inflate(layoutInflater)
        setContentView(binding.root)

        setSupportActionBar(binding.toolbar)
    }

    override fun onResume() {
        super.onResume()

        if (certman.hasCertificate()) {
            var certInfo = certman.getCertificateInfo()!!
            binding.toolbar.subtitle = certInfo.email
        } else {
            handleNoCertificate()
        }
    }

    private fun handleNoCertificate() {
        val builder = AlertDialog.Builder(this@MainActivity)
        builder.setMessage("Secure Scan needs to create a personal certificate before you can use the application. Do you want to continue?")
            .setCancelable(false)
            .setTitle("Personal certificate")
            .setPositiveButton("Yes") { dialog, id ->
                var intent = Intent(baseContext, CreateCertificateActivity::class.java)
                startActivity(intent)
            }
            .setNegativeButton("No") { dialog, id ->
                dialog.dismiss()
                finish()
            }
        val alert = builder.create()
        alert.show()
    }

    @RequiresApi(Build.VERSION_CODES.P)
    private fun makeCert(context: Context) {

        val pm = context.packageManager

        for (f in pm.systemAvailableFeatures) {
            Log.i("SecureScan", "${f.name}: ${f.flags}")
        }

        return

        var cm = CertificateManager()
        if (!cm.hasCertificate()) {
            cm.createCertificate(context, "J.L.O. Ouwehand", "jan@softable.nl")
        }


    }

    private fun createCert(view: View) {
        Permissions().ensurePermission(this, Manifest.permission.CAMERA)
        { name, requested, granted ->
            Snackbar.make(view, "$name req: $requested gran:$granted", Snackbar.LENGTH_LONG)
                .setAction("Action", null).show()
        }
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
            else -> super.onOptionsItemSelected(item)
        }
    }

    override fun onSupportNavigateUp(): Boolean {
        /*val navController = findNavController(R.id.StartFragment)
        return navController.navigateUp(appBarConfiguration)
                || super.onSupportNavigateUp()*/
        return false
    }
}