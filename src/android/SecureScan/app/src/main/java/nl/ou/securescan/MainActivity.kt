package nl.ou.securescan

import android.app.AlertDialog
import android.content.Intent
import android.os.Bundle
import android.util.Log
import android.view.Menu
import android.view.MenuItem
import androidx.appcompat.app.AppCompatActivity
import nl.ou.securescan.crypto.CertificateManager
import nl.ou.securescan.crypto.extensions.decryptData
import nl.ou.securescan.crypto.extensions.encryptData
import nl.ou.securescan.crypto.extensions.getNameAndEmail
import nl.ou.securescan.crypto.extensions.toHexString
import nl.ou.securescan.databinding.ActivityMainBinding
import kotlin.random.Random


class MainActivity : AppCompatActivity() {

    private val certificateManager: CertificateManager = CertificateManager()
    private lateinit var binding: ActivityMainBinding

    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)

        binding = ActivityMainBinding.inflate(layoutInflater)
        setContentView(binding.root)

        setSupportActionBar(binding.toolbar)

        binding.buttonEncrypt.setOnClickListener {
            val ciphertext = tryEncrypt()
            val str = tryDecrypt(ciphertext)
            binding.buttonEncrypt.text = str
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
        startActivity(Intent(baseContext, CertificateInfoActivity::class.java))
        return true
    }

    private fun removeCertificate(): Boolean {
        val builder = AlertDialog.Builder(this@MainActivity)
        builder.setMessage("Are you sure that you want to delete the certificate?")
            .setCancelable(false)
            .setTitle("Delete certificate?")
            .setPositiveButton("Yes") { _, _ ->
                CertificateManager().removeCertificate()
                finish()
            }
            .setNegativeButton("No") { dialog, _ ->
                dialog.dismiss()
            }
        val alert = builder.create()
        alert.show()
        return true
    }

    override fun onSupportNavigateUp(): Boolean {
        /*val navController = findNavController(R.id.StartFragment)
        return navController.navigateUp(appBarConfiguration)
                || super.onSupportNavigateUp()*/
        return false
    }
}

