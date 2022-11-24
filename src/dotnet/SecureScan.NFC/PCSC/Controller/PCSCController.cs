using System;
using System.Linq;
using System.Threading.Tasks;
using PCSC;
using PCSC.Exceptions;
using PCSC.Iso7816;

namespace SecureScan.NFC.PCSC.Controller
{
  public class PCSCController
  {
    public PCSCController(AID aid) => Aid = aid;

    public AID Aid { get; }

    public async Task<PCSCConnection> WaitForConnectionAsync(int timeoutSeconds = 10)
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
          await Task.Delay(100);
        }
      }

      if (isoReader == null)
      {
        throw new Exception("Time-out waiting for NFC.");
      }

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
        return new PCSCConnection(context, isoReader, data);
      }
      else
      {
        throw new Exception("Application not found on smartphone!");
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

