package nl.ou.securescan.abilities

import android.app.Activity
import android.content.Context
import android.content.pm.PackageManager
import com.karumi.dexter.Dexter
import com.karumi.dexter.PermissionToken
import com.karumi.dexter.listener.PermissionDeniedResponse
import com.karumi.dexter.listener.PermissionGrantedResponse
import com.karumi.dexter.listener.PermissionRequest
import com.karumi.dexter.listener.single.PermissionListener

typealias PermissionHandler = (name: String, requested: Boolean, granted: Boolean) -> Unit

class Permissions {

    fun hasPermission(context: Context, name: String): Boolean {
        return when (context.checkSelfPermission(name)) {
            PackageManager.PERMISSION_GRANTED -> true
            else -> false
        }
    }

    fun ensurePermission(activity: Activity, name: String, handler: PermissionHandler) {

        if (hasPermission(activity.baseContext, name)) {
            handler(name, false, true)
            return
        }

        Dexter.withActivity(activity)
            .withPermission(name)
            .withListener(object : PermissionListener {
                override fun onPermissionGranted(response: PermissionGrantedResponse) {
                    handler(name, true, true)
                }

                override fun onPermissionDenied(response: PermissionDeniedResponse) {
                    handler(name, true, false)
                }

                override fun onPermissionRationaleShouldBeShown(
                    permission: PermissionRequest?,
                    token: PermissionToken
                ) = token.continuePermissionRequest()
            }).check()
    }
}