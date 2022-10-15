using System;

namespace SecureScan.Bluetooth
{
  public class CharacteristicReceiveValueEventArgs : EventArgs
  {
    public CharacteristicReceiveValueEventArgs(GattCharacteristicDefinition gattCharacteristicDefinition, byte[] receivedValue)
    {
      GattCharacteristicDefinition = gattCharacteristicDefinition;
      ReceivedValue = receivedValue;
    }

    public GattCharacteristicDefinition GattCharacteristicDefinition { get; }
    public byte[] ReceivedValue { get; }
    public GattErrors? Error { get; set; } = null;
  }
}
