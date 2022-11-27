package nl.ou.securescan

import android.os.Bundle
import androidx.fragment.app.Fragment
import android.view.LayoutInflater
import android.view.View
import android.view.ViewGroup
import androidx.navigation.fragment.findNavController
import nl.ou.securescan.crypto.CertificateManager
import nl.ou.securescan.databinding.FirstUsageCreateCertificateBinding
import java.util.regex.Pattern

/**
 * A simple [Fragment] subclass as the default destination in the navigation.
 */
class FirstUsageCreateCertificate : Fragment() {

    private var _binding: FirstUsageCreateCertificateBinding? = null

    // This property is only valid between onCreateView and
    // onDestroyView.
    private val binding get() = _binding!!

    override fun onCreateView(
        inflater: LayoutInflater, container: ViewGroup?,
        savedInstanceState: Bundle?
    ): View? {

        _binding = FirstUsageCreateCertificateBinding.inflate(inflater, container, false)
        return binding.root
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

    override fun onViewCreated(view: View, savedInstanceState: Bundle?) {
        super.onViewCreated(view, savedInstanceState)

        binding.buttonCreateCertificate.setOnClickListener {
            var name = binding.edtYourName.text
            var email = binding.edtEmail.text

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

        /*binding.buttonFirst.setOnClickListener {
            findNavController().navigate(R.id.action_FirstFragment_to_SecondFragment)
        }*/
    }

    private fun createCertificate(name: String, email: String) {
        var cm=CertificateManager()
        cm.CreateCertificate(name, email)
        findNavController().navigate(R.id.action_FirstFragment_to_SecondFragment)
    }

    override fun onDestroyView() {
        super.onDestroyView()
        _binding = null
    }
}