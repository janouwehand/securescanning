using System;
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
      using (var gatt = new GattService(Constants.SecureScanApplication))
      {
        await gatt.ScanAsync();

        await Task.Delay(TimeSpan.FromDays(1));
      }
    }
  }
}
