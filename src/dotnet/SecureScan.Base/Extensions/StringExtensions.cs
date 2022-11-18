using System;
using System.Linq;

namespace SecureScan.Base.Extensions
{
  public static class StringExtensions
  {
    public static byte[] HexStringToBytes(this string hex) =>
      Enumerable.Range(0, hex.Length)
      .Where(x => x % 2 == 0)
      .Select(x => Convert.ToByte(hex.Substring(x, 2), 16))
      .ToArray();
  }
}
