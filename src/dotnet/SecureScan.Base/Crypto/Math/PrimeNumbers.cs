using System;
using System.Numerics;
using System.Security.Cryptography;

namespace SecureScan.Base.Crypto.Math
{
  public static class PrimeNumbers
  {
    private static BigInteger GeneratePrime(int bitsize)
    {
      var i = 0;
      BigInteger p;
      using (var rng = RandomNumberGenerator.Create())
      {
        var bytes = new byte[bitsize / 8];
        do
        {
          i++;
          rng.GetBytes(bytes);
          p = new BigInteger(bytes);
        }
        while (!IsProbablyPrime(p));
      }
      return p;
    }

    /// <summary>
    /// As per NIST SP 800-56A let's only work with safe primes.
    /// Note that the Miller-Rabin primality test is probabilistic and could return a non-prime number. However, with 5 witnesses that chance is rather small.
    /// </summary>
    public static BigInteger GenerateSafePrime(int bitsize)
    {
      while (true)
      {
        var prime = GeneratePrime(bitsize);
        var safePrime = prime * 2 + 1;

        // But test if safePrime is actually prime too!
        if (IsProbablyPrime(safePrime))
        {
          return safePrime;
        }
      }
    }

    private static bool IsProbablyPrime(BigInteger value, int witnesses = 5)
    {
      if (value < 2)
      {
        return false;
      }

      if (value == 2 || value == 3)
      {
        return true;
      }

      if (value % 2 == 0)
      {
        return false;
      }

      var d = value - 1;
      var s = 0;

      while (d % 2 == 0)
      {
        d /= 2;
        s += 1;
      }

      var bytes = new byte[value.ToByteArray().LongLength];
      BigInteger a;

      for (var i = 0; i < witnesses; i++)
      {
        do
        {
          new Random().NextBytes(bytes);
          a = new BigInteger(bytes);
        }
        while (a < 2 || a >= value - 2);

        var x = BigInteger.ModPow(a, d, value);

        if (x == 1 || x == value - 1)
        {
          continue;
        }

        for (var r = 1; r < s; r++)
        {
          x = BigInteger.ModPow(x, 2, value);

          if (x == 1)
          {
            return false;
          }

          if (x == value - 1)
          {
            break;
          }
        }

        if (x != value - 1)
        {
          return false;
        }
      }

      return true;
    }
  }
}
