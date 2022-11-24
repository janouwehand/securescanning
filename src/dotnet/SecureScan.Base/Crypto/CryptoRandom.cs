using System;
using System.Security.Cryptography;

namespace SecureScan.Base.Crypto
{
  public static class CryptoRandom
  {
    public static byte[] GetBytes(int length)
    {
      var bs = new byte[length];
      using (var rng = new RNGCryptoServiceProvider())
      {
        rng.GetBytes(bs);
      }
      return bs;
    }

    public static int Next(int minValue, int maxValue)
    {
      if (minValue > maxValue)
      {
        throw new ArgumentOutOfRangeException(nameof(minValue));
      }

      if (minValue == maxValue)
      {
        return minValue;
      }

      var _uint32Buffer = new byte[4];

      using (var rng = new RNGCryptoServiceProvider())
      {
        long diff = maxValue - minValue;
        while (true)
        {
          rng.GetBytes(_uint32Buffer);
          var rand = BitConverter.ToUInt32(_uint32Buffer, 0);

          var max = (1 + (long)uint.MaxValue);
          var remainder = max % diff;
          if (rand < max - remainder)
          {
            return (int)(minValue + (rand % diff));
          }
        }
      }
    }

    public static Guid NewGuid()
    {
      var bs = GetBytes(16);
      return new Guid(bs);
    }

  }
}
