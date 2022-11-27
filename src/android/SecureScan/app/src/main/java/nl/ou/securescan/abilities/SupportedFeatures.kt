package nl.ou.securescan.abilities

import android.content.Context
import android.content.pm.PackageManager

class SupportedFeatures {

    data class MissingFeature(val feature: String, val name: String)

    companion object {
        val requiredFeatures = arrayOf(
            MissingFeature(PackageManager.FEATURE_NFC_HOST_CARD_EMULATION, "NFC Host Card Emulation"),
            MissingFeature(PackageManager.FEATURE_BLUETOOTH_LE, "Bluetooth Low Energy (BLE)")
        )
    }

    fun checkRequiredFeatures(context: Context): Array<String> {
        var missing = arrayOf<String>()
        val pm = context.packageManager
        for (req in requiredFeatures) {
            var has = pm.hasSystemFeature(req.feature)
            if (!has) {
                missing = missing.plus(req.name)
            }
        }
        return missing
    }

}
