using System;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Devices.Bluetooth;
using Windows.Devices.Bluetooth.GenericAttributeProfile;

namespace SecureScan.Bluetooth.Extensions
{
  public static class GattCharacteristicExtensions
  {
    public static async Task<byte[]> ReadAsync(this GattCharacteristic characteristic)
    {
      var result = await characteristic.ReadValueAsync(BluetoothCacheMode.Uncached);
      if (result.Status == GattCommunicationStatus.Success)
      {
        return result.Value.ToArray();
      }
      else
      {
        throw new Exception("Cannot read attribute value for " + characteristic.Uuid);
      }
    }

    public static async Task WriteAsync(this GattCharacteristic characteristic, byte[] value)
    {
      var result = await characteristic.WriteValueAsync(value.AsBuffer());
      if (result != GattCommunicationStatus.Success)
      {
        throw new Exception("Cannot write attribute value for " + characteristic.Uuid);
      }
    }

  }
}
