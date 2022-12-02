package nl.ou.securescan.helpers

import android.app.AlertDialog
import androidx.appcompat.app.AppCompatActivity
import androidx.appcompat.app.AppCompatCallback

fun AppCompatActivity.alert(message: String, title: String? = null) {
    var builder = AlertDialog.Builder(this)
    builder.setMessage(message)
        .setCancelable(true)
        .setNeutralButton("OK") { dialog, _ ->
            dialog.dismiss()
        }

    if (title != null) {
        builder = builder.setTitle(title!!)
    }

    val msg = builder.create()
    msg.show()
}

fun AppCompatActivity.confirm(question: String, callback: (result: Boolean) -> Unit) {
    var builder = AlertDialog.Builder(this)
    builder.setMessage(question)
        .setCancelable(false)
        .setPositiveButton("Yes") { dialog, _ ->
            dialog.dismiss()
            callback.invoke(true)
        }
        .setNegativeButton("No") { dialog, _ ->
            dialog.dismiss()
            callback.invoke(false)
        }
        .setTitle("Confirm")

    val msg = builder.create()
    msg.show()
}