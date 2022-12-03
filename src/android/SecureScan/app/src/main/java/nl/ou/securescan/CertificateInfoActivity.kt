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
        binding.buttonDeleteCer.setOnClickListener { deleteCertificate() }
    }

    private fun deleteCertificate() {
        confirm("Are you sure that you want to delete your certificate and private key?") { ok ->
            if (ok){
                CertificateManager().removeCertificate()
                finish()
            }
        }
    }

    private fun storeCertificate() {
        val cert = CertificateManager().getCertificate()!!

        confirm("Note: this will only export the certificate with the public key. The private key cannot be exported due to the limitations of the Android Keystore. Continue?") { ok ->
            if (ok) {
                val appFileManager = AppFileManager(BuildConfig.APPLICATION_ID)

                val file = appFileManager.createFile(
                    context = this,
                    fileLocationCategory = FileLocationCategory.MEDIA_DIRECTORY,
                    fileName = "SecureScan-public-cert",
                    fileExtension = "txt"
                )

                val pem = cert.certificateToPem()
                file.writeBytes(pem.toByteArray())

                alert("Success: your x.509 public key certificate has been successfully stored as ${file.name} in android/media folder.")
            }
        }
    }

}