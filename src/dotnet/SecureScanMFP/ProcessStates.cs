namespace SecureScanMFP
{
  internal enum ProcessStates
  {
    /// <summary>
    /// MFP is idle. User places document.
    /// </summary>
    Initial,

    /// <summary>
    /// User started secure scanning process. MFP starts advertising.
    /// </summary>
    Advertising,

    /// <summary>
    /// User successfully connected smartphone and smartphone info was recieved.
    /// The MFP locks in the smartphone - no other connections allowed.
    /// </summary>
    SmartphoneConnected,

    /// <summary>
    /// The user checked the smartphoneinfo on the MFP and hit start button to continue the scanning process.
    /// </summary>
    SmartphoneStartCommandReceived,

    /// <summary>
    /// Establishing a shared secert.
    /// </summary>
    KeyExchange,

    /// <summary>
    /// The smartphone sent owner info from the smartphone app.
    /// </summary>
    OwnerInfoReceived,

    /// <summary>
    /// The MFP created the protected container containing the document.
    /// </summary>
    ProtectedContainerCreated,

    LicenseSent,
    EmailSent
  }
}
