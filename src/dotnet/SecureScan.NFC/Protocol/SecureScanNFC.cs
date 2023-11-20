using System;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using SecureScan.Base.Crypto;
using SecureScan.Base.Crypto.Symmetric.AESGCM;
using SecureScan.Base.Extensions;
using SecureScan.Base.Logger;
using SecureScan.NFC.PCSC.Controller;
using SecureScan.NFC.Protocol.Messages;

namespace SecureScan.NFC.Protocol
{
  internal class SecureScanNFC : ISecureScanNFC
  {
    private readonly ILogger logger;

    public SecureScanNFC(ILogger logger) => this.logger = logger;

    /// <summary>
    /// Retrieve X.509 from smartphone.
    /// </summary>
    /// <param name="qrSessionKey"></param>
    /// <param name="waitForNFCTimeout"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<OwnerInfo> RetrieveOwnerInfoAsync(byte[] qrSessionKey, TimeSpan waitForNFCTimeout, CancellationToken cancellationToken)
    {
      var controller = new PCSCController(Constants.APPLICATIONID);
      using (var connection = await controller.WaitForConnectionAsync((int)waitForNFCTimeout.TotalSeconds, cancellationToken))
      {
        var ownerInfo = RetrieveInfo(qrSessionKey, connection);
        return ownerInfo;
      }
    }

    public async Task<OwnerInfo> StartEnrolling(byte[] qrSessionKey, X509Certificate2 certificateOfMFP, TimeSpan waitForNFCTimeout, CancellationToken cancellationToken)
    {
      var controller = new PCSCController(Constants.APPLICATIONID);
      using (var connection = await controller.WaitForConnectionAsync((int)waitForNFCTimeout.TotalSeconds, cancellationToken))
      {
        logger.Log("Send AES-GCM encrypted certificate of MFP to smartphone");

        // Send AES-GCM encrypted certificate of MFP to smartphone
        var message1 = MessageFactory.Enrolling.CreateEnrollMessage1SendX509OfMFP(connection);
        message1.Execute(new EnrollMessage1SendX509OfMFP.Input(qrSessionKey, certificateOfMFP));

        logger.Log("Retrieve AES-GCM encrypted certificate from the smartphone");

        // Retrieve AES-GCM encrypted certificate from the smartphone
        var message2 = MessageFactory.Enrolling.CreateEnrollMessage2RetrieveX509OfSmartphone(connection);
        var message2result = message2.Execute(new EnrollMessage2RetrieveX509OfSmartphone.Input(qrSessionKey));

        logger.Log("Sign combination of certificates and send to smartphone.");

        // Sign combination of certificates.
        byte[] signature;
        var bytesToSign = certificateOfMFP.RawData.Concat(message2result.SmartphoneCertificate.RawData).ToArray();
        using (var pk = certificateOfMFP.GetRSAPrivateKey())
        {
          signature = pk.SignData(bytesToSign, HashAlgorithmName.SHA256, RSASignaturePadding.Pkcs1);
        }

        // Send signature and have the smartphone verify the signature
        var message3 = MessageFactory.Enrolling.CreateEnrollMessage3SendBindingSignatureToMFP(connection);
        var message3Output = message3.Execute(new EnrollMessage3SendBindingSignatureToMFP.Input(signature));

        if (!message3Output.SignatureVerifiedOKBySmartphone)
        {
          throw new Exception("Smartphone didn't verify signature successfully. No need to proceed with the enrollment.");
        }

        logger.Log("Retrieve signature from smartphone and verify it.");

        // Retrieve signature from smartphone and verify it.
        var message4 = new EnrollMessage4RetrieveBindingSignatureFromSmartphone(connection);
        var message4Output = message4.Execute(new EnrollMessage4RetrieveBindingSignatureFromSmartphone.Input());

        // Verify
        var data = message2result.SmartphoneCertificate.RawData.Concat(certificateOfMFP.RawData).ToArray();
        var signatureOK = message2result.SmartphoneCertificate.VerifySignature(data, message4Output.Signature);

        if (!signatureOK)
        {
          throw new Exception("Smartphone didn't produce a valid signature. No need to proceed with the enrollment.");
        }

        var message5 = new EnrollMessage5FinishEnrollment(connection);
        var message5Output = message5.Execute(new EnrollMessage5FinishEnrollment.Input());

        if (message5Output.Success)
        {
          // Finish: store signature
        }
        else
        {
          // Failed. Do not do anything
        }

        logger.Log("Success");

        //var ownerInfo = RetrieveInfo(aesKey, connection);
        //return ownerInfo;
        return null;
      }
    }

    private OwnerInfo RetrieveInfo(byte[] qrSessionKey, PCSCConnection nfc)
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

      var aes = new AESGCMSymmetricEncryption2();
      var bsDec = aes.Decrypt(info.X509, qrSessionKey);
      info.X509 = bsDec;

      if (info.X509 == null || !info.X509.Any())
      {
        throw new Exception($"NFC: did not receive X509 certificate!");
      }

      logger.Log($"NFC: X.509 received successfully (size: {info.X509.Length} bytes)", Color.DarkGoldenrod);

      return !PerformChallenge(nfc.Transceiver, info.X509Certificate().Value)
        ? throw new Exception("Challenge failed: smartphone did not prove to own private key.")
        : info;
    }

    private bool PerformChallenge(Transceiver transceiver, X509Certificate2 x509)
    {
      var randomData = CryptoRandom.GetBytes(32).ToArray();

      logger.Log($"Sending challenge: {randomData.ToHEX()} (size: {randomData.Length} bytes)", Color.DarkOliveGreen);

      var signature = transceiver.RetrieveMultiApduData(Constants.CMDCHALLENGE, randomData, out _);

      if (signature == null || !signature.Any())
      {
        logger.Log("Challenge failed: no signature received.", Color.Red);
        return false;
      }

      logger.Log($"Signature for challenge received (size: {signature.Length}). Now verifying...", Color.DarkOliveGreen);

      var valid = x509.VerifySignature(randomData, signature);

      if (valid)
      {
        logger.Log($"Signature sucessfully verified!", Color.DarkOliveGreen);
      }
      else
      {
        logger.Log($"Signature verification failed for {signature.ToHEX()}", Color.DarkOliveGreen);
      }

      return valid;
    }

    private byte[] RetrieveX509(Transceiver transceiver) => transceiver.RetrieveMultiApduData(Constants.CMDGETX509, null, out _);

    public async Task<DocumentInfo> SendSymmetricPasswordAndHash(byte[] secureContainerSHA1, byte[] password, X509Certificate2 certificate, TimeSpan waitForNFCTimeout, CancellationToken cancellationToken)
    {
      var docInfo = new DocumentInfo();

      var controller = new PCSCController(Constants.APPLICATIONID);
      using (var connection = await controller.WaitForConnectionAsync(waitForNFCTimeout.Seconds, cancellationToken))
      {
        PerformSendPasswordAndHash(connection, certificate, secureContainerSHA1, password, docInfo);
      }

      return docInfo;
    }

    private void PerformSendPasswordAndHash(PCSCConnection connection, X509Certificate2 certificate, byte[] secureContainerSHA1, byte[] password, DocumentInfo docInfo)
    {
      var applicationVersion = Encoding.UTF8.GetString(connection.ReturnData);
      if (!applicationVersion.StartsWith(Constants.APPVERSIONPREFIX))
      {
        throw new Exception("Unexpected result for select");
      }

      if (!PerformChallenge(connection.Transceiver, certificate))
      {
        throw new Exception("Challenge failed: smartphone did not prove to own private key.");
      }

      logger.Log($"Sending encrypted symmetric password of secure container (size: {password.Length}).");
      connection.Transceiver.SendMultiApduData(Constants.CMDSENDSECURECONTAINERPASSWORD, password);

      logger.Log($"Sending SHA1 of secure container: {secureContainerSHA1.ToHEX()}");
      connection.Transceiver.Transceive(0x00, Constants.CMDSENDSECURECONTAINERHASH.Instruction, 0x00, 0x00, secureContainerSHA1);

      logger.Log($"Sending store/get document-id instruction");
      var response = connection.Transceiver.Transceive(0x00, Constants.CMDSTOREGETDOCUMENTID.Instruction, 0x00, 0x00, secureContainerSHA1);

      var str = Encoding.UTF8.GetString(response.Data);
      docInfo.DocumentNumber = int.Parse(str);
    }

  }
}
