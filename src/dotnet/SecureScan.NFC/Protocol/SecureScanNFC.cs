using System;
using System.Drawing;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using SecureScan.Base.Crypto;
using SecureScan.Base.Extensions;
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
        throw new Exception("Challenge failed: smartphone did not prove to own private key.");
      }

      return info;
    }

    private bool PerformChallenge(Transceiver transceiver, X509Certificate2 x509)
    {
      var randomData = CryptoRandom.GetBytes(32);

      logger.Log($"Sending challenge: {randomData.ToHEX()} (size: {randomData.Length} bytes)", Color.DarkOrange);

      var signature = transceiver.RetrieveMultiApduData(Constants.CMDCHALLENGE, randomData, out _);

      logger.Log($"Signature for challenge received (size: {signature.Length}). Now verifying...", Color.DarkOrange);

      var valid = x509.VerifySignature(randomData, signature);

      if (valid)
      {
        logger.Log($"Signature sucessfully verified!", Color.DarkOrange);
      }

      return valid;
    }

    private byte[] RetrieveX509(Transceiver transceiver) => transceiver.RetrieveMultiApduData(Constants.CMDGETX509, null, out _);

  }
}
