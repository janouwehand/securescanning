using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecureScanTool
{
  internal class BTScanner
  {
    public async Task FindAsync()
    {
      var filter = BluetoothLEDevice.GetDeviceSelectorFromPairingState(false);
      var bleDeviceInfo = DeviceInformation.FindAllAsync(filter, new string[] { "System.Devices.Aep.ProtectionLevel" });
      await bleDeviceInfo;
    }
  }
}
