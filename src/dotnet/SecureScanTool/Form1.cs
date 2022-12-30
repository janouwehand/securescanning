using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using SecureScan.Bluetooth;
using SecureScan.Bluetooth.Server;
using SecureScan.Bluetooth.UI;
using Windows.Devices.Bluetooth;
using Windows.Devices.Bluetooth.Advertisement;
using Windows.Devices.Enumeration;

namespace SecureScanTool
{
  public partial class Form1 : Form
  {
    public Form1() => InitializeComponent();

    private void log(string s)
    {
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

    private readonly CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();

    private async void buttonScan_Click(object sender, EventArgs e)
    {
      using (var gatt = new GattServer(Constants.SECURESCANSERVICE))
      {
        gatt.OnLog += (s, e2) => log(e2);
        var advs = await gatt.ScanAdvertisementsAsync(TimeSpan.FromSeconds(10), async ad => await LogAdvertisement(ad.address, ad.advertisement),
        cancellationTokenSource.Token);
      }

      log("finito");
    }

    private async Task LogAdvertisement(ulong address, BluetoothLEAdvertisement advertisement)
    {
      foreach (var sec in advertisement.DataSections)
      {
        log($@"  DataType: {sec.DataType}, length: {sec.Data.Length}");
      }

      var device = await BluetoothLEDevice.FromBluetoothAddressAsync(address);

      if (device == null)
      {
        return;
      }

      log($@"
device.DeviceInformation.Name: {device.DeviceInformation.Name}
");


      //  device.NameChanged += Device_NameChanged;

      device.DeviceInformation.Pairing.Custom.PairingRequested += (s, e) =>
      {
        switch (e.PairingKind)
        {
          case DevicePairingKinds.ConfirmOnly:
            // Windows itself will pop the confirmation dialog as part of "consent" if this is running on Desktop or Mobile
            // If this is an App for 'Windows IoT Core' or a Desktop and Console application
            // where there is no Windows Consent UX, you may want to provide your own confirmation.
            e.Accept();
            break;

          case DevicePairingKinds.ProvidePin:
            // A PIN may be shown on the target device and the user needs to enter the matching PIN on 
            // this Windows device. Get a deferral so we can perform the async request to the user.
            var collectPinDeferral = e.GetDeferral();
            var pinFromUser = "952693";
            if (!string.IsNullOrEmpty(pinFromUser))
            {
              e.Accept(pinFromUser);
            }
            collectPinDeferral.Complete();
            break;
        }
      };

      if (!device.DeviceInformation.Pairing.IsPaired)
      {
        var result = await device.DeviceInformation.Pairing.Custom.PairAsync(DevicePairingKinds.ConfirmOnly);
        log($"pairing result: {result.Status}");
      }
      return;
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

      foreach (var p in device.DeviceInformation.Properties)
      {
        log($@"  {p.Key}: {p.Value}");
      }
    }

    private void Device_NameChanged(BluetoothLEDevice sender, object args) { }

    private DeviceWatcher deviceWatcher;

    private void buttonScan2_Click(object sender, EventArgs e)
    {
      try
      {
        deviceWatcher = DeviceInformation.CreateWatcher(
           BluetoothDevice.GetDeviceSelectorFromPairingState(false),
        null,
           DeviceInformationKind.Device);

        deviceWatcher.Added += (watcher, args) => log($"Added {args.Name}");
        deviceWatcher.Updated += (watcher, args) => log($"Updated {args.Id}");
        deviceWatcher.Removed += (watcher, args) => log($"Removed {args.Id}");
        deviceWatcher.EnumerationCompleted += (watcher, args) => log("No more devices found");
        deviceWatcher.Start();
      }
      catch (ArgumentException ex)
      {
        log(ex.Message);
      }
    }

    private async void buttonFindPaired_Click(object sender, EventArgs e)
    {
      var selector = BluetoothDevice.GetDeviceSelector();
      var devices = await DeviceInformation.FindAllAsync(selector);

      foreach (var deviceInfo in devices)
      {
        var bluetoothDevice = await BluetoothLEDevice.FromIdAsync(deviceInfo.Id);
        var gattServices = await bluetoothDevice.GetGattServicesAsync();
        foreach (var gatt in gattServices.Services)
        {
          if (gatt.Uuid == Constants.SECURESCANSERVICE)
          {
            log($"found {deviceInfo.Name} {deviceInfo.Id}");
          }
        }

      }
    }

    private void button1_Click(object sender, EventArgs e)
    {
      var func = new BluetoothUIFunctions();
      func.PairNewDevice();
    }
  }
}