using System;

namespace SecureScan.Base.Extensions
{
  public static class ByteExtensions
  {
    public static string ToHEX(this byte[] bs) => BitConverter.ToString(bs, 0, bs.Length).Replace("-", "").ToLower();

    public static bool TimedEquals(this byte[] bs1, byte[] bs2)
    {
      if (bs1 == null || bs2 == null)
      {
        throw new ArgumentException("Cannot be null!");
      }

      var diff = (uint)bs1.Length ^ (uint)bs2.Length;

      for (var i = 0; i < bs1.Length && i < bs2.Length; i++)
      {
        diff |= (uint)(bs1[i] ^ bs2[i]);
      }

      return diff == 0;
    }
  }
}
