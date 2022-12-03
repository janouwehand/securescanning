using System;
using System.Linq;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;

namespace SecureScan.Base.Extensions
{
  public static class X509Extensions
  {
    public class X509Parts
    {
      public string CN { get; set; }

      public string O { get; set; }

      public static X509Parts FromString(string s)
      {
        var rs = s
          .Split(new[] { "," }, StringSplitOptions.RemoveEmptyEntries)
          .Select(x => x.Trim())
          .ToArray();

        var model = new X509Parts
        {
          CN = rs.FirstOrDefault(x => x.StartsWith("CN="))?.Substring(3),
          O = rs.FirstOrDefault(x => x.StartsWith("O="))?.Substring(2)
        };
        return model;
      }
    }

    public static X509Parts GetSubjectParts(this X509Certificate2 x509) => X509Parts.FromString(x509.Subject);

    public static X509Parts GetIssuerParts(this X509Certificate2 x509) => X509Parts.FromString(x509.Issuer);

    public static string ToBase64Certificate(this X509Certificate x509)
    {
      var str = $@"-----BEGIN CERTIFICATE-----
{Convert.ToBase64String(x509.GetRawCertData(), Base64FormattingOptions.InsertLineBreaks)}
-----END CERTIFICATE-----
";
      return str;
    }

    public static byte[] EncryptWithPublicKey(this X509Certificate2 x509, byte[] plainText)
    {
      using (var rsa = x509.GetRSAPublicKey())
      {
        return rsa.Encrypt(plainText, RSAEncryptionPadding.Pkcs1);
      }
    }

    public static byte[] DecryptWithPublicKey(this X509Certificate2 x509, byte[] ciptherText)
    {
      using (var rsa = x509.GetRSAPublicKey())
      {
        return rsa.Decrypt(ciptherText, RSAEncryptionPadding.Pkcs1);
      }
    }

    public static bool VerifySignature(this X509Certificate2 x509, byte[] data, byte[] signature)
    {
      using (var rsa = x509.GetRSAPublicKey())
      {
        return rsa.VerifyData(data, signature, HashAlgorithmName.SHA256, RSASignaturePadding.Pkcs1);
      }
    }

  }
}
