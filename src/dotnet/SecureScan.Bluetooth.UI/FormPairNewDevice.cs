using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using SecureScan.Bluetooth.Server;
using Windows.Devices.Bluetooth.Advertisement;
using Windows.Devices.Enumeration;

namespace SecureScan.Bluetooth.UI
{
  public partial class FormPairNewDevice : Form
  {
    private GattClient gattServer = new GattClient(Constants.SECURESCANSERVICE);
    private readonly CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();

    protected override void OnFormClosed(FormClosedEventArgs e)
    {
      gattServer?.Dispose();
      gattServer = null;
    }

    public FormPairNewDevice() => InitializeComponent();

    protected override async void OnShown(EventArgs e) => await StartScan();

    private async Task StartScan()
    {
      await gattServer.ScanAdvertisementsAsync(
        TimeSpan.FromSeconds(10),
        ad => Add(ad.address, ad.advertisement),
        cancellationTokenSource.Token);

      pictureBox1.Hide();
    }

    private void Add(ulong address, BluetoothLEAdvertisement advertisement)
    {
      if (InvokeRequired)
      {
        Invoke(new Action(() => Add(address, advertisement)));
        return;
      }

      var uc = new UcAdvertisement();
      uc.SetInfo(address, advertisement);
      flow.Controls.Add(uc);
      uc.Show();
      uc.Parent = flow;
    }
  }
}
