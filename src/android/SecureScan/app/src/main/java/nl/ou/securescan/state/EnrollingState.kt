package nl.ou.securescan.state

import java.security.cert.X509Certificate

object EnrollingState {
    // Symmetric encryption key obtained from scanning a QR on the MFP.
    var qrCodeKey: ByteArray? = null

    var mfpCertificate: X509Certificate? = null
}