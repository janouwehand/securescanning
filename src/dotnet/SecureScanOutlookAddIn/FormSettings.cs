using System;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using System.Windows.Forms;
using SecureScan.Base.Interfaces;

namespace SecureScanOutlookAddIn
{
  public partial class FormSettings : Form
  {
    public FormSettings() => InitializeComponent();

    public X509Certificate2 Certificate { get; set; }

    public IBluetoothUIFunctions BluetoothUIFunctions { get; internal set; }

    protected override async void OnShown(EventArgs e)
    {
      base.OnShown(e);

      if (Certificate != null)
      {
        labelCertificaat.Text = Certificate.ToString();
      }

      await PopulateDevices();
    }

    private async Task PopulateDevices()
    {
      listBoxDevices.Items.Clear();
      var devices = await BluetoothUIFunctions.GetPairedDevicesAsync();
      listBoxDevices.Items.AddRange(devices);
    }

    private void buttonOK_Click(object sender, EventArgs e) => DialogResult = DialogResult.OK;    
  }
}
