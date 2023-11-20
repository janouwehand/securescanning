using System;
using System.Security.Cryptography.X509Certificates;
using SecureScan.Base.Crypto.Symmetric;
using SecureScan.Base.Crypto.Symmetric.AESGCM;
using SecureScan.NFC.PCSC.Controller;
using static SecureScan.NFC.Protocol.Messages.EnrollMessage1SendX509OfMFP;

namespace SecureScan.NFC.Protocol.Messages
{
  /// <summary>
  /// Enrolling: smartphone and MFP exchange certificates.
  /// Authentication is enforced by encrypting the certificates using the symmetric key provided through a secure enough channel by the MFP (QR code on display).
  /// </summary>
  internal class EnrollMessage1SendX509OfMFP : MessageBase<Input, Output>
  {
    private const byte APDUINSTRUCTION = 0x51;
    private readonly ISymmetricEncryption symmetricEncryption;

    public class Input
    {
      public Input(byte[] sessionKey, X509Certificate mFPCertificate)
      {
        SessionKey = sessionKey ?? throw new ArgumentNullException(nameof(sessionKey));
        MFPCertificate = mFPCertificate ?? throw new ArgumentNullException(nameof(mFPCertificate));
      }

      /// <summary>
      /// The session key from the scanned QR.
      /// </summary>
      public byte[] SessionKey { get; set; }

      /// <summary>
      /// The certificate of the MFP to send.
      /// </summary>
      public X509Certificate MFPCertificate { get; set; }
    }

    public class Output { }

    public override byte APDUInstruction => APDUINSTRUCTION;

    public EnrollMessage1SendX509OfMFP(PCSCConnection connection) : base(connection) => symmetricEncryption = new AESGCMSymmetricEncryption2();

    protected override Output InnerExecute(Input input)
    {
      var bsCipherText = symmetricEncryption.Encrypt(input.MFPCertificate.GetRawCertData(), input.SessionKey);
      Connection.Transceiver.SendMultiApduData(APDUINSTRUCTION, bsCipherText);
      return new Output();
    }
  }
}
