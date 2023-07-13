using SecureScan.NFC.PCSC.Controller;
using static SecureScan.NFC.Protocol.Messages.EnrollMessage4RetrieveBindingSignatureFromSmartphone;

namespace SecureScan.NFC.Protocol.Messages
{
  /// <summary>
  /// Enrolling: signing both certificates
  /// </summary>
  internal class EnrollMessage4RetrieveBindingSignatureFromSmartphone : MessageBase<Input, Output>
  {
    private const byte APDUINSTRUCTION = 0x54;

    public class Input { }

    public class Output
    {
      public byte[] Signature { get; set; }

      public Output(byte[] signature) => Signature = signature;
    }

    public override byte APDUInstruction => APDUINSTRUCTION;

    public EnrollMessage4RetrieveBindingSignatureFromSmartphone(PCSCConnection connection) : base(connection) { }

    protected override Output InnerExecute(Input input)
    {
      var signature = Connection.Transceiver.Transceive(APDUINSTRUCTION);
      return new Output(signature);
    }
  }
}
