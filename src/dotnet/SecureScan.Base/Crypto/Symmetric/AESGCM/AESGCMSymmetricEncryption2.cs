using System;
using System.Linq;
using Org.BouncyCastle.Crypto.Engines;
using Org.BouncyCastle.Crypto.Modes;
using Org.BouncyCastle.Crypto.Parameters;

namespace SecureScan.Base.Crypto.Symmetric.AESGCM
{
  public class AESGCMSymmetricEncryption2 : ISymmetricEncryption
  {
    private const int KEYBYTESIZE = 32;
    private const int IVSIZE = 12;
    private const int MACBITSIZE = 128;

    public byte[] Encrypt(byte[] plaintext, byte[] key)
    {
      if (key.Length != KEYBYTESIZE)
      {
        throw new Exception($"Invalid key length: {KEYBYTESIZE} bytes required!");
      }

      var iv = CryptoRandom.GetBytes(IVSIZE);

      var cipher = new GcmBlockCipher(new AesEngine());
      var parameters = new AeadParameters(new KeyParameter(key), MACBITSIZE, iv);
      cipher.Init(true, parameters);

      var outputSize = cipher.GetOutputSize(plaintext.Length);
      var cipherTextBuffer = new byte[outputSize];

      var len = cipher.ProcessBytes(plaintext, 0, plaintext.Length, cipherTextBuffer, 0);
      cipher.DoFinal(cipherTextBuffer, len);

      var bsenc = iv
        .Concat(cipherTextBuffer)
        .ToArray();

      return bsenc;
    }

    public byte[] Decrypt(byte[] source, byte[] key)
    {
      var iv = source.Take(IVSIZE).ToArray();
      var ciphertext = source.Skip(IVSIZE).ToArray();

      var cipher = new GcmBlockCipher(new AesEngine());

      var parameters = new AeadParameters(new KeyParameter(key), MACBITSIZE, iv);
      cipher.Init(false, parameters);

      var plainText = new byte[cipher.GetOutputSize(ciphertext.Length)];
      var len = cipher.ProcessBytes(ciphertext, 0, ciphertext.Length, plainText, 0);
      cipher.DoFinal(plainText, len);

      return plainText;
    }

  }
}
