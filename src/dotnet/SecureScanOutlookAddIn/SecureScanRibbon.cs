using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using Microsoft.Office.Interop.Outlook;
using Microsoft.Office.Tools.Ribbon;
using SecureScan.Base.Interfaces;

namespace SecureScanOutlookAddIn
{
  public partial class SecureScanRibbon
  {
    private void SecureScanRibbon_Load(object sender, RibbonUIEventArgs e)
    {
      CheckState();

      var ex = Context as Explorer;
      ex.SelectionChange += Ex_SelectionChange;
    }

    private void Ex_SelectionChange() => CheckState();

    private void CheckState()
    {
      var ctx = Context;
      var explorer = ctx as Explorer;

      var selection = explorer.Selection;

      buttonReadSecureDocument.Enabled = false;

      MailItem first = null;

      foreach (MailItem item in selection)
      {
        first = item;
        break;
      }

      if (first == null)
      {
        return;
      }

      var body = first.Body;
      var ats = first.Attachments;

      Attachment at = null;
      foreach (Attachment attachment in ats)
      {
        at = attachment;
        break;
      }

      if (at == null)
      {
        return;
      }

      var contentType = at.PropertyAccessor.GetProperty("http://schemas.microsoft.com/mapi/proptag/0x370E001E");
      buttonReadSecureDocument.Enabled = contentType == "application/ou-secure-document";
    }

    private void button1_Click(object sender, RibbonControlEventArgs e)
    {
      var uri = new Uri(Assembly.GetExecutingAssembly().CodeBase);
      var codeBaseDir = new DirectoryInfo(Path.GetDirectoryName(uri.LocalPath));
      var projDir = codeBaseDir.Parent.Parent.Parent.FullName;
      var uiDir = Path.Combine(projDir, "SecureScan.Bluetooth.UI", "bin", "debug");
      var uilib = Path.Combine(uiDir, "SecureScan.Bluetooth.UI.dll");

      var ass = Assembly.LoadFrom(uilib);
      var types = ass.GetTypes();
      var type = types.FirstOrDefault(x => x.Name == "BluetoothUIFunctions");
      var bt = (IBluetoothUIFunctions)Activator.CreateInstance(type);
      var result = bt.RetrieveKeyForSecureDocument(new byte[] { });
      MessageBox.Show(result.error);
    }

  }
}
