using System;

namespace SecureScan.Bluetooth
{
  public class GattCharacteristicDefinition
  {
    public GattCharacteristicDefinition(Guid uUID, string name, GattCharacteristicModes mode)
    {
      UUID = uUID;
      Name = name ?? throw new ArgumentNullException(nameof(name));
      Mode = mode;
    }

    public Guid UUID { get; set; }

    public string Name { get; set; }

    public GattCharacteristicModes Mode { get; set; }

    public override int GetHashCode() => UUID.GetHashCode();

    public override bool Equals(object obj)
    {
      if (obj is GattCharacteristicDefinition gattCharacteristic)
      {
        return gattCharacteristic.UUID == UUID;
      }
      else
      {
        return false;
      }
    }
  }
}
