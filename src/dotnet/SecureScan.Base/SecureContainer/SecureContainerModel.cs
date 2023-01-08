using System;
using System.Security.Cryptography.X509Certificates;
using SecureScan.Base.Extensions;
using SecureScan.Base.Files;

namespace SecureScan.Base.SecureContainer
{
  public class SecureContainerModel
  {
    private const string DATA = nameof(DATA);
    private const string ORIGINATOR = nameof(ORIGINATOR);

    public string CreatedOn { get; }
    
    public string SHA1 { get; }

    public string MfpName { get; }

    public X509Certificate2 Originator { get; }

    public byte[] EncryptedData { get; }

    public SecureContainerModel(X509Certificate2 originator, byte[] bsEncryptedData, string mfpName)
    {
      if (string.IsNullOrWhiteSpace(mfpName))
      {
        throw new ArgumentException($"'{nameof(mfpName)}' cannot be null or whitespace.", nameof(mfpName));
      }

      CreatedOn = DateTime.Now.ToString("O");
      Originator = originator ?? throw new ArgumentNullException(nameof(originator));
      EncryptedData = bsEncryptedData ?? throw new ArgumentNullException(nameof(bsEncryptedData));
      MfpName = mfpName;
    }

    private SecureContainerModel(X509Certificate2 originator, byte[] bsEncryptedData, string mfpName, string createdOn, string sha1)
    {
      Originator = originator;
      EncryptedData = bsEncryptedData;
      MfpName = mfpName;
      CreatedOn = createdOn;
      SHA1 = sha1;
    }

    public byte[] Write()
    {
      var compoundFile = new CompoundFile();

      compoundFile.Values[nameof(CreatedOn)] = CreatedOn;
      compoundFile.Values[nameof(MfpName)] = MfpName;

      compoundFile.AddPart(EncryptedData, DATA);
      compoundFile.AddPart(Originator.RawData, ORIGINATOR);

      return compoundFile.ToBytes();
    }

    public static SecureContainerModel Read(byte[] bsSecureContainer)
    {
      var sha1 = bsSecureContainer.ComputeSHA1();
      var compoundFile = CompoundFile.FromBytes(bsSecureContainer);
      var createdOn = compoundFile.Values[nameof(CreatedOn)];
      var mfpName = compoundFile.Values[nameof(MfpName)];
      var bsEncryptedData = compoundFile.GetPart(DATA).Data;
      var bsOriginator = compoundFile.GetPart(ORIGINATOR).Data;
      var originator = new X509Certificate2(bsOriginator);

      return new SecureContainerModel(originator, bsEncryptedData, mfpName, createdOn, sha1.ToHEX());
    }

  }
}
