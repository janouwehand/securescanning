using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SecureScanOutlookAddIn
{
  public partial class FormSettings : Form
  {
    public FormSettings()
    {
      InitializeComponent();
    }

   public X509Certificate2 Certificate { get; set; }

    protected override void OnShown(EventArgs e)
    {
      base.OnShown(e);

      if (Certificate != null)
      {
        labelCertificaat.Text = Certificate.ToString();
      }
      
    }

    private void buttonOK_Click(object sender, EventArgs e)
    {
      DialogResult = DialogResult.OK;
    }
  }
}
