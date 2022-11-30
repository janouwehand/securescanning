package nl.ou.securescan

import android.app.AlertDialog
import android.os.Bundle
import androidx.appcompat.app.AppCompatActivity
import nl.ou.securescan.crypto.CertificateManager
import nl.ou.securescan.databinding.ActivityCreateCertificateBinding
import java.util.regex.Pattern


class CreateCertificateActivity : AppCompatActivity() {

    private lateinit var binding: ActivityCreateCertificateBinding

    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)

        binding = ActivityCreateCertificateBinding.inflate(layoutInflater)
        setContentView(binding.root)

        setSupportActionBar(binding.toolbar)

        binding.toolbar.title = "Create certificate"
        binding.toolbar.subtitle = "Create your personal certificate"

        binding.buttonCreateCertificate.setOnClickListener {
            val name = binding.edtYourName.text
            val email = binding.edtEmail.text

            var errors = false

            if (name.length < 5) {
                binding.edtYourName.error = "Your name is too short."
                errors = true
            } else {
                binding.edtYourName.error = null
            }

            if (email.length < 5) {
                binding.edtEmail.error = "Your email address is too short."
                errors = true
            } else if (!isValidEmail(email)) {
                binding.edtEmail.error = "Please provide a valid email address."
                errors = true
            } else {
                binding.edtEmail.error = null
            }

            if (!errors) {
                createCertificate(name.toString(), email.toString())
            }
        }
    }

    private fun isValidEmail(email: CharSequence): Boolean {
        var isValid = true
        val expression = "^[\\w.-]+@([\\w\\-]+\\.)+[A-Z]{2,4}$"
        val pattern = Pattern.compile(expression, Pattern.CASE_INSENSITIVE)
        val matcher = pattern.matcher(email)
        if (!matcher.matches()) {
            isValid = false
        }
        return isValid
    }

    private fun createCertificate(name: String, email: String) {
        val cm = CertificateManager()
        cm.createCertificate(name, email)

        val builder = AlertDialog.Builder(this)
        builder.setMessage("Your personal certificate has successfully been created.")
            .setCancelable(false)
            .setTitle("Success!")
            .setNeutralButton("Continue") { _, _ ->
                finish()
            }
        val alert = builder.create()
        alert.show()
    }

}