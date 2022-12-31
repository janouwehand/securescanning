package nl.ou.securescan.permissions

import android.Manifest
import android.content.pm.PackageManager
import android.os.Build
import androidx.annotation.RequiresApi
import androidx.appcompat.app.AppCompatActivity
import androidx.core.app.ActivityCompat

class PermissionHandler(private val activity: AppCompatActivity) {

    fun ensurePermissions(requestMissingPermissions: Boolean = true): Boolean {
        var ok = true

        ok = ok && ensurePermission_BLUETOOTH(requestMissingPermissions)
        ok = ok && ensurePermission_BLUETOOTH_ADMIN(requestMissingPermissions)

        if (Build.VERSION.SDK_INT >= Build.VERSION_CODES.S) {
            ok = ok && ensurePermission_BLUETOOTH_ADVERTISE(requestMissingPermissions)
        }

        if (Build.VERSION.SDK_INT >= Build.VERSION_CODES.S) {
            ok = ok && ensurePermission_BLUETOOTH_CONNECT(requestMissingPermissions)
        }

        ok = ok && ensurePermission_ACCESS_COARSE_LOCATION(requestMissingPermissions)
        ok = ok && ensurePermission_ACCESS_FINE_LOCATION(requestMissingPermissions)
        ok = ok && ensurePermission_RECEIVE_BOOT_COMPLETED(requestMissingPermissions)

        return ok
    }

    private fun ensurePermission_BLUETOOTH(requestMissingPermissions: Boolean): Boolean {
        if (ActivityCompat.checkSelfPermission(
                activity,
                Manifest.permission.BLUETOOTH
            ) != PackageManager.PERMISSION_GRANTED
        ) {
            if (requestMissingPermissions)
                ActivityCompat.requestPermissions(
                    activity,
                    arrayOf(Manifest.permission.BLUETOOTH),
                    1
                )
            return false
        }
        return true
    }

    private fun ensurePermission_BLUETOOTH_ADMIN(requestMissingPermissions: Boolean): Boolean {
        if (ActivityCompat.checkSelfPermission(
                activity,
                Manifest.permission.BLUETOOTH_ADMIN
            ) != PackageManager.PERMISSION_GRANTED
        ) {
            if (requestMissingPermissions)
                ActivityCompat.requestPermissions(
                    activity,
                    arrayOf(Manifest.permission.BLUETOOTH_ADMIN),
                    1
                )
            return false
        }
        return true
    }

    @RequiresApi(Build.VERSION_CODES.S)
    private fun ensurePermission_BLUETOOTH_ADVERTISE(requestMissingPermissions: Boolean): Boolean {
        if (ActivityCompat.checkSelfPermission(
                activity,
                Manifest.permission.BLUETOOTH_ADVERTISE
            ) != PackageManager.PERMISSION_GRANTED
        ) {
            if (requestMissingPermissions)
                ActivityCompat.requestPermissions(
                    activity,
                    arrayOf(Manifest.permission.BLUETOOTH_ADVERTISE),
                    1
                )
            return false
        }
        return true
    }

    @RequiresApi(Build.VERSION_CODES.S)
    private fun ensurePermission_BLUETOOTH_CONNECT(requestMissingPermissions: Boolean): Boolean {
        if (ActivityCompat.checkSelfPermission(
                activity,
                Manifest.permission.BLUETOOTH_CONNECT
            ) != PackageManager.PERMISSION_GRANTED
        ) {
            if (requestMissingPermissions)
                ActivityCompat.requestPermissions(
                    activity,
                    arrayOf(Manifest.permission.BLUETOOTH_CONNECT),
                    1
                )
            return false
        }
        return true
    }

    private fun ensurePermission_ACCESS_FINE_LOCATION(requestMissingPermissions: Boolean): Boolean {
        if (ActivityCompat.checkSelfPermission(
                activity,
                Manifest.permission.ACCESS_FINE_LOCATION
            ) != PackageManager.PERMISSION_GRANTED
        ) {
            if (requestMissingPermissions)
                ActivityCompat.requestPermissions(
                    activity,
                    arrayOf(Manifest.permission.ACCESS_FINE_LOCATION),
                    1
                )
            return false
        }
        return true
    }

    private fun ensurePermission_ACCESS_COARSE_LOCATION(requestMissingPermissions: Boolean): Boolean {
        if (ActivityCompat.checkSelfPermission(
                activity,
                Manifest.permission.ACCESS_COARSE_LOCATION
            ) != PackageManager.PERMISSION_GRANTED
        ) {
            if (requestMissingPermissions)
                ActivityCompat.requestPermissions(
                    activity,
                    arrayOf(Manifest.permission.ACCESS_COARSE_LOCATION),
                    1
                )
            return false
        }
        return true
    }

    private fun ensurePermission_RECEIVE_BOOT_COMPLETED(requestMissingPermissions: Boolean): Boolean {
        if (ActivityCompat.checkSelfPermission(
                activity,
                Manifest.permission.RECEIVE_BOOT_COMPLETED
            ) != PackageManager.PERMISSION_GRANTED
        ) {
            if (requestMissingPermissions)
                ActivityCompat.requestPermissions(
                    activity,
                    arrayOf(Manifest.permission.RECEIVE_BOOT_COMPLETED),
                    1
                )
            return false
        }
        return true
    }

}