using System;
using SecureScan.Bluetooth;

namespace SecureScanMFP.Bluetooth
{
  internal static class SecureScanGattCharacteristics
  {
    /// <summary>
    /// Each MFP that supports the Secure Scanning Protocol advertises this specific UUID.
    /// </summary>
    public static Guid SecureScanningMFPServiceUuid { get; } = Guid.Parse("bf80ab30-e076-46bc-aac8-7fefcc09976c");

    public static GattCharacteristicDefinition SmartphoneInfoCharacteristicDefinition { get; } = new GattCharacteristicDefinition(
       Guid.Parse("e879628b-5832-45dd-9432-33a9717c8176"),
       nameof(SmartphoneInfoCharacteristicDefinition),
       GattCharacteristicModes.Write);
  }
}
