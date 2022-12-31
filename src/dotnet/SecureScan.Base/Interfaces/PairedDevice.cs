namespace SecureScan.Base.Interfaces
{
  public class PairedDevice
  {
    public string ID { get; set; }

    public string Name { get; set; }

    public override string ToString() => Name;
  }
}
