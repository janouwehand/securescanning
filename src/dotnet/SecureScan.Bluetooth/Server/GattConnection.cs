using System;
using Windows.Devices.Bluetooth;
using Windows.Devices.Bluetooth.Advertisement;
using Windows.Devices.Bluetooth.GenericAttributeProfile;

namespace SecureScan.Bluetooth.Server
{
  public class GattConnection : EventArgs
  {
    public GattConnection(BluetoothLEAdvertisementReceivedEventArgs bluetoothLEAdvertisementReceivedEventArgs, BluetoothLEDevice Device, GattDeviceService gattService, GattCharacteristic[] characteristics)
    {
      BluetoothLEAdvertisementReceivedEventArgs = bluetoothLEAdvertisementReceivedEventArgs;
      this.Device = Device;
      GattService = gattService;
      Characteristics = characteristics;
    }

    public BluetoothLEAdvertisementReceivedEventArgs BluetoothLEAdvertisementReceivedEventArgs { get; }

    public BluetoothLEDevice Device { get; }

    public GattDeviceService GattService { get; }

    public GattCharacteristic[] Characteristics { get; }
  }
}
