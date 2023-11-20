using System.Linq;
using SecureScan.Base.Extensions;
using SecureScan.NFC.PCSC.Controller;
using static SecureScan.NFC.Protocol.Messages.EnrollMessage3SendBindingSignatureToMFP;

namespace SecureScan.NFC.Protocol.Messages
{
  /// <summary>
  /// Enrolling: signing both certificates
  /// </summary>
  internal class EnrollMessage3SendBindingSignatureToMFP : MessageBase<Input, Output>
  {
    private const byte APDUINSTRUCTION = 0x53;

    public class Input
    {
      /// <summary>
      /// Digital signature over { CertMFP, CertSmartphone } using MFP's private key.
      /// </summary>
      public byte[] DigitalSignatureOfBothCertificatesCombined { get; }

      public Input(byte[] digitalSignatureOfBothCertificatesCombined) => DigitalSignatureOfBothCertificatesCombined = digitalSignatureOfBothCertificatesCombined;
    }

    public class Output
    {
      public bool SignatureVerifiedOKBySmartphone { get; }

      public Output(bool signatureVerifiedOKBySmartphone) => SignatureVerifiedOKBySmartphone = signatureVerifiedOKBySmartphone;
    }

    public override byte APDUInstruction => APDUINSTRUCTION;

    public EnrollMessage3SendBindingSignatureToMFP(PCSCConnection connection) : base(connection) { }

    protected override Output InnerExecute(Input input)
    {
      var responses = Connection.Transceiver.SendMultiApduData(APDUINSTRUCTION, input.DigitalSignatureOfBothCertificatesCombined);
      var lastResponse = responses.Last();
      var success = lastResponse.Data.TimedEquals(new byte[] { Constants.AFFIRMATIVE });
      return new Output(success);
    }
  }
}
