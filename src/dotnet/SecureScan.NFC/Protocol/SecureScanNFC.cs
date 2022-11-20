using System;
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
      }

      return null;
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
      logger.Log($"NFC: remote application version = {info.ApplicationVersion}");

      info.Name= RetrieveOwnerName(nfc.Transceiver);
      logger.Log($"NFC: owner name = {info.Name}");

      return info;
    }

    private string RetrieveOwnerName(Transceiver transceiver)
    {
      var response = transceiver.Transceive(Constants.CMDOWNERNAME);
      var name = Encoding.UTF8.GetString(response.Data);
      return name;
    }

  }
}
