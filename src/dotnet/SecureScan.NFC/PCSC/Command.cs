namespace SecureScan.NFC.PCSC
{
  public class Command
  {
    public Command(string name, byte instruction, int? blockCount = null)
    {
      Name = name;
      Instruction = instruction;
      BlockCount = blockCount;
    }

    public string Name { get; }

    public byte Instruction { get; }

    public int? BlockCount { get; }
  }
}
