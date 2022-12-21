using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SecureScan.Bluetooth;

namespace SecureScanTests
{
  [TestClass]
  public class UnitTest1
  {
    [TestMethod]
    public async Task TestMethod1()
    {
      var c = new CancellationTokenSource();

      using (var gatt = new GattService(Constants.SECURESCANSERVICE))
      {
        var task = gatt.ScanAsync(c.Token);

        await Task.Delay(TimeSpan.FromMinutes(20));

        c.Cancel();
      }
    }
  }
}
