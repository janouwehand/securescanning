using SecureScan.NFC.PCSC.Controller;
using static SecureScan.NFC.Protocol.Messages.EnrollMessage5FinishEnrollment;

namespace SecureScan.NFC.Protocol.Messages
{
  /// <summary>
  /// Enrolling: signing both certificates
  /// </summary>
  internal class EnrollMessage5FinishEnrollment : MessageBase<Input, Output>
  {
    private const byte APDUINSTRUCTION = 0x55;

    public class Input { }

    public class Output
    {
      public bool Success { get; set; }

      public Output(bool success) => Success = success;
    }

    public override byte APDUInstruction => APDUINSTRUCTION;

    public EnrollMessage5FinishEnrollment(PCSCConnection connection) : base(connection) { }

    protected override Output InnerExecute(Input input)
    {
      var result = Connection.Transceiver.Transceive(APDUINSTRUCTION);
      return new Output(result.Length > 0 && result[0] == Constants.AFFIRMATIVE);
    }
  }
}
