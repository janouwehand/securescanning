package nl.ou.securescan

import android.os.Bundle
import androidx.appcompat.app.AppCompatActivity
import androidx.core.text.set
import kotlinx.coroutines.runBlocking
import nl.ou.securescan.data.Document
import nl.ou.securescan.data.DocumentDatabase
import nl.ou.securescan.databinding.ActivityCertificateInfoBinding
import nl.ou.securescan.databinding.ActivityDocumentInfoBinding
import nl.ou.securescan.helpers.alert
import nl.ou.securescan.helpers.showKeyboard
import nl.ou.securescan.helpers.toNeatDateString
import nl.ou.securescan.helpers.toZonedDateTime
import kotlin.properties.Delegates

class DocumentInfoActivity : AppCompatActivity() {

    private lateinit var binding: ActivityDocumentInfoBinding
    private lateinit var db: DocumentDatabase
    private var documentId by Delegates.notNull<Int>()

    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)

        binding = ActivityDocumentInfoBinding.inflate(layoutInflater)
        setContentView(binding.root)

        val extras = intent.extras
        documentId = extras!!.getInt("DocumentId")

        binding.toolbar.title = "Document $documentId"
        binding.toolbar.setNavigationOnClickListener {
            finish()
        }

        db = DocumentDatabase.getDatabase(baseContext)

        runBlocking {
            val document = db.documentDao().getById(documentId)
            binding.toolbar.subtitle = document.scannedOn!!.toZonedDateTime().toNeatDateString()
            binding.editTextDocumentName.setText(document.name)

        }

        binding.editTextDocumentName.selectAll()
        binding.editTextDocumentName.requestFocus()

        binding.buttonSave.setOnClickListener {
            Save()
        }
    }

    private fun Save() {

        val newName = binding.editTextDocumentName.text.toString()
        if (newName.isBlank()) {
            binding.editTextDocumentName.error = "Please enter a name"
            return
        }

        binding.editTextDocumentName.error = null

        runBlocking {
            val document = db.documentDao().getById(documentId)
            if (document.name != newName) {
                db.documentDao().updateName(documentId, newName)
            }
        }

        finish()
    }

    override fun onEnterAnimationComplete() {
        super.onEnterAnimationComplete()
        binding.editTextDocumentName.showKeyboard()
    }
}