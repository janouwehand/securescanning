using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SecureScanOutlookAddIn
{
  public partial class FormCertificate : Form
  {
    public FormCertificate()
    {
      InitializeComponent();
    }

    protected override void OnShown(EventArgs e)
    {
      base.OnShown(e);

      CreateX509.CreateCertificate("jan@sodf.nl", "jano");

    }
  }
}
