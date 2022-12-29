using System;
using System.Diagnostics;
using System.Threading;
using System.Windows.Forms;
using SecureScan.Bluetooth;
using SecureScan.Bluetooth.Server;
using Windows.Devices.Bluetooth;
using Windows.Devices.Enumeration;

namespace SecureScanTool
{
  public partial class Form1 : Form
  {
    public Form1() => InitializeComponent();
    
    void log(string s) {
      if (InvokeRequired)
      {
        Invoke(new Action(() => log(s)));
      }
      else
      {
        richTextBox1.AppendText(s + Environment.NewLine);
        richTextBox1.ScrollToCaret();
        Application.DoEvents();
      }
    }

    CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();

    private async void buttonScan_Click(object sender, EventArgs e)
    {
      using (var gatt = new GattServer(Constants.SECURESCANSERVICE))
      {
        gatt.OnLog += (s, e2) => log(e2);
        var advs = await gatt.ScanAdvertisementsAsync(TimeSpan.FromSeconds(30), cancellationTokenSource.Token);
foreach(var a in advs)
        {

          log($@"
a.advertisement.LocalName: {a.advertisement.LocalName}
");
          
          foreach(var sec in a.advertisement.DataSections)
          {
            log($@"  DataType: {sec.DataType}, length: {sec.Data.Length}");
          }
          
          var device = await BluetoothLEDevice.FromBluetoothAddressAsync(a.address);
          device.NameChanged += Device_NameChanged;

          log($@"
device.DeviceId: {device.DeviceId}
device.Name: {device.Name}  
device.DeviceInformation.Name: {device.DeviceInformation.Name}
device.DeviceInformation.Kind: {device.DeviceInformation.Kind}
device.DeviceInformation.Id: {device.DeviceInformation.Id}
device.DeviceInformation.IsDefault: {device.DeviceInformation.IsDefault}
device.DeviceInformation.Pairing.CanPair: {device.DeviceInformation.Pairing.CanPair}
device.DeviceInformation.Pairing.IsPaired: {device.DeviceInformation.Pairing.IsPaired}
device.DeviceInformation.Pairing.ProtectionLevel: {device.DeviceInformation.Pairing.ProtectionLevel}

device.Appearance.Category: {device.Appearance.Category}
device.Appearance.SubCategory: {device.Appearance.SubCategory}
device.Appearance.RawValue: {device.Appearance.RawValue}

device.BluetoothDeviceId.Id: {device.BluetoothDeviceId.Id}
device.BluetoothDeviceId.IsClassicDevice: {device.BluetoothDeviceId.IsClassicDevice}
device.BluetoothDeviceId.IsLowEnergyDevice: {device.BluetoothDeviceId.IsLowEnergyDevice}

device.DeviceAccessInformation.CurrentStatus: {device.DeviceAccessInformation.CurrentStatus}

");
          
          foreach(var p in device.DeviceInformation.Properties)
          {
            log($@"  {p.Key}: {p.Value}");  
          }
          
          
        }
        
      }
    }

    private void Device_NameChanged(BluetoothLEDevice sender, object args)
    {
      throw new NotImplementedException();
    }

    DeviceWatcher deviceWatcher;

    private void buttonScan2_Click(object sender, EventArgs e)
    {
      try
      {
        deviceWatcher = DeviceInformation.CreateWatcher(
           BluetoothDevice.GetDeviceSelectorFromPairingState(false),
        null,
           DeviceInformationKind.Device);

        deviceWatcher.Added += (watcher, args) =>
        {
          log($"Added {args.Name}");
        };
        deviceWatcher.Updated += (watcher, args) =>
        {
          log($"Updated {args.Id}");
        };
        deviceWatcher.Removed += (watcher, args) =>
        {
          log($"Removed {args.Id}");
        };
        deviceWatcher.EnumerationCompleted += (watcher, args) =>
        {
          log("No more devices found");
        };
        deviceWatcher.Start();
      }
      catch (ArgumentException ex)
      {
        log(ex.Message);
      }
    }
  }
}
