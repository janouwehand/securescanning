using System;
using Org.BouncyCastle.Asn1.Pkcs;
using Org.BouncyCastle.Asn1.Sec;
using Org.BouncyCastle.Asn1.X509;
using Org.BouncyCastle.Asn1.X9;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Generators;
using Org.BouncyCastle.Crypto.Operators;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Math;
using Org.BouncyCastle.Security;
using Org.BouncyCastle.X509;

namespace SecureScanOutlookAddIn
{
  internal class CreateX509
  {
    private static readonly SecureRandom secureRandom = new SecureRandom();

    private static AsymmetricCipherKeyPair GenerateRsaKeyPair(int length)
    {
      var keygenParam = new KeyGenerationParameters(secureRandom, length);

      var keyGenerator = new RsaKeyPairGenerator();
      keyGenerator.Init(keygenParam);
      return keyGenerator.GenerateKeyPair();
    }

    private static AsymmetricCipherKeyPair GenerateEcKeyPair(string curveName = "secp256r1")
    {
      var ecParam = SecNamedCurves.GetByName(curveName);
      var ecDomain = new ECDomainParameters(ecParam.Curve, ecParam.G, ecParam.N);
      var keygenParam = new ECKeyGenerationParameters(ecDomain, secureRandom);

      var keyGenerator = new ECKeyPairGenerator();
      keyGenerator.Init(keygenParam);
      return keyGenerator.GenerateKeyPair();
    }

    private static X509Certificate GenerateCertificate(
        X509Name issuer, X509Name subject,
        AsymmetricKeyParameter issuerPrivate,
        AsymmetricKeyParameter subjectPublic)
    {
      ISignatureFactory signatureFactory;
      if (issuerPrivate is ECPrivateKeyParameters)
      {
        signatureFactory = new Asn1SignatureFactory(
            X9ObjectIdentifiers.ECDsaWithSha256.ToString(),
            issuerPrivate);
      }
      else
      {
        signatureFactory = new Asn1SignatureFactory(
            PkcsObjectIdentifiers.Sha256WithRsaEncryption.ToString(),
            issuerPrivate);
      }

      var certGenerator = new X509V3CertificateGenerator();
      certGenerator.SetIssuerDN(issuer);
      certGenerator.SetSubjectDN(subject);
      certGenerator.SetSerialNumber(BigInteger.ValueOf(1));
      certGenerator.SetNotAfter(DateTime.UtcNow.AddHours(1));
      certGenerator.SetNotBefore(DateTime.UtcNow);
      certGenerator.SetPublicKey(subjectPublic);
      return certGenerator.Generate(signatureFactory);
    }

    private static bool ValidateSelfSignedCert(X509Certificate cert, ICipherParameters pubKey)
    {
      cert.CheckValidity(DateTime.UtcNow);
      var tbsCert = cert.GetTbsCertificate();
      var sig = cert.GetSignature();

      var signer = SignerUtilities.GetSigner(cert.SigAlgName);
      signer.Init(false, pubKey);
      signer.BlockUpdate(tbsCert, 0, tbsCert.Length);
      return signer.VerifySignature(sig);
    }

    public static X509Certificate CreateCertificate(string emailaddress, string name)
    {
      var eeName = new X509Name($"CN={emailaddress}, O={name}");
      //var eeKey = GenerateRsaKeyPair(2048);
      var eeKey = GenerateEcKeyPair();

      var eeCert = GenerateCertificate(eeName, eeName, eeKey.Private, eeKey.Public);
      var eeOk = ValidateSelfSignedCert(eeCert, eeKey.Public);

      //using (var f = File.OpenWrite("ee.cer"))
      {
        //var buf = eeCert.GetEncoded();
        //f.Write(buf, 0, buf.Length);
      }

      return eeCert;
    }

  }
}
