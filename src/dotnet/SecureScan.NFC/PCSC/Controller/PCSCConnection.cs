namespace SecureScan.NFC.PCSC.Controller
{
  public class PCSCConnection
  {
    public PCSCConnection(bool isConnected, byte[] returnData, Transceiver transceiver)
    {
      IsConnected = isConnected;
      ReturnData = returnData;
      Transceiver = transceiver;
    }

    public bool IsConnected { get; }
    public byte[] ReturnData { get; }
    public Transceiver Transceiver { get; }
  }
}
