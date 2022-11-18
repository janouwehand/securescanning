using System;
using System.Linq;
using System.Threading;
using PCSC;
using PCSC.Exceptions;
using PCSC.Iso7816;

namespace SecureScan.NFC.PCSC.Controller
{
  public class PCSCController
  {
    //public PCSCController() : this(new AID(new byte[] { 0xF0, 0x01, 0x02, 0x03, 0x04, 0x05, 0x06 })) { }

    public PCSCController(AID aid) => Aid = aid;

    public AID Aid { get; }

    public bool WaitForConnection(Action<PCSCConnection> onConnection, int timeoutSeconds = 10)
    {
      ISCardContext context = null;
      IsoReader isoReader = null;

      var timeout = DateTime.Now.AddSeconds(timeoutSeconds);

      while (isoReader == null && DateTime.Now < timeout)
      {
        var tryOpenResult = TryOpenApplication();
        context = tryOpenResult.context;
        isoReader = tryOpenResult.reader;

        if (isoReader == null)
        {
          Thread.Sleep(500);
        }
      }

      if (isoReader == null)
      {
        return false;
      }
      else
      {
        using (context)
        using (isoReader)
        {
          var apdu = new CommandApdu(IsoCase.Case4Short, isoReader.ActiveProtocol)
          {
            CLA = 0x00, // Class
            Instruction = InstructionCode.SelectFile,
            P1 = 0x04, // Parameter 1
            P2 = 0x00, // Parameter 2
            Le = 13, // Expected length of the returned data					 
            Data = Aid
          };

          var response = isoReader.Transmit(apdu);
          if (response.HasData)
          {
            var data = response.GetData();
            onConnection?.Invoke(new PCSCConnection(true, data, new Transceiver(isoReader)));
            return true;
          }
          else
          {
            // Application error
            onConnection?.Invoke(new PCSCConnection(false, null, null));
            return false;
          }
        }
      }
    }

    private (ISCardContext context, IsoReader reader) TryOpenApplication()
    {
      var context = ContextFactory.Instance.Establish(SCardScope.System);
      var readerNames = context.GetReaders();
      var readerName = readerNames.FirstOrDefault() ?? throw new Exception("No reader!");

      try
      {
        var isoReader = new IsoReader(context: context, readerName: readerName, mode: SCardShareMode.Shared, protocol: SCardProtocol.Any, releaseContextOnDispose: false);
        return (context, isoReader);
      }
      catch (RemovedCardException)
      {
        context?.Dispose();
        return (null, null);
      }
      catch (ReaderUnavailableException)
      {
        context?.Dispose();
        return (null, null);
      }
    }

  }
}

