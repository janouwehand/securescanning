package nl.ou.securescan

import android.content.Intent
import android.util.Log
import android.view.LayoutInflater
import android.view.View
import android.view.ViewGroup
import android.widget.TextView
import androidx.recyclerview.widget.RecyclerView
import nl.ou.securescan.data.Document
import nl.ou.securescan.helpers.toNeatDateString
import nl.ou.securescan.helpers.toZonedDateTime

class DocumentItemsAdapter(private val dataSet: Array<Document>) :
    RecyclerView.Adapter<DocumentItemsAdapter.ViewHolder>() {

    /**
     * Provide a reference to the type of views that you are using
     * (custom ViewHolder).
     */
    class ViewHolder(view: View) : RecyclerView.ViewHolder(view) {
        val textViewName: TextView
        val textViewDateTime: TextView
        val textViewId: TextView

        init {
            // Define click listener for the ViewHolder's View.
            textViewName = view.findViewById(R.id.textViewName)
            textViewDateTime = view.findViewById(R.id.textViewDateTime)
            textViewId = view.findViewById(R.id.textViewId)
        }
    }

    // Create new views (invoked by the layout manager)
    override fun onCreateViewHolder(viewGroup: ViewGroup, viewType: Int): ViewHolder {
        // Create a new view, which defines the UI of the list item
        val view = LayoutInflater.from(viewGroup.context)
            .inflate(R.layout.list_item, viewGroup, false)

        return ViewHolder(view)
    }

    // Replace the contents of a view (invoked by the layout manager)
    override fun onBindViewHolder(viewHolder: ViewHolder, position: Int) {
        // Get element from your dataset at this position and replace the
        // contents of the view with that element
        viewHolder.textViewId.text = dataSet[position].id.toString()
        viewHolder.textViewName.text = dataSet[position].name

        val scannedOn = dataSet[position].scannedOn!!
        var scannedOnDateTime = scannedOn.toZonedDateTime()
        viewHolder.textViewDateTime.text = scannedOnDateTime.toNeatDateString()

        val documentId = dataSet[position].id!!

        viewHolder.itemView.setOnClickListener { view ->
            val intent = Intent(view.context, DocumentInfoActivity::class.java)
            intent.putExtra("DocumentId", documentId)
            view.context.startActivity(intent)
        }
    }

    // Return the size of your dataset (invoked by the layout manager)
    override fun getItemCount() = dataSet.size

    override fun getItemId(position: Int): Long {
        return dataSet[position].id!!.toLong()
    }
}
