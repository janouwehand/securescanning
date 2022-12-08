using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Office.Core;
using Microsoft.Office.Interop.Outlook;
using Microsoft.Office.Tools.Ribbon;

namespace SecureScanOutlookAddIn
{
  public partial class SecureScanRibbon
  {
    private void SecureScanRibbon_Load(object sender, RibbonUIEventArgs e)
    {
      
    }

    private void button1_Click(object sender, RibbonControlEventArgs e)
    {
      var control = e.Control as IRibbonControl;
      var ctx=control.Context;
      var explorer = ctx as Explorer;

      var selection = explorer.Selection;

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

      //var item = e.Control.Context as Inspector;
      //var mailItem = item.CurrentItem as MailItem;

    }
  }
}
