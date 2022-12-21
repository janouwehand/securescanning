using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;
using Windows.Devices.Bluetooth;
using Windows.Devices.Bluetooth.Advertisement;
using Windows.Devices.Bluetooth.GenericAttributeProfile;
using Windows.Storage.Streams;

namespace SecureScan.Bluetooth
{
  public class GattService : IDisposable
  {
    // Decorator for characteristic binding the user definition with the actual characteristic.
    private class CharacteristicContainer
    {
      public GattCharacteristicDefinition Definition { get; set; }
      public GattLocalCharacteristicResult Result { get; set; }
    }

    private readonly Dictionary<Guid, CharacteristicContainer> characteristicContainers = new Dictionary<Guid, CharacteristicContainer>();

    private GattServiceProvider serviceProvider;

    public Guid ServiceUUID { get; }

    public GattService(Guid serviceUUID) => ServiceUUID = serviceUUID;

    public GattService() => ServiceUUID = Constants.SECURESCANSERVICE;

    public async Task ScanAsync(System.Threading.CancellationToken token)
    {
      var localAdapter = await BluetoothAdapter.GetDefaultAsync();
      if (localAdapter.IsCentralRoleSupported)
      {
        //var filter = new BluetoothLEAdvertisementFilter();
        //filter.Advertisement = new BluetoothLEAdvertisement();
        //filter.Advertisement.ServiceUuids.Add(Constants.SecureScanApplication);

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
          var localname = e.Advertisement.LocalName;

          foreach (var sid in e.Advertisement.ServiceUuids)
          {
            var localname2 = e.Advertisement.LocalName;
          }

          if (e.Advertisement.ServiceUuids.Contains(Constants.SECURESCANSERVICE))
          {
            Console.WriteLine(string.Concat(e.Advertisement.LocalName, ", ", string.Join(" | ", e.Advertisement.ServiceUuids)));
            watcher.Stop();
            await ConnectAsync(e, token);
          }
        };
        watcher.Start();
      }
    }

    private bool inConnect = false;

    private async Task ConnectAsync(BluetoothLEAdvertisementReceivedEventArgs e, System.Threading.CancellationToken token)
    {
      if (inConnect)
      {
        return;
      }

      inConnect = true;

      var list = new List<GattCharacteristic>();

      using (var device = await BluetoothLEDevice.FromBluetoothAddressAsync(e.BluetoothAddress, e.BluetoothAddressType))
      {
        var result = await device.GetGattServicesAsync(BluetoothCacheMode.Uncached);

        if (result.Status == GattCommunicationStatus.Success)
        {
          GattDeviceService service = null;

          foreach (var _service in result.Services)
          {
            if (_service.Uuid == Constants.SECURESCANSERVICE)
            {
              service = _service;
            }
          }

          var characteristics = await service.GetCharacteristicsAsync();

          if (characteristics.Status == GattCommunicationStatus.Success)
          {
            foreach (var ch in characteristics.Characteristics)
            {

              if (ch.Uuid == Guid.Parse("00000999-1999-1999-7999-009999999999"))
              {
                var bs = Enumerable.Range(0, 255).Select(x => (byte)(x % 255)).ToArray();  //new byte[] { 0x01, 0x00 };
                var buff = bs.AsBuffer();
                await ch.WriteValueAsync(buff);
                continue;
              }


              /*var all = await ch.GetDescriptorsAsync(BluetoothCacheMode.Uncached);
              var len = all.Descriptors.Count;
              foreach (var al in all.Descriptors)
              {
                var bs = Enumerable.Range(0, 255).Select(x => (byte)(x % 255)).ToArray();  //new byte[] { 0x01, 0x00 };
                var buff = bs.AsBuffer();
                await al.WriteValueAsync(buff);
                await al.WriteValueAsync(buff);
              }*/

              var value = await ch.ReadValueAsync(BluetoothCacheMode.Uncached);
              if (value.Status == GattCommunicationStatus.Success)
              {
                var time = GetTime(value.Value);
                Debug.WriteLine(time);
                Console.WriteLine(time);
              }

              //break;
            }
          }

          while (!token.IsCancellationRequested)
          {
            await Task.Delay(100);
          }
        }
      }

      //      var localAdapter = await BluetoothAdapter.GetDefaultAsync();     

    }

    private string GetTime(IBuffer buffer)
    {
      if (buffer.Length < 10)
      {
        return "";
      }

      var dataReader = DataReader.FromBuffer(buffer);
      var bs = new byte[buffer.Length];
      dataReader.ReadBytes(bs);

      return Encoding.UTF8.GetString(bs);

      var uur = (int)bs[4];
      var minuten = (int)bs[5];
      var seconden = (int)bs[6];

      var str = $"{uur:00}:{minuten:00}:{seconden:00}";

      return str;
    }

    public void RegisterCharacteristic(GattCharacteristicDefinition definition) => characteristicContainers.Add(definition.UUID, new CharacteristicContainer { Definition = definition });

    private async Task<bool> CheckPeripheralRoleSupportAsync()
    {
      // BT_Code: New for Creator's Update - Bluetooth adapter has properties of the local BT radio.
      var localAdapter = await BluetoothAdapter.GetDefaultAsync();
      if (localAdapter != null)
      {
        return localAdapter.IsPeripheralRoleSupported;
      }
      else
      {
        // Bluetooth is not turned on 
        return false;
      }
    }

    public bool IsStarted => serviceProvider != null;

    public async Task StartAdvertisingAsync()
    {
      var supported = await CheckPeripheralRoleSupportAsync();
      if (!supported)
      {
        throw new Exception("Bluetooth not turned on or not supported!");
      }

      if (!characteristicContainers.Any())
      {
        throw new Exception("No characteristics registered!");
      }

      var serviceResult = await GattServiceProvider.CreateAsync(ServiceUUID);
      if (serviceResult.Error == BluetoothError.Success) // The error is ... there is no error!
      {
        serviceProvider = serviceResult.ServiceProvider;
      }
      else
      {
        throw new Exception($"Could not create service provider: {serviceResult.Error}");
      }

      await CreateCharacteristicsAsync();

      // BT_Code: Indicate if your sever advertises as connectable and discoverable.
      var advParameters = new GattServiceProviderAdvertisingParameters
      {
        // IsConnectable determines whether a call to publish will attempt to start advertising and 
        // put the service UUID in the ADV packet (best effort)
        IsConnectable = true,

        // IsDiscoverable determines whether a remote device can query the local device for support 
        // of this service
        IsDiscoverable = true
      };

      serviceProvider.AdvertisementStatusChanged += (s, e) =>
      {
        if (e.Error != BluetoothError.Success)
        {
          throw new Exception($"Advertisement status changed error: {e.Error}!");
        };
      };

      serviceProvider.StartAdvertising(advParameters);
    }

    public void StopAdvertising()
    {
      serviceProvider?.StopAdvertising();
      serviceProvider = null;
    }

    private async Task CreateCharacteristicsAsync()
    {
      foreach (var characteristicContainer in characteristicContainers.Values)
      {
        switch (characteristicContainer.Definition.Mode)
        {
          case GattCharacteristicModes.Read:
            await CreateReadCharacteristicAsync(characteristicContainer);
            break;

          case GattCharacteristicModes.Write:
            await CreateWriteCharacteristicAsync(characteristicContainer);
            break;

          default:
            throw new NotSupportedException();
        }

        if (characteristicContainer.Result.Error != BluetoothError.Success)
        {
          throw new Exception($"Could not create characteristic '{characteristicContainer.Definition.Name}': {characteristicContainer.Result.Error}");
        }
      }
    }

    private async Task CreateWriteCharacteristicAsync(CharacteristicContainer characteristicContainer)
    {
      characteristicContainer.Result = await serviceProvider.Service.CreateCharacteristicAsync(
        characteristicContainer.Definition.UUID,
        new GattLocalCharacteristicParameters
        {
          CharacteristicProperties = GattCharacteristicProperties.Write | GattCharacteristicProperties.WriteWithoutResponse,
          WriteProtectionLevel = GattProtectionLevel.Plain,
          UserDescription = characteristicContainer.Definition.Name
        });

      characteristicContainer.Result.Characteristic.WriteRequested += async (sender, args) =>
      {
        using (args.GetDeferral())
        {
          // Get the request information.  This requires device access before an app can access the device's request.
          var request = await args.GetRequestAsync();
          if (request == null)
          {
            throw new Exception($"No access allowed to the device while getting request for '{characteristicContainer.Definition.Name}'!");
          }

          var reader = DataReader.FromBuffer(request.Value);
          reader.ByteOrder = ByteOrder.LittleEndian;
          var bytes = new byte[request.Value.Length];
          reader.ReadBytes(bytes);

          var error = HandleWriteReceiveEvent(characteristicContainer, bytes);

          if (request.Option == GattWriteOption.WriteWithResponse)
          {
            if (error != null)
            {
              request.RespondWithProtocolError(ConvertToGattProtocolError(error.Value));
            }
            else
            {
              request.Respond();
            }
          }
        }
      };
    }

    public event EventHandler<CharacteristicReceiveValueEventArgs> ValueReceived;

    private GattErrors? HandleWriteReceiveEvent(CharacteristicContainer characteristicContainer, byte[] bytes)
    {
      if (ValueReceived == null)
      {
        return null;
      }

      var eventObject = new CharacteristicReceiveValueEventArgs(characteristicContainer.Definition, bytes);
      ValueReceived(this, eventObject);
      return eventObject.Error;
    }

    private byte ConvertToGattProtocolError(GattErrors gatError)
    {
      var pi = typeof(GattProtocolError).GetProperty(gatError.ToString()) ?? throw new Exception($"Property not found: {gatError}");
      return (byte)pi.GetValue(null);
    }

    private async Task CreateReadCharacteristicAsync(CharacteristicContainer characteristicContainer)
    {
      characteristicContainer.Result = await serviceProvider.Service.CreateCharacteristicAsync(
        characteristicContainer.Definition.UUID,
        new GattLocalCharacteristicParameters
        {
          CharacteristicProperties = GattCharacteristicProperties.Read | GattCharacteristicProperties.Notify,
          WriteProtectionLevel = GattProtectionLevel.Plain,
          UserDescription = characteristicContainer.Definition.Name
        });

      characteristicContainer.Result.Characteristic.ReadRequested += async (sender, args) =>
      {
        using (args.GetDeferral())
        {
          // Get the request information.  This requires device access before an app can access the device's request.
          var request = await args.GetRequestAsync();
          if (request == null)
          {
            throw new Exception($"No access allowed to the device while getting request for '{characteristicContainer.Definition.Name}'!");
          }

          var (error, value) = HandleHandleValueRequested(characteristicContainer);

          if (error != null)
          {
            request.RespondWithProtocolError(ConvertToGattProtocolError(error.Value));
          }
          else
          {
            var writer = new DataWriter { ByteOrder = ByteOrder.LittleEndian };
            writer.WriteBytes(value);

            // Can get details about the request such as the size and offset, as well as monitor the state to see if it has been completed/cancelled externally.
            // request.Offset
            // request.Length
            // request.State
            // request.StateChanged += <Handler>

            // Gatt code to handle the response
            request.RespondWithValue(writer.DetachBuffer());
          }
        }
      };
    }

    public event EventHandler<CharacteristicRequestValueEventArgs> ValueRequested;

    private (GattErrors? error, byte[] value) HandleHandleValueRequested(CharacteristicContainer characteristicContainer)
    {
      if (ValueRequested == null)
      {
        return (GattErrors.AttributeNotFound, null);
      }

      var eventObject = new CharacteristicRequestValueEventArgs(characteristicContainer.Definition);
      ValueRequested(this, eventObject);
      return (eventObject.Error, eventObject.Value);
    }

    public void Dispose()
    {
      if (!IsStarted)
      {
        return;
      }

      serviceProvider.StopAdvertising();
      serviceProvider = null;
    }
  }
}
