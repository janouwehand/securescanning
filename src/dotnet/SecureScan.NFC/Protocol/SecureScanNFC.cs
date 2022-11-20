using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using SecureScan.NFC.PCSC.Controller;

namespace SecureScan.NFC.Protocol
{
  internal class SecureScanNFC : ISecureScanNFC
  {
    public async Task<OwnerInfo> RetrieveOwnerInfoAsync(TimeSpan waitForNFCTimeout, CancellationToken cancellationToken)
    {
      var controller = new PCSCController(Constants.APPLICATIONID);
      using (var connection = await controller.WaitForConnectionAsync(waitForNFCTimeout.Seconds))
      {
        await RetrieveInfoAsync(connection);
      }
      
      return null;
    }

    private async Task RetrieveInfoAsync(PCSCConnection nfc)
    {
      var applicationVersion = Encoding.UTF8.GetString(nfc.ReturnData);

    }

  }
}
