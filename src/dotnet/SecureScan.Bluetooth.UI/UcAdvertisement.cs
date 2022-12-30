using System;
using System.Windows.Forms;
using Windows.Devices.Bluetooth;
using Windows.Devices.Bluetooth.Advertisement;

namespace SecureScan.Bluetooth.UI
{
  public partial class UcAdvertisement : UserControl
  {
    public UcAdvertisement() => InitializeComponent();

    internal async void SetInfo(ulong address, BluetoothLEAdvertisement advertisement)
    {
      labelAddress.Text = address.ToString();
      var device = await BluetoothLEDevice.FromBluetoothAddressAsync(address);

      labelName.Text = device.DeviceInformation.Name;
      labelAddress.Text = device.Name;

      if (device.DeviceInformation.Pairing.IsPaired)
      {
        buttonPair.Enabled = false;
        buttonPair.Text = "Paired already";
      }
    }

    private void buttonPair_Click(object sender, EventArgs e)
    {

    }
  }
}
