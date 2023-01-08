using System;
using System.Windows.Forms;
using SecureScan.Base.SecureContainer;

namespace SecureScanOutlookAddIn
{
  public partial class FormDocumentInfo : Form
  {
    public FormDocumentInfo() => InitializeComponent();

    public SecureContainerModel SecureContainer { get; set; }

    public string ContentType { get; set; }

    public string FileName { get; set; }

    protected override void OnShown(EventArgs e)
    {
      labelName.Text = SecureContainer.Originator.Subject;
      labelTimestamp.Text = SecureContainer.CreatedOn;
      labelMFP.Text = SecureContainer.MfpName;
      labelSHA1.Text = SecureContainer.Originator.GetCertHashString().ToLower();
      labelContentType.Text = ContentType;
      labelSSSHA1.Text = SecureContainer.SHA1;
      labelSSFileName.Text = FileName;
    }
  }
}
