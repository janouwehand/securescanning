using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SecureScan.Bluetooth;
using SecureScan.Bluetooth.Server;

namespace SecureScanTests
{
  [TestClass]
  public class UnitTest1
  {
    [TestMethod]
    public async Task TestMethod1()
    {
      var c = new CancellationTokenSource();

      using (var gatt = new GattServer(Constants.SECURESCANSERVICE))
      {
        var task = await gatt.ScanAsync(TimeSpan.FromMinutes(20), c.Token);
      }
    }
  }
}
