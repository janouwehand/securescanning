package nl.ou.securescan

object constants {
    const val APPVERSION = "APPV://1.0.0"

    const val AFFIRMATIVE = 0xA0.toByte()

    const val NEGATIVE = 0xA1.toByte()

    const val WAITINGFORMOREDATA = 0xA1.toByte()

    const val EmptyByte: Byte = 0x00;

    const val SelectAPDU: Byte = 0xa4.toByte()

    object Messages {
        // Enrolling messages originating from MFP mock
        object Enrolling {
            const val EnrollMessage1SendX509OfMFP: Byte = 0x51
            const val EnrollMessage2RetrieveX509OfSmartphone: Byte = 0x52
            const val EnrollMessage3SendBindingSignatureToMFP: Byte = 0x53
            const val EnrollMessage4RetrieveBindingSignatureFromSmartphone: Byte = 0x54
            const val EnrollMessage5Finish: Byte = 0x55
        }

        // Request for user's X.509 originating from MFP mock
        const val GetX509: Byte = 0x50.toByte()

        // Receive and execute challenge: deliver proof that the smartphone has the private key associated with the certificate, provided to the MFP.
        const val Challenge: Byte = 0x60.toByte()

        // Receive hash of secure container to verify whether the smartphone could provide the key to the document that the MFP requests.
        const val ReceiveHashOfSecureContainer: Byte = 0x80.toByte()

        // Receive encrypted document from MFP to store it.
        const val ReceiveEncryptedPassword: Byte = 0x90.toByte()

        // Store document and return the document-id to the MFP.
        const val StoreDocumentReturnDocumentId: Byte = 0x91.toByte()
    }
}