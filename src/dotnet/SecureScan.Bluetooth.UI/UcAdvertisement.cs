using System;
using System.Windows.Forms;
using Windows.Devices.Bluetooth;
using Windows.Devices.Bluetooth.Advertisement;
using Windows.Devices.Enumeration;

namespace SecureScan.Bluetooth.UI
{
  public partial class UcAdvertisement : UserControl
  {
    BluetoothLEDevice device;
    BluetoothLEAdvertisement advertisement;
    
    public UcAdvertisement() => InitializeComponent();

    internal async void SetInfo(ulong address, BluetoothLEAdvertisement advertisement)
    {
      this.advertisement = advertisement;

      labelAddress.Text = address.ToString();
      device = await BluetoothLEDevice.FromBluetoothAddressAsync(address);

      labelName.Text = device.DeviceInformation.Name;
      labelAddress.Text = device.Name;

      if (device.DeviceInformation.Pairing.IsPaired)
      {
        buttonPair.Enabled = false;
        buttonPair.Text = "Paired already";
      }
      else if (!device.DeviceInformation.Pairing.CanPair)
      {
        buttonPair.Enabled = false;
        buttonPair.Text = "Pairing not possible";
      }
    }

    private async void buttonPair_Click(object sender, EventArgs e)
    {
      device.DeviceInformation.Pairing.Custom.PairingRequested += Custom_PairingRequested;
      var pairingResult = await device.DeviceInformation.Pairing.Custom.PairAsync(DevicePairingKinds.ConfirmOnly);

      if (pairingResult.Status == DevicePairingResultStatus.Paired)
      {
        buttonPair.Enabled = false;
        buttonPair.Text = "Paired !";

        // Remember device id
        //Properties.Settings.Default.BluetoothId = device.DeviceInformation.Id;
      }
      else
      {
        buttonPair.Enabled = false;
        buttonPair.Text = "Pairing failed";
      }      
    }

    private void Custom_PairingRequested(DeviceInformationCustomPairing sender, DevicePairingRequestedEventArgs args)
    {
      args.Accept();
    }
  }
}
