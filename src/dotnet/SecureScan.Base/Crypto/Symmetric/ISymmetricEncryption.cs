namespace SecureScan.Base.Crypto.Symmetric
{
  public interface ISymmetricEncryption
  {
    byte[] Encrypt(byte[] plaintext, byte[] password);

    byte[] Decrypt(byte[] ciphertext, byte[] password);
  }
}
