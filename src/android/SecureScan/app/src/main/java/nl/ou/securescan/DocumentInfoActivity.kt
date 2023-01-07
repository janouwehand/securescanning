package nl.ou.securescan

import android.os.Bundle
import android.view.Menu
import android.view.MenuItem
import androidx.appcompat.app.AppCompatActivity
import androidx.core.text.set
import kotlinx.coroutines.runBlocking
import nl.ou.securescan.crypto.extensions.toHexString
import nl.ou.securescan.data.Document
import nl.ou.securescan.data.DocumentDatabase
import nl.ou.securescan.databinding.ActivityCertificateInfoBinding
import nl.ou.securescan.databinding.ActivityDocumentInfoBinding
import nl.ou.securescan.helpers.*
import kotlin.properties.Delegates

class DocumentInfoActivity : AppCompatActivity() {

    private lateinit var binding: ActivityDocumentInfoBinding
    private lateinit var db: DocumentDatabase
    private var documentId by Delegates.notNull<Int>()

    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)

        binding = ActivityDocumentInfoBinding.inflate(layoutInflater)
        setContentView(binding.root)

        setSupportActionBar(binding.toolbar)

        val extras = intent.extras
        documentId = extras!!.getInt("DocumentId")

        binding.toolbar.title = "Document $documentId"
        binding.toolbar.setNavigationOnClickListener {
            if (binding.editTextDocumentName.hasFocus()) {
                binding.editTextDocumentName.hideKeyboard()
            }
            finish()
        }

        db = DocumentDatabase.getDatabase(baseContext)

        runBlocking {
            val document = db.documentDao().getById(documentId)
            binding.toolbar.subtitle = document.scannedOn!!.toZonedDateTime().toNeatDateString()
            binding.editTextDocumentName.setText(document.name)
            binding.textViewShaValue.text = document.documentHash!!.toHexString()
        }

        binding.editTextDocumentName.selectAll()
        binding.editTextDocumentName.requestFocus()

        binding.buttonSave.setOnClickListener {
            save()
        }
    }

    private fun save() {

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

        if (binding.editTextDocumentName.hasFocus()) {
            binding.editTextDocumentName.hideKeyboard()
        }

        finish()
    }

    override fun onEnterAnimationComplete() {
        super.onEnterAnimationComplete()
        binding.editTextDocumentName.showKeyboard()
    }

    override fun onCreateOptionsMenu(menu: Menu): Boolean {
        // Inflate the menu; this adds items to the action bar if it is present.
        menuInflater.inflate(R.menu.menu_documentinfo, menu)
        return true
    }

    override fun onOptionsItemSelected(item: MenuItem): Boolean {
        // Handle action bar item clicks here. The action bar will
        // automatically handle clicks on the Home/Up button, so long
        // as you specify a parent activity in AndroidManifest.xml.
        return when (item.itemId) {
            R.id.action_deletedocument -> deleteDocument()
            else -> super.onOptionsItemSelected(item)
        }
    }

    private fun deleteDocument(): Boolean {
        confirm("Are you sure that you want to delete this document?") { ok ->
            if (ok) {
                confirm("This action cannot be undone. Still delete?") { ok2 ->
                    if (ok2) {
                        val dao = db.documentDao()
                        runBlocking {
                            val document = dao.getById(documentId)
                            dao.delete(document)
                            finish()
                        }
                    }
                }
            }
        }
        return true
    }
}