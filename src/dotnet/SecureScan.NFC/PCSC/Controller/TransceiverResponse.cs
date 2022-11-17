namespace SecureScan.NFC.PCSC.Controller
{
  public class TransceiverResponse
  {
    public TransceiverResponse(byte sw1, byte sw2, byte[] data)
    {
      Sw1 = sw1;
      Sw2 = sw2;
      Data = data;
    }

    public byte Sw1 { get; }
    public byte Sw2 { get; }
    public byte[] Data { get; }
  }
}
