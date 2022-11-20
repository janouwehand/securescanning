using System;
using System.Collections.Generic;
using System.Security.Cryptography;
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

      info.Name = RetrieveOwnerName(nfc.Transceiver);
      logger.Log($"NFC: owner name = {info.Name}");

      info.Email = RetrieveOwnerEmail(nfc.Transceiver);
      logger.Log($"NFC: owner email = {info.Email}");

      info.ECPublicKey = RetrieveOwnerPublicKey(nfc.Transceiver);
      logger.Log($"NFC: EC public key = {BitConverter.ToString(info.ECPublicKey).Replace("-", "").ToLower()}");

      TestKey(info.ECPublicKey);

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

      void AddPart(int partnr)
      {
        var response = transceiver.Transceive(Constants.CMDPUBKEY, partnr);
        list.AddRange(response.Data);
      }

      for (var i = 1; i <= 5; i++)
      {
        AddPart(i);
      }

      return list.ToArray();
    }

  }
}
