using System;

namespace SecureScan.Base.Extensions
{
  public static class ByteExtensions
  {
    public static string ToHEX(this byte[] bs) => BitConverter.ToString(bs, 0, bs.Length).Replace("-", "").ToLower();
  }
}
