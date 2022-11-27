package nl.ou.securescan

import android.Manifest
import android.content.Context
import android.os.Bundle
import android.util.Log
import android.view.Menu
import android.view.MenuItem
import android.view.View
import android.widget.Button
import androidx.appcompat.app.AppCompatActivity
import androidx.navigation.findNavController
import androidx.navigation.ui.AppBarConfiguration
import androidx.navigation.ui.navigateUp
import androidx.navigation.ui.setupActionBarWithNavController
import com.google.android.material.snackbar.Snackbar
import com.karumi.dexter.Dexter
import com.karumi.dexter.PermissionToken
import com.karumi.dexter.listener.PermissionDeniedResponse
import com.karumi.dexter.listener.PermissionGrantedResponse
import com.karumi.dexter.listener.PermissionRequest
import com.karumi.dexter.listener.single.PermissionListener
import nl.ou.securescan.abilities.Permissions
import nl.ou.securescan.crypto.CertificateManager
import nl.ou.securescan.databinding.ActivityMainBinding


class MainActivity : AppCompatActivity() {

    private lateinit var appBarConfiguration: AppBarConfiguration
    private lateinit var binding: ActivityMainBinding

    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)

        binding = ActivityMainBinding.inflate(layoutInflater)
        setContentView(binding.root)

        setSupportActionBar(binding.toolbar)

        var button = findViewById<Button>(R.id.buttonMakeCert)
        button.text = "sdsdgsdg"
        button.setOnClickListener { makeCert(this.baseContext) }

        val navController = findNavController(R.id.nav_host_fragment_content_main)
        appBarConfiguration = AppBarConfiguration(navController.graph)
        setupActionBarWithNavController(navController, appBarConfiguration)

        binding.fab.setOnClickListener { view ->
            createCert(view)
            /*Snackbar.make(view, "create cert", Snackbar.LENGTH_LONG)
                .setAction("Action", null).show()*/
        }

        //binding.
        //R.id.buttonMakeCert
        //buttonMakeCert
    }

    private fun makeCert(context: Context) {

        val pm = context.packageManager

        for (f in pm.systemAvailableFeatures) {
            Log.i("SecureScan", "${f.name}: ${f.flags}")
        }

        return

        var cm = CertificateManager()
        if (!cm.HasCertificate()) {
            cm.CreateCertificate("J.L.O. Ouwehand", "jan@softable.nl")
        }

        var cert = cm.GetCertificate()
        var enc = cert?.encoded

        Log.i("SecureScan", "Sdaizeip: ${enc?.size}")
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
        val navController = findNavController(R.id.nav_host_fragment_content_main)
        return navController.navigateUp(appBarConfiguration)
                || super.onSupportNavigateUp()
    }
}