package nl.ou.securescan

import androidx.appcompat.app.AppCompatActivity
import android.os.Bundle
import nl.ou.securescan.databinding.ActivityCertificateInfoBinding
import nl.ou.securescan.databinding.ActivityCreateCertificateBinding

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
    }
}