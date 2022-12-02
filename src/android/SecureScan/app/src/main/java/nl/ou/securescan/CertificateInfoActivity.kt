package nl.ou.securescan

import androidx.appcompat.app.AppCompatActivity
import android.os.Bundle
import com.arnab.storage.AppFileManager
import com.arnab.storage.FileLocationCategory
import nl.ou.securescan.crypto.CertificateManager
import nl.ou.securescan.crypto.extensions.*
import nl.ou.securescan.databinding.ActivityCertificateInfoBinding
import nl.ou.securescan.helpers.alert
import nl.ou.securescan.helpers.confirm

class CertificateInfoActivity : AppCompatActivity() {

    private lateinit var binding: ActivityCertificateInfoBinding

    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)
        setContentView(R.layout.activity_certificate_info)

        binding = ActivityCertificateInfoBinding.inflate(layoutInflater)
        setContentView(binding.root)

        //setSupportActionBar(binding.toolbar)

        binding.toolbar.title = "Your certificate"
        binding.toolbar.subtitle = "Certificate info"

        val cert = CertificateManager().getCertificate()!!
        val info = cert.getNameAndEmail()

        binding.textViewName.text = info.name
        binding.textViewEmail.text = info.email
        binding.textViewSerial.text = cert.serialNumber.toString()
        binding.textViewSHA1.text = cert.getSHA1().toHexString()

        binding.buttonCer.setOnClickListener { storeCertificate() }
    }

    private fun storeCertificate() {
        val cert = CertificateManager().getCertificate()!!

        val appFileManager = AppFileManager(BuildConfig.APPLICATION_ID)

        val file = appFileManager.createFile(
            context = this,
            fileLocationCategory = FileLocationCategory.MEDIA_DIRECTORY,
            fileName = "SecureScan-public-cert",
            fileExtension = "txt"
        )

        val pem = cert.certificateToPem()
        file.writeBytes(pem.toByteArray())

        alert("Success: your x.509 public key certificate has been stored as ${file.name} in android/media folder. Note that Android does not allow to export private keys. Hence, the private key was not included in this export.")
    }

}