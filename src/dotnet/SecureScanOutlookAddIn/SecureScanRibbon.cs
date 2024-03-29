﻿using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography.X509Certificates;
using System.Windows.Forms;
using Microsoft.Office.Interop.Outlook;
using Microsoft.Office.Tools.Ribbon;
using SecureScan.Base.Crypto.Symmetric.AESGCM;
using SecureScan.Base.Extensions;
using SecureScan.Base.Interfaces;
using SecureScan.Base.SecureContainer;

namespace SecureScanOutlookAddIn
{
  public partial class SecureScanRibbon
  {
    private void SecureScanRibbon_Load(object sender, RibbonUIEventArgs e)
    {
      LoadInterface();
      CheckState();

      var ex = Context as Explorer;
      ex.SelectionChange += Ex_SelectionChange;
    }

    private void Ex_SelectionChange() => CheckState();

    private MailItem GetMailItem()
    {
      var ctx = Context;
      var explorer = ctx as Explorer;

      var selection = explorer.Selection;

      MailItem first = null;

      foreach (var item in selection)
      {
        if (item is MailItem mailItem)
        {
          first = mailItem;
          break;
        }
      }

      return first;
    }

    private Attachment GetAttachment(MailItem mailItem)
    {
      var attachments = mailItem.Attachments;

      Attachment first = null;

      foreach (Attachment attachment in attachments)
      {
        first = attachment;
        break;
      }

      return first;
    }

    private void CheckState()
    {
      var ctx = Context;
      var explorer = ctx as Explorer;

      var selection = explorer.Selection;

      buttonReadSecureDocument.Enabled = false;
      buttonDocumentInfo.Enabled = false;

      MailItem first = null;

      foreach (var item in selection)
      {
        if (item is MailItem mailItem)
        {
          first = mailItem;
          break;
        }
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
      buttonDocumentInfo.Enabled = buttonReadSecureDocument.Enabled;
    }

    private IBluetoothUIFunctions bluetoothUIFunctions;

    private void LoadInterface()
    {
      var uri = new Uri(Assembly.GetExecutingAssembly().CodeBase);
      var codeBaseDir = new DirectoryInfo(Path.GetDirectoryName(uri.LocalPath));
      var projDir = codeBaseDir.Parent.Parent.Parent.FullName;
      var uiDir = Path.Combine(projDir, "SecureScan.Bluetooth.UI", "bin", "debug");
      var uilib = Path.Combine(uiDir, "SecureScan.Bluetooth.UI.dll");

      var ass = Assembly.LoadFrom(uilib);
      var types = ass.GetTypes();
      var type = types.FirstOrDefault(x => x.Name == "BluetoothUIFunctions");
      bluetoothUIFunctions = (IBluetoothUIFunctions)Activator.CreateInstance(type);
    }

    private const string PR_ATTACH_DATA_BIN = "http://schemas.microsoft.com/mapi/proptag/0x37010102";

    private void button1_Click(object sender, RibbonControlEventArgs e)
    {
      var mailItem = GetMailItem();
      if (mailItem == null)
      {
        return;
      }

      var attachment = GetAttachment(mailItem);
      if (attachment == null)
      {
        return;
      }

      var certificate = GetCertificate();

      var contentType = attachment.PropertyAccessor.GetProperty("http://schemas.microsoft.com/mapi/proptag/0x370E001E");
      var isOK = contentType == "application/ou-secure-document";

      var attachmentData = attachment.PropertyAccessor.GetProperty(PR_ATTACH_DATA_BIN);
      byte[] bsAttachment = attachmentData;

      var result = bluetoothUIFunctions.RetrieveKeyForSecureDocument(bsAttachment, certificate);

      if (!string.IsNullOrEmpty(result.error) || result.key == null || !result.key.Any())
      {
        MessageBox.Show(result.error ?? "no key received");
      }
      else
      {
        var secureContainer = SecureContainerModel.Read(bsAttachment);

        var symmetricPassword = certificate.DecryptWithPrivateKey(result.key);
        var symmetricEncryption = new AESGCMSymmetricEncryption();
        var decryptedDocument = symmetricEncryption.Decrypt(secureContainer.EncryptedData, symmetricPassword);
        symmetricPassword = null;

        using (var formRenderPDF = new FormRenderPDF())
        {
          formRenderPDF.DocumentStream = new MemoryStream(decryptedDocument);
          decryptedDocument = null;
          formRenderPDF.ShowDialog();
        }
      }
    }

    private X509Certificate2 GetCertificate()
    {
      var mailItem = GetMailItem();
      if (mailItem == null)
      {
        return null;
      }

      var account = mailItem.SendUsingAccount;
      if (account == null)
      {
        return null;
      }

      if (account.CurrentUser == null)
      {
        return null;
      }

      var name = account.CurrentUser.Name;
      var email = account.SmtpAddress;
      var pc = Environment.MachineName;

      var cert = X509Manager.RetrieveOrCreateCertificate(email, name, pc);
      return cert;
    }

    private void groupSecureScan_DialogLauncherClick(object sender, RibbonControlEventArgs e)
    {
      using (var formSettings = new FormSettings())
      {
        formSettings.BluetoothUIFunctions = bluetoothUIFunctions;
        formSettings.Certificate = GetCertificate();
        formSettings.ShowDialog();
      }
    }

    private void buttonDocumentInfo_Click(object sender, RibbonControlEventArgs e)
    {
      var mailItem = GetMailItem();
      if (mailItem == null)
      {
        return;
      }

      var attachment = GetAttachment(mailItem);
      if (attachment == null)
      {
        return;
      }

      var certificate = GetCertificate();

      var contentType = attachment.PropertyAccessor.GetProperty("http://schemas.microsoft.com/mapi/proptag/0x370E001E");
      var isOK = contentType == "application/ou-secure-document";

      var attachmentData = attachment.PropertyAccessor.GetProperty(PR_ATTACH_DATA_BIN);
      byte[] bsAttachment = attachmentData;

      var secureContainer = SecureContainerModel.Read(bsAttachment);

      using (var form = new FormDocumentInfo())
      {
        form.SecureContainer = secureContainer;
        form.ContentType = contentType;
        form.FileName = attachment.FileName;
        form.ShowDialog();
      }
    }
  }
}
