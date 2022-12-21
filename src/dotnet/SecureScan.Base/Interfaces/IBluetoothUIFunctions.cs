using System.Security.Cryptography.X509Certificates;

namespace SecureScan.Base.Interfaces
{
  public interface IBluetoothUIFunctions
  {
    (byte[] key, string error) RetrieveKeyForSecureDocument(byte[] secureDocument, X509Certificate2 certificate);
  }
}
