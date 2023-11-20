using System;
using System.Security.Cryptography.X509Certificates;
using SecureScan.Base.Crypto.Symmetric;
using SecureScan.Base.Crypto.Symmetric.AESGCM;
using SecureScan.NFC.PCSC.Controller;
using static SecureScan.NFC.Protocol.Messages.EnrollMessage2RetrieveX509OfSmartphone;

namespace SecureScan.NFC.Protocol.Messages
{
  /// <summary>
  /// Enrolling: smartphone and MFP exchange certificates.
  /// Authentication is enforced by encrypting the certificates using the symmetric key provided through a secure enough channel by the MFP (QR code on display).
  /// </summary>
  internal class EnrollMessage2RetrieveX509OfSmartphone : MessageBase<Input, Output>
  {
    private const byte APDUINSTRUCTION = 0x52;
    private readonly ISymmetricEncryption symmetricEncryption;

    public class Input
    {
      public Input(byte[] sessionKey) => SessionKey = sessionKey ?? throw new ArgumentNullException(nameof(sessionKey));

      /// <summary>
      /// The session key from the scanned QR.
      /// </summary>
      public byte[] SessionKey { get; set; }
    }

    public class Output
    {
      /// <summary>
      /// The retrieved certificate of the smartphone.
      /// </summary>
      public X509Certificate2 SmartphoneCertificate { get; set; }

      public Output(X509Certificate2 smartphoneCertificate) => SmartphoneCertificate = smartphoneCertificate;
    }

    public override byte APDUInstruction => APDUINSTRUCTION;

    public EnrollMessage2RetrieveX509OfSmartphone(PCSCConnection connection) : base(connection) => symmetricEncryption = new AESGCMSymmetricEncryption2();

    protected override Output InnerExecute(Input input)
    {
      var bsCipherText = Connection.Transceiver.RetrieveMultiApduData(APDUINSTRUCTION, null, out _);
      var bsPlainText = symmetricEncryption.Decrypt(bsCipherText, input.SessionKey);
      var x509certificate = new X509Certificate2(bsPlainText);
      return new Output(x509certificate);
    }
  }
}
