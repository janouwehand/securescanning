using System;

namespace SecureScan.Bluetooth
{
  public class CharacteristicRequestValueEventArgs : EventArgs
  {
    public CharacteristicRequestValueEventArgs(GattCharacteristicDefinition gattCharacteristicDefinition) => GattCharacteristicDefinition = gattCharacteristicDefinition;

    public GattCharacteristicDefinition GattCharacteristicDefinition { get; }
    public byte[] Value { get; set; }
    public GattErrors? Error { get; set; } = null;
  }
}
