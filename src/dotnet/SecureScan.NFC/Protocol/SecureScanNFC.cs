using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using SecureScan.Base.Logger;
using SecureScan.NFC.PCSC.Controller;

namespace SecureScan.NFC.Protocol
{
  internal class SecureScanNFC : ISecureScanNFC
  {
    private readonly ILogger logger;

    public SecureScanNFC(ILogger logger) => this.logger = logger;

    public async Task<OwnerInfo> RetrieveOwnerInfoAsync(TimeSpan waitForNFCTimeout, CancellationToken cancellationToken)
    {
      var controller = new PCSCController(Constants.APPLICATIONID);
      using (var connection = await controller.WaitForConnectionAsync(waitForNFCTimeout.Seconds))
      {
        var ownerInfo = RetrieveInfo(connection);
        return ownerInfo;
      }

      //return null;
    }

    private OwnerInfo RetrieveInfo(PCSCConnection nfc)
    {
      var info = new OwnerInfo();

      var applicationVersion = Encoding.UTF8.GetString(nfc.ReturnData);
      if (!applicationVersion.StartsWith(Constants.APPVERSIONPREFIX))
      {
        throw new Exception("Unexpected result for select");
      }

      info.ApplicationVersion = applicationVersion.Substring(Constants.APPVERSIONPREFIX.Length);
      logger.Log($"NFC: remote application version = {info.ApplicationVersion}", Color.DarkGoldenrod);
      
      info.X509 = RetrieveOwnerPublicKey(nfc.Transceiver);
      logger.Log($"NFC: X.509 received successfully (size: {info.X509.Length} bytes)", Color.DarkGoldenrod);

      TestKey(info.X509);

      return info;
    }

    private void TestKey(byte[] eCPublicKey)
    {
      //var rsa = RSACryptoServiceProvider.Create();

    }

    private string RetrieveOwnerName(Transceiver transceiver)
    {
      var response = transceiver.Transceive(Constants.CMDOWNERNAME);
      return Encoding.UTF8.GetString(response.Data);
    }

    private string RetrieveOwnerEmail(Transceiver transceiver)
    {
      var response = transceiver.Transceive(Constants.CMDOWNEREMAIL);
      return Encoding.UTF8.GetString(response.Data);
    }

    private byte[] RetrieveOwnerPublicKey(Transceiver transceiver)
    {
      var list = new List<byte>();

      bool AddPart(int partnr)
      {
        var response = transceiver.Transceive(Constants.CMDPUBKEY, partnr);
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

      for (var i = 1; i <= 5; i++)
      {
        AddPart(i);
      }

      return list.ToArray();
    }

  }
}
