using System;
using System.IO;
using System.Security.Cryptography.X509Certificates;
using Org.BouncyCastle.Asn1.Pkcs;
using Org.BouncyCastle.Asn1.Sec;
using Org.BouncyCastle.Asn1.X509;
using Org.BouncyCastle.Asn1.X9;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Generators;
using Org.BouncyCastle.Crypto.Operators;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Math;
using Org.BouncyCastle.Pkcs;
using Org.BouncyCastle.Security;
using Org.BouncyCastle.X509;
using X509Certificate = Org.BouncyCastle.X509.X509Certificate;

namespace SecureScanOutlookAddIn
{
  internal class X509Manager
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
      certGenerator.SetNotAfter(DateTime.UtcNow.AddMonths(6));
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

    private static (X509Certificate cert, AsymmetricCipherKeyPair keypair) CreateCertificate(string emailaddress, string name, string pc)
    {
      var eeName = new X509Name($"CN={emailaddress}, O={name}, L={pc}");
      var eeKey = GenerateRsaKeyPair(2048);
      //var eeKey = GenerateEcKeyPair();

      var eeCert = GenerateCertificate(eeName, eeName, eeKey.Private, eeKey.Public);
      var eeOk = ValidateSelfSignedCert(eeCert, eeKey.Public);

      if (!eeOk)
      {
        throw new Exception("Self-signed certificate is not valid");
      }

      return (eeCert, eeKey);
    }

    public static X509Certificate2 RetrieveOrCreateCertificate(string emailaddress, string name, string pc)
    {
      X509Certificate2 activeCert = null;

      using (var store = new X509Store(StoreName.My, StoreLocation.CurrentUser))
      {
        store.Open(OpenFlags.ReadWrite);
        var certs = store.Certificates.Find(X509FindType.FindBySubjectName, emailaddress, false);

        foreach (var cert in certs)
        {
          if (cert.HasPrivateKey && cert.NotAfter > DateTime.UtcNow)
          {
            activeCert = cert;
            break;
          }
        }

        if (activeCert != null)
        {
          var pktest = activeCert.GetRSAPrivateKey();
          return activeCert;
        }

        var (bccert, keypair) = CreateCertificate(emailaddress, name, pc);
        var rand = new SecureRandom();

        using (var p12Stream = new MemoryStream())
        {
          var p12 = new Pkcs12Store();
          p12.SetKeyEntry(emailaddress, new AsymmetricKeyEntry(keypair.Private), new X509CertificateEntry[] { new X509CertificateEntry(bccert) });
          p12.Save(p12Stream, "--".ToCharArray(), rand);

          //activeCert = new X509Certificate2(p12Stream.ToArray(), "--", X509KeyStorageFlags.DefaultKeySet);
          activeCert = new X509Certificate2(p12Stream.ToArray(), "--", X509KeyStorageFlags.Exportable);

          store.Add(activeCert);
        }
        store.Close();
      }

      return activeCert;
    }
  }
}