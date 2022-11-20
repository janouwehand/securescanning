using System;
using System.Threading;
using System.Threading.Tasks;

namespace SecureScan.NFC.Protocol
{
  internal class SecureScanNFC : ISecureScanNFC
  {
    public async Task<OwnerInfo> RetrieveOwnerInfoAsync(TimeSpan waitForNFCTimeout, CancellationToken cancellationToken)
    {
      await Task.Delay(waitForNFCTimeout, cancellationToken);
      throw new TimeoutException();
    }
  }
}
