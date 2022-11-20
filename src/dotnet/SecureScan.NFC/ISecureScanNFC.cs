using System;
using System.Threading;
using System.Threading.Tasks;

namespace SecureScan.NFC
{
  public interface ISecureScanNFC
  {
    Task<OwnerInfo> RetrieveOwnerInfoAsync(TimeSpan waitForNFCTimeout, CancellationToken cancellationToken);
  }
}
