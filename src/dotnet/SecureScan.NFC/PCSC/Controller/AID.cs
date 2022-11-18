using System;
using SecureScan.Base.Extensions;

namespace SecureScan.NFC.PCSC.Controller
{
  public class AID
  {
    public AID(byte[] bytes) => Bytes = bytes ?? throw new ArgumentNullException(nameof(bytes));

    public byte[] Bytes { get; set; }

    public override string ToString() => BitConverter.ToString(Bytes).Replace("-", "");

    public static implicit operator byte[](AID aid) => aid.Bytes;

    public static AID Parse(string hex)
    {
      if (string.IsNullOrWhiteSpace(hex))
      {
        throw new ArgumentException($"'{nameof(hex)}' cannot be null or whitespace.", nameof(hex));
      }

      var bs = hex.HexStringToBytes();

      return new AID(bs);
    }
  }
}
