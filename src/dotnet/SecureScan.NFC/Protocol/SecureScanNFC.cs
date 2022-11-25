using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using SecureScan.Base.Crypto;
using SecureScan.Base.Crypto.Symmetric;
using SecureScan.Base.Extensions;
using SecureScan.Base.Logger;
using SecureScan.NFC.PCSC.Controller;

namespace SecureScan.NFC.Protocol
{
  internal class SecureScanNFC : ISecureScanNFC
  {
    private readonly ILogger logger;
    private readonly ISymmetricEncryption symmetricEncryption;

    public SecureScanNFC(ILogger logger, ISymmetricEncryption symmetricEncryption)
    {
      this.logger = logger;
      this.symmetricEncryption = symmetricEncryption;
    }

    public async Task<OwnerInfo> RetrieveOwnerInfoAsync(TimeSpan waitForNFCTimeout, CancellationToken cancellationToken)
    {
      var controller = new PCSCController(Constants.APPLICATIONID);
      using (var connection = await controller.WaitForConnectionAsync(waitForNFCTimeout.Seconds))
      {
        var ownerInfo = RetrieveInfo(connection);
        return ownerInfo;
      }
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

      info.X509 = RetrieveX509(nfc.Transceiver);
      logger.Log($"NFC: X.509 received successfully (size: {info.X509.Length} bytes)", Color.DarkGoldenrod);

      if (!PerformChallenge(nfc.Transceiver, info.X509Certificate().Value))
      {
        throw new Exception("Challenge failed: smartphone did not prove to posess private key.");
      }


      return info;
    }

    private bool PerformChallenge(Transceiver transceiver, X509Certificate2 x509)
    {
      var plainText = CryptoRandom.GetBytes(32);
      var cipherText = x509.EncryptWithPublicKey(plainText).Take(100).ToArray();

      var response = transceiver.Transceive(Constants.CMDCHALLENGE, null, cipherText);

      var retCipherText = response.Data;
      var retPlainText = x509.DecryptWithPublicKey(retCipherText);

      return plainText.TimedEquals(retPlainText);
    }

    private byte[] RetrieveX509(Transceiver transceiver)
    {
      var list = new List<byte>();

      bool AddPart(int partnr)
      {
        var response = transceiver.Transceive(Constants.CMDGETX509, partnr);
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

      return list.ToArray();
    }

  }
}
