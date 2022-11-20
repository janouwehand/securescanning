namespace SecureScan.NFC.PCSC
{
  public class Command
  {
    public Command(string name, byte p1, int? p2count = null)
    {
      Name = name;
      P1 = p1;
      P2Count = p2count;
    }

    public string Name { get; }

    public byte P1 { get; }

    public int? P2Count { get; }
  }
}
