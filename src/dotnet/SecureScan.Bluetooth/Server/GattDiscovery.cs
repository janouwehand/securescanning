using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using SecureScan.Base.Interfaces;
using Windows.Devices.Bluetooth;
using Windows.Devices.Bluetooth.Advertisement;

namespace SecureScan.Bluetooth.Server
{
  /// <summary>
  /// Find a Bluetooth LE devices that advertises a specific service.
  /// </summary>
  public class GattDiscovery
  {
    private readonly Action<string> log;

    public class Item : IDiscoveryItem
    {
      public BluetoothLEAdvertisement Advertisement { get; set; }

      public BluetoothLEDevice Device { get; set; }

      public bool IsSecureScanCapable { get; set; }

      public bool IsPaired => Device.DeviceInformation.Pairing.IsPaired;

      public ulong Id => Device.BluetoothAddress;

      public string Name => Device?.Name ?? Device?.DeviceInformation?.Name;
    }

    public Guid ServiceUUID { get; }

    public GattDiscovery(Guid serviceUUID, Action<string> log)
    {
      ServiceUUID = serviceUUID;
      this.log = log;
    }

    public async Task<Item[]> DiscoverDevicesAsync(TimeSpan? timeOut = null, CancellationToken cancellationToken = default)
    {
      timeOut = timeOut ?? TimeSpan.FromSeconds(4);

      var dict = new ConcurrentDictionary<ulong, Item>();

      var localAdapter = await BluetoothAdapter.GetDefaultAsync();
      if (!localAdapter.IsCentralRoleSupported)
      {
        log("Error: central role not supported");
        return new Item[0];
      }

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
        // Already found the device
        if (dict.ContainsKey(e.BluetoothAddress))
        {
          return;
        }

        if (!e.IsConnectable)
        {
          return;
        }

        var item = new Item { Advertisement = e.Advertisement };

        if (!dict.TryAdd(e.BluetoothAddress, item))
        {
          return;
        }

        var device = await BluetoothLEDevice.FromBluetoothAddressAsync(e.BluetoothAddress, e.BluetoothAddressType);
        if (device == null)
        {
          return; // out of range?
        }

        item.Device = device;

        if (e.Advertisement.ServiceUuids.Contains(Constants.SECURESCANSERVICE))
        {
          item.IsSecureScanCapable = true;

          if (!device.DeviceInformation.Pairing.IsPaired)
          {
            log($"Unpaired device found ({e.BluetoothAddress:X}). Ignoring ...");
          }
          else
          {
            log($"Paired device {item.Name} found ({e.BluetoothAddress:X}) in range and active!");
          }
        }
      };

      var startedOn = DateTime.Now;
      watcher.Start();
      log("Advertisement watcher started.");

      while (DateTime.Now - startedOn < timeOut && !cancellationToken.IsCancellationRequested)
      {
        await Task.Delay(200, cancellationToken);
      }

      log("Advertisement watcher finished.");

      var result = dict.Values.Where(x => x.IsSecureScanCapable && x.IsPaired).ToArray();
      return result;
    }
  }
}
