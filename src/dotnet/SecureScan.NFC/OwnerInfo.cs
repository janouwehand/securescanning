namespace SecureScan.NFC
{
  public class OwnerInfo
  {
    public string Name { get; set; }

    public string Email { get; set; }

    public byte[] RsaPublicKey { get; set; }
  }
}
