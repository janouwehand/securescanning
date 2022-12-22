using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Windows.Devices.Bluetooth;
using Windows.Devices.Bluetooth.Advertisement;
using Windows.Devices.Bluetooth.GenericAttributeProfile;

namespace SecureScan.Bluetooth.Server
{
  public class GattServer : IDisposable
  {
    private bool advertisementIsFound;
    private readonly List<BluetoothLEDevice> listOfDevices = new List<BluetoothLEDevice>();

    public GattServer(Guid serviceUUID) => ServiceUUID = serviceUUID;

    public Guid ServiceUUID { get; }

    private async Task<GattConnection> OnFoundAdvertisementCreateObjectAsync(BluetoothLEAdvertisementReceivedEventArgs bluetoothLEAdvertisementReceivedEventArgs)
    {
      var device = await BluetoothLEDevice.FromBluetoothAddressAsync(bluetoothLEAdvertisementReceivedEventArgs.BluetoothAddress, bluetoothLEAdvertisementReceivedEventArgs.BluetoothAddressType);
      listOfDevices.Add(device);

      var gattServices = await device.GetGattServicesAsync(BluetoothCacheMode.Uncached);
      if (gattServices.Status != GattCommunicationStatus.Success)
      {
        throw new Exception($"Could not get GattServices ({gattServices.ProtocolError})");
      }

      var gattService = gattServices.Services.FirstOrDefault(x => x.Uuid == ServiceUUID) ?? throw new Exception("Service UUID not found!");

      var characteristics = await gattService.GetCharacteristicsAsync(BluetoothCacheMode.Uncached);
      if (characteristics.Status != GattCommunicationStatus.Success)
      {
        throw new Exception($"Could not get characteristics ({characteristics.ProtocolError})");
      }

      return new GattConnection(bluetoothLEAdvertisementReceivedEventArgs, device, gattService, characteristics.Characteristics.ToArray());
    }

    public async Task<GattConnection> ScanAsync(TimeSpan timeOut, CancellationToken cancellationToken)
    {
      advertisementIsFound = false;
      var startedOn = DateTime.Now;
      GattConnection result = null;

      var localAdapter = await BluetoothAdapter.GetDefaultAsync();
      if (localAdapter.IsCentralRoleSupported)
      {
        var watcher = new BluetoothLEAdvertisementWatcher();

        //Set the in-range threshold to -70dBm. This means advertisements with RSSI >= -70dBm 
        //will start to be considered "in-range"
        watcher.SignalStrengthFilter.InRangeThresholdInDBm = -70;

        // Set the out-of-range threshold to -75dBm (give some buffer). Used in conjunction with OutOfRangeTimeout
        // to determine when an advertisement is no longer considered "in-range"
        watcher.SignalStrengthFilter.OutOfRangeThresholdInDBm = -75;

        // Set the out-of-range timeout to be 2 seconds. Used in conjunction with OutOfRangeThresholdInDBm
        // to determine when an advertisement is no longer considered "in-range"
        watcher.SignalStrengthFilter.OutOfRangeTimeout = TimeSpan.FromMilliseconds(2000);

        watcher.AllowExtendedAdvertisements = true;
        watcher.Received += async (s, e) =>
        {
          if (e.Advertisement.ServiceUuids.Contains(Constants.SECURESCANSERVICE))
          {
            Console.WriteLine(string.Concat(e.Advertisement.LocalName, ", ", string.Join(" | ", e.Advertisement.ServiceUuids)));
            watcher.Stop();

            if (!advertisementIsFound)
            {
              advertisementIsFound = true;
              result = await OnFoundAdvertisementCreateObjectAsync(e);
            }
          }
        };
        watcher.Start();

        while (DateTime.Now - startedOn < timeOut && !cancellationToken.IsCancellationRequested && result == null)
        {
          await Task.Delay(200, cancellationToken);
        }

        return result;
      }

      return null;
    }

    public void Dispose()
    {
      foreach (var device in listOfDevices)
      {
        device.Dispose();
      }
    }
  }
}
