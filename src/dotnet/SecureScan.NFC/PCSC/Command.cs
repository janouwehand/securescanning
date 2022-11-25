namespace SecureScan.NFC.PCSC
{
  public class Command
  {
    public Command(string name, byte instruction)
    {
      Name = name;
      Instruction = instruction;
    }

    public string Name { get; }

    public byte Instruction { get; }
  }
}
