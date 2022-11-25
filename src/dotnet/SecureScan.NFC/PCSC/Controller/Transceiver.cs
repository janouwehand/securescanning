using System;
using System.Collections.Generic;
using System.Linq;
using PCSC.Iso7816;
using SecureScan.Base.Extensions;

namespace SecureScan.NFC.PCSC.Controller
{
  public class Transceiver
  {
    private const int MAXBLOCKSIZE = 200;

    private readonly IsoReader reader;

    public Transceiver(IsoReader reader) => this.reader = reader;

    public TransceiverResponse[] SendMultiApduData(Command command, byte[] data)
    {
      if (command is null)
      {
        throw new ArgumentNullException(nameof(command));
      }

      var useBlocks = data != null && data.Length > MAXBLOCKSIZE;
      if (!useBlocks)
      {
        var response = Transceive(0x00, command.Instruction, 0x00, 0x00, data);
        return new[] { response };
      }

      var responses = new List<TransceiverResponse>();

      var apduDatas = data.ToList().SplitList(MAXBLOCKSIZE).ToArray();
      for (var block = 1; block <= apduDatas.Length; block++)
      {
        var isLastBlock = block == apduDatas.Length;

        var apduData = apduDatas[block - 1].ToArray();
        var response = Transceive(0x00, command.Instruction, (byte)block, isLastBlock ? (byte)0xFF : (byte)0xFE, apduData);
        responses.Add(response);
      }

      return responses.ToArray();
    }

    public byte[] RetrieveMultiApduData(Command command, out TransceiverResponse[] responses)
    {
      if (command is null)
      {
        throw new ArgumentNullException(nameof(command));
      }

      var resps = new List<TransceiverResponse>();
      var list = new List<byte>();

      bool AddPart(int partnr)
      {
        var response = Transceive(0x00, command.Instruction, (byte)partnr, 0x00, null);
        resps.Add(response);
        if (response.Data != null && response.Data.Any())
        {
          list.AddRange(response.Data);
          return true;
        }
        else
        {
          return false;
        }
      }

      var part = 1;
      bool @continue;
      do
      {
        @continue = AddPart(part++);
      }
      while (@continue);

      responses = resps.ToArray();
      return list.ToArray();
    }

    public TransceiverResponse Transceive(byte cla, byte instruction, byte p1, byte p2, byte[] data)
    {
      if (data != null && data.Length > MAXBLOCKSIZE)
      {
        throw new Exception("Blocsize limit exceeded!");
      }

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

      if (responseData != null)
      {
        if (response.SW1 != instruction)
        {
          throw new Exception("Not the result for the instruction!");
        }
      }

      return new TransceiverResponse(response.SW1, response.SW2, responseData);
    }

  }
}
