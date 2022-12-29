using System;
using System.Threading;
using System.Windows.Forms;

namespace SecureScan.Bluetooth.UI
{
  public partial class BluetoothForm : Form
  {
    public BluetoothForm() => InitializeComponent();

    public CancellationTokenSource CancellationTokenSource { get; set; }

    public void AddLog(string str)
    {
      if (InvokeRequired)
      {
        Invoke(new Action(() => AddLog(str)));
      }
      else
      {
        richTextBox1.AppendText(str + Environment.NewLine);
        richTextBox1.ScrollToCaret();
        Application.DoEvents();
      }
    }

    private void buttonAbort_Click(object sender, EventArgs e)
    {
      AddLog("Aborting...");
      CancellationTokenSource?.Cancel();
      Application.DoEvents();
    }
  }
}
