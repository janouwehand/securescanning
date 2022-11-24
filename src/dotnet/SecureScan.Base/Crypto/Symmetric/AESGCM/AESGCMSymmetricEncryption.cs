using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using Org.BouncyCastle.Crypto.Engines;
using Org.BouncyCastle.Crypto.Modes;
using Org.BouncyCastle.Crypto.Parameters;
using SecureScan.Base.Extensions;

namespace SecureScan.Base.Crypto.Symmetric.AESGCM
{
  internal class AESGCMSymmetricEncryption : ISymmetricEncryption
  {
    private const int KEYBYTESIZE = 32;
    private const int NONCEBYTESIZE = 12;
    private const int MACBITSIZE = 128;
    private readonly byte[] SIG = Encoding.UTF8.GetBytes("SecSca");

    public byte[] Encrypt(byte[] plaintext, byte[] key)
    {
      if (key.Length != KEYBYTESIZE)
      {
        throw new Exception($"Invalid key length: {KEYBYTESIZE} bytes required!");
      }

      var nonce = CryptoRandom.GetBytes(NONCEBYTESIZE);

      var cipher = new GcmBlockCipher(new AesEngine());
      var parameters = new AeadParameters(new KeyParameter(key), MACBITSIZE, nonce);
      cipher.Init(true, parameters);

      var outputSize = cipher.GetOutputSize(plaintext.Length);
      var cipherTextBuffer = new byte[outputSize];

      var len = cipher.ProcessBytes(plaintext, 0, plaintext.Length, cipherTextBuffer, 0);
      cipher.DoFinal(cipherTextBuffer, len);

      var bsenc = SIG
        .Concat(nonce)
        .Concat(cipherTextBuffer)
        .ToArray();

      return bsenc;
    }

    public byte[] Decrypt(byte[] source, byte[] key)
    {
      var sig = source.Take(SIG.Length).ToArray();
      var nonce = source.Skip(SIG.Length).Take(NONCEBYTESIZE).ToArray();
      var ciphertext = source.Skip(SIG.Length + NONCEBYTESIZE).ToArray();

      if (!sig.TimedEquals(SIG))
      {
        throw new CryptographicException("Signature does not match!");
      }

      var cipher = new GcmBlockCipher(new AesEngine());

      var parameters = new AeadParameters(new KeyParameter(key), MACBITSIZE, nonce);
      cipher.Init(false, parameters);

      var plainText = new byte[cipher.GetOutputSize(ciphertext.Length)];
      var len = cipher.ProcessBytes(ciphertext, 0, ciphertext.Length, plainText, 0);
      cipher.DoFinal(plainText, len);

      return plainText;
    }

  }
}
