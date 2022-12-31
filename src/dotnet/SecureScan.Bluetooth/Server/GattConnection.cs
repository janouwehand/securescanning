using Windows.Devices.Bluetooth;
using Windows.Devices.Bluetooth.Advertisement;
using Windows.Devices.Bluetooth.GenericAttributeProfile;

namespace SecureScan.Bluetooth.Server
{
  public class GattConnection
  {
    public GattConnection(BluetoothLEAdvertisement advertisement, BluetoothLEDevice Device, GattDeviceService gattService, GattCharacteristic[] characteristics)
    {
      Advertisement = advertisement;
      this.Device = Device;
      GattService = gattService;
      Characteristics = characteristics;
    }

    public BluetoothLEAdvertisement Advertisement { get; }

    public BluetoothLEDevice Device { get; }

    public GattDeviceService GattService { get; }

    public GattCharacteristic[] Characteristics { get; }
  }
}
