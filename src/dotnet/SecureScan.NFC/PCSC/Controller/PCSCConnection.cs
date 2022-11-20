using System;
using PCSC;
using PCSC.Iso7816;

namespace SecureScan.NFC.PCSC.Controller
{
  public class PCSCConnection : IDisposable
  {
    private readonly ISCardContext context;
    private readonly IsoReader isoReader;

    public PCSCConnection(ISCardContext context, IsoReader isoReader, byte[] returnData)
    {
      this.context = context;
      this.isoReader = isoReader;
      ReturnData = returnData;
      Transceiver = new Transceiver(isoReader);
    }

    public byte[] ReturnData { get; }
    public Transceiver Transceiver { get; }

    public void Dispose()
    {
      context.Dispose();
      isoReader.Dispose();
    }
  }
}
