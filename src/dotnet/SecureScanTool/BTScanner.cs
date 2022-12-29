using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Devices.Bluetooth;
using Windows.Devices.Enumeration;

namespace SecureScanTool
{
  internal class BTScanner
  {
    public async Task<DeviceInformation[]> FindAsync()
    {
      var filter = BluetoothLEDevice.GetDeviceSelectorFromPairingState(false);  
      var bleDeviceInfo = await DeviceInformation.FindAllAsync(filter, new string[] { "System.Devices.Aep.DeviceAddress" });
      return bleDeviceInfo.ToArray(); 
    }
  }
}
