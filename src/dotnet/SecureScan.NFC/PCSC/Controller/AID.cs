using System;

namespace SecureScan.NFC.PCSC.Controller
{
  public class AID
  {
    public AID(byte[] bytes) => Bytes = bytes ?? throw new ArgumentNullException(nameof(bytes));

    public byte[] Bytes { get; set; }

    public override string ToString() => BitConverter.ToString(Bytes);

    public static implicit operator byte[](AID aid) => aid.Bytes;
  }
}
