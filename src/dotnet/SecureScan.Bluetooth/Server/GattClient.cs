﻿using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Windows.Devices.Bluetooth;
using Windows.Devices.Bluetooth.Advertisement;
using Windows.Devices.Bluetooth.GenericAttributeProfile;

namespace SecureScan.Bluetooth.Server
{
  public class GattClient : IDisposable
  {
    private bool advertisementIsFound;
    private readonly List<IDisposable> listOfDisposables = new List<IDisposable>();

    public GattClient(Guid serviceUUID) => ServiceUUID = serviceUUID;

    public Guid ServiceUUID { get; }

    public event EventHandler<string> OnLog;

    private void Log(string s) => OnLog?.Invoke(this, s);

    private async Task<GattConnection> OnFoundAdvertisementCreateObjectAsync(BluetoothLEAdvertisementReceivedEventArgs bluetoothLEAdvertisementReceivedEventArgs)
    {
      var device = await BluetoothLEDevice.FromBluetoothAddressAsync(bluetoothLEAdvertisementReceivedEventArgs.BluetoothAddress, bluetoothLEAdvertisementReceivedEventArgs.BluetoothAddressType);
      listOfDisposables.Add(device);

      var gattServices = await device.GetGattServicesAsync(BluetoothCacheMode.Uncached);
      if (gattServices.Status != GattCommunicationStatus.Success)
      {
        throw new Exception($"Could not get GattServices ({gattServices.ProtocolError})");
      }

      listOfDisposables.AddRange(gattServices.Services);

      var gattService = gattServices.Services.FirstOrDefault(x => x.Uuid == ServiceUUID) ?? throw new Exception("Service UUID not found!");

      var characteristics = await gattService.GetCharacteristicsAsync(BluetoothCacheMode.Uncached);
      if (characteristics.Status != GattCommunicationStatus.Success)
      {
        throw new Exception($"Could not get characteristics ({characteristics.ProtocolError})");
      }

      return new GattConnection(bluetoothLEAdvertisementReceivedEventArgs, device, gattService, characteristics.Characteristics.ToArray());
    }

    /// <summary>
    /// Scan advertisements of only paired devices with SecureScan service UUID
    /// </summary>
    public async Task<(ulong address, BluetoothLEAdvertisement advertisement)[]> ScanAdvertisementsAsync(TimeSpan timeOut, Action<(ulong address, BluetoothLEAdvertisement advertisement)> onAdvertisement, CancellationToken cancellationToken)
    {
      advertisementIsFound = false;
      var startedOn = DateTime.Now;
      GattConnection result = null;

      var list = new ConcurrentDictionary<ulong, BluetoothLEAdvertisement>();

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
          if (!e.IsConnectable)
          {
            return;
          }

          var device = await BluetoothLEDevice.FromBluetoothAddressAsync(e.BluetoothAddress, e.BluetoothAddressType);

          if (device == null)
          {
            return;
          }

          if (!device.DeviceInformation.Pairing.IsPaired)
          {
           // return;
          }

          if (e.Advertisement.ServiceUuids.Contains(Constants.SECURESCANSERVICE))
          {
            if (list.TryAdd(e.BluetoothAddress, e.Advertisement))
            {
              onAdvertisement((e.BluetoothAddress, e.Advertisement));
            }
          }
        };

        watcher.Start();
        Log("Advertisement watcher started.");

        while (DateTime.Now - startedOn < timeOut && !cancellationToken.IsCancellationRequested && result == null)
        {
          await Task.Delay(200, cancellationToken);
        }

        return list.Select(x=>(x.Key, x.Value)).ToArray();
      }

      Log("Error: central role not supported");
      return null;
    }

    public async Task<GattConnection> ScanAsync(TimeSpan timeOut, ulong[] ignoreIds, CancellationToken cancellationToken)
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
          // Next?
          if (ignoreIds.Contains(e.BluetoothAddress))
          {
            return;
          }

          if (e.Advertisement.ServiceUuids.Contains(Constants.SECURESCANSERVICE))
          {
            Console.WriteLine(string.Concat(e.Advertisement.LocalName, ", ", string.Join(" | ", e.Advertisement.ServiceUuids)));
            watcher.Stop();

            if (!advertisementIsFound)
            {
              advertisementIsFound = true;
              Log("Result found!");
              result = await OnFoundAdvertisementCreateObjectAsync(e);
            }
          }
        };
        watcher.Start();
        Log("Advertisement watcher started.");

        while (DateTime.Now - startedOn < timeOut && !cancellationToken.IsCancellationRequested && result == null)
        {
          await Task.Delay(200, cancellationToken);
        }

        return result;
      }

      Log("Error: central role not supported");
      return null;
    }

    public void Dispose()
    {
      foreach (var device in listOfDisposables)
      {
        device.Dispose();
      }
    }
  }
}