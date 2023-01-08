namespace SecureScan.Base.Files
{
  public abstract class CompoundFilePart
  {
    public string Name { get; set; }
    public string ContentType { get; set; }
    public byte[] Data { get; set; }
  }
}
