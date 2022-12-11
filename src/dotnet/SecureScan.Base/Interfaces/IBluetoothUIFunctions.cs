namespace SecureScan.Base.Interfaces
{
  public interface IBluetoothUIFunctions
  {
    (byte[] key, string error) RetrieveKeyForSecureDocument(byte[] secureDocument);
  }
}
