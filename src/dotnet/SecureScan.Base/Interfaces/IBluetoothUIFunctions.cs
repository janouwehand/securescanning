using System;
using System.Security.Cryptography.X509Certificates;
using System.Threading;
using System.Threading.Tasks;

namespace SecureScan.Base.Interfaces
{
  public interface IBluetoothUIFunctions
  {
    (byte[] key, string error) RetrieveKeyForSecureDocument(byte[] secureDocument, X509Certificate2 certificate);

    Task<PairedDevice[]> GetPairedDevicesAsync();

    Task<IDiscoveryItem[]> DiscoverDevicesAsync(Action<string> log, CancellationToken cancellationToken = default);
  }
}
