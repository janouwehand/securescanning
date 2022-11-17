using System.Linq;
using PCSC.Iso7816;

namespace SecureScan.NFC.PCSC.Controller
{
  public class Transceiver
  {
    private readonly IsoReader reader;

    public Transceiver(IsoReader reader) => this.reader = reader;

    public TransceiverResponse Transceive(byte cla, byte instruction, byte p1, byte p2, byte[] data)
    {
      var apdu = new CommandApdu(data == null || !data.Any() ? IsoCase.Case2Extended : IsoCase.Case4Extended, reader.ActiveProtocol)
      {
        CLA = cla, // Class
        Instruction = (InstructionCode)instruction,
        P1 = p1, // Parameter 1
        P2 = p2, // Parameter 2
        //Le = 13, // Expected length of the returned data					 
      };

      if (apdu.Case == IsoCase.Case4Extended)
      {
        apdu.Data = data;
      }

      var response = reader.Transmit(apdu);
      var responseData = response.HasData ? response.GetData() : null;
      return new TransceiverResponse(response.SW1, response.SW2, responseData);
    }

  }
}
