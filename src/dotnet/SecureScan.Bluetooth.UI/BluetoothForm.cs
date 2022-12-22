using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SecureScan.Bluetooth.UI
{
  public partial class BluetoothForm : Form
  {
    GattService gattService;

    public BluetoothForm()
    {
      InitializeComponent();
    }

    protected override void OnShown(EventArgs e) => Start();

    private void Start()
    {
      //gattService = new GattService();
      //gattService.StartAdvertisingAsync();
    }

    protected override void OnFormClosed(FormClosedEventArgs e)
    {
      //gattService?.Dispose();
      //gattService = null;
      base.OnFormClosed(e);
    }
  }
}
