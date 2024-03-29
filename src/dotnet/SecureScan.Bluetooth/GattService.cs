﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Windows.Devices.Bluetooth;
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
