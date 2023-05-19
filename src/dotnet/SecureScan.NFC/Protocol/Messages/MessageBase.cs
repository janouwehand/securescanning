using SecureScan.NFC.PCSC.Controller;

namespace SecureScan.NFC.Protocol.Messages
{
  /// <summary>
  /// A message can execute multiple APDU's to overcome the max data size of an APDU.
  /// </summary>
  internal abstract class MessageBase<INPUT, OUTPUT>
  {
    /// <summary>
    /// APDU instruction
    /// </summary>
    public abstract byte APDUInstruction { get; }

    public PCSCConnection Connection { get; }

    protected MessageBase(PCSCConnection connection) => Connection = connection;

    public OUTPUT Execute(INPUT input) => InnerExecute(input);

    protected abstract OUTPUT InnerExecute(INPUT input);
  }
}
