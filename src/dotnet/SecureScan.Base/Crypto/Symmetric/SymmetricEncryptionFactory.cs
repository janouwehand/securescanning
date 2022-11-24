using SecureScan.Base.Crypto.Symmetric.AESGCM;

namespace SecureScan.Base.Crypto.Symmetric
{
  public static class SymmetricEncryptionFactory
  {
    public static ISymmetricEncryption Create() => new AESGCMSymmetricEncryption();
  }
}
