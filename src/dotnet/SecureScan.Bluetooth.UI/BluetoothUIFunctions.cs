using System;
using System.Security.Cryptography.X509Certificates;
using SecureScan.Base.Extensions;
using SecureScan.Base.Interfaces;

namespace SecureScan.Bluetooth.UI
{
  public class BluetoothUIFunctions : IBluetoothUIFunctions
  {
    public (byte[] key, string error) RetrieveKeyForSecureDocument(byte[] secureDocument, X509Certificate2 certificate)
    {
      if (secureDocument is null)
      {
        throw new ArgumentNullException(nameof(secureDocument));
      }

      var sha1 = secureDocument.ComputeSHA1();

      using (var form = new BluetoothForm())
      {
        form.ShowDialog();
      }

      return (null, "niets gedaan");
    }
  }
}
