using System;
using System.Windows.Forms;

namespace SecureScan.Bluetooth.UI
{
  public partial class BluetoothForm : Form
  {
    public BluetoothForm() => InitializeComponent();

    public void AddLog(string str)
    {
      if (InvokeRequired)
      {
        this.Invoke(new Action(() => AddLog(str))); 
      }
      else
      {
        richTextBox1.AppendText(str + Environment.NewLine);
        richTextBox1.ScrollToCaret();
        Application.DoEvents();
      }
    }
  }
}
