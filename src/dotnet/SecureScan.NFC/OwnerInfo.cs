using System;
using System.Security.Cryptography.X509Certificates;

namespace SecureScan.NFC
{
  public class OwnerInfo
  {
    public string Name { get; set; }

    public string Email { get; set; }

    public byte[] X509 { get; set; }

    public string ApplicationVersion { get; set; }

    public Lazy<X509Certificate2> X509Certificate() => X509 == null ? null : new Lazy<X509Certificate2>(() => new X509Certificate2(X509));
  }
}
