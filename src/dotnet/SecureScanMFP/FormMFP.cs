using System;
using System.Configuration;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using SecureScan.Base.Crypto;
using SecureScan.Base.Crypto.Symmetric;
using SecureScan.Base.Extensions;
using SecureScan.Base.Logger;
using SecureScan.Base.SecureContainer;
using SecureScan.NFC;
using SecureScanMFP.Properties;

namespace SecureScanMFP
{
  public partial class FormMFP : Form
  {
    private MFPStates state = MFPStates.Idle;
    private readonly ISecureScanNFC secureScanNFC;
    private readonly ISymmetricEncryption symmetricEncryption;
    private OwnerInfo ownerInfo;
    private CancellationTokenSource cancellationTokenSource;

    public FormMFP(ILogger logger, ISecureScanNFC secureScanNFC, ISymmetricEncryption symmetricEncryption)
    {
      InitializeComponent();
      this.secureScanNFC = secureScanNFC;
      this.symmetricEncryption = symmetricEncryption;
      UpdateUI();

      logger.OnLog += (s, e) => Log(e.Message, e.IsError, false, e.Color);
    }

    protected override void OnFormClosing(FormClosingEventArgs e)
    {
      if (state == MFPStates.SecureScanInitiated)
      {
        cancellationTokenSource.Cancel();
        e.Cancel = true;
      }
      else
      {
        cancellationTokenSource?.Dispose();
      }
    }

    private void Log(string s, Color color) => Log(s, false, false, color);

    /// <summary>
    /// Log method that allows calls from a non-UI thread to synchronize with the UI thread.
    /// </summary>
    private void Log(string s, bool isError = false, bool isStateChange = false, Color color = default)
    {
      if (IsDisposed || Disposing)
      {
        return;
      }

      if (InvokeRequired)
      {
        Invoke(new Action<string, bool, bool, Color>(Log), new object[] { s, isError, isStateChange, color });
      }
      else
      {
        if (isError)
        {
          edtlog.SelectionColor = Color.Red;
          s = string.Concat("Error: ", s);
        }
        else if (isStateChange)
        {
          edtlog.SelectionColor = Color.Green;
        }
        else if (color != default)
        {
          edtlog.SelectionColor = color;
        }

        edtlog.AppendText($"{DateTime.Now:HH:mm:ss} {s}\r\n");
        edtlog.ScrollToCaret();
        edtlog.SelectionColor = Color.Black;
      }
    }

    private void SetState(MFPStates state)
    {
      if (this.state == state)
      {
        return;
      }

      if (state != MFPStates.SecureScanWaitForGO && state != MFPStates.CreatingSecureContainer && state != MFPStates.SecureContainerCreated)
      {
        ownerInfo = null;
      }

      Log($"State changed to {state}", isStateChange: true);

      this.state = state;
      UpdateUI();
    }

    private void UpdateUI()
    {
      labelState.Text = state.ToString();

      switch (state)
      {
        case MFPStates.Idle:
          buttonAbort.Enabled = false;
          buttonGO.Enabled = true;
          buttonSecureScan.Enabled = true;
          buttonEnrollSmartphone.Enabled = true;
          break;

        case MFPStates.CopyingDocument:
          buttonAbort.Enabled = true;
          buttonGO.Enabled = false;
          buttonSecureScan.Enabled = false;
          buttonEnrollSmartphone.Enabled = false;
          break;

        case MFPStates.SecureScanInitiated:
          buttonAbort.Enabled = true;
          buttonGO.Enabled = false;
          buttonSecureScan.Enabled = false;
          buttonEnrollSmartphone.Enabled = false;
          break;

        case MFPStates.SecureScanWaitForGO:
          buttonAbort.Enabled = true;
          buttonGO.Enabled = true;
          buttonSecureScan.Enabled = false;
          buttonEnrollSmartphone.Enabled = false;
          break;

        case MFPStates.CreatingSecureContainer:
          buttonAbort.Enabled = true;
          buttonGO.Enabled = false;
          buttonSecureScan.Enabled = false;
          buttonEnrollSmartphone.Enabled = false;
          break;

        case MFPStates.EnrollShowQRWaitForNFC:
          buttonAbort.Enabled = true;
          buttonGO.Enabled = false;
          buttonSecureScan.Enabled = false;
          buttonEnrollSmartphone.Enabled = false;
          break;

        default:
          break;
      }

      buttonAbort.BackColor = buttonAbort.Enabled ? Color.Red : Color.DarkGray;
      buttonGO.BackColor = buttonGO.Enabled ? Color.FromArgb(0, 192, 0) : Color.DarkGray;
      buttonSecureScan.BackColor = buttonSecureScan.Enabled ? Color.Yellow : Color.DarkGray;
      buttonEnrollSmartphone.BackColor = buttonSecureScan.Enabled ? Color.Yellow : Color.DarkGray;

      if (state != MFPStates.EnrollShowQRWaitForNFC)
      {
        qrControl1.Visible = false;
      }

      Application.DoEvents();
    }

    private void buttonGO_Click(object sender, EventArgs e)
    {
      switch (state)
      {
        case MFPStates.Idle:
          SetState(MFPStates.CopyingDocument);
          Thread.Sleep(2000);
          SetState(MFPStates.Idle);
          break;

        case MFPStates.CopyingDocument:
          break;
        case MFPStates.SecureScanInitiated:
          break;
        case MFPStates.SecureScanWaitForGO:
          ExecuteSecureScanning();
          break;
        default:
          break;
      }
    }

    private void buttonAbort_Click(object sender, EventArgs e)
    {
      Log("Abort requested.");
      cancellationTokenSource?.Cancel();
      cancellationTokenSource?.Dispose();
      cancellationTokenSource = null;
      SetState(MFPStates.Idle);
    }

    private void buttonSecureScan_Click(object sender, EventArgs e)
    {
      SetState(MFPStates.SecureScanInitiated);

      cancellationTokenSource = new CancellationTokenSource();

      Log("Please hold your smartphone to the NFC tag.");
      var task = secureScanNFC.RetrieveOwnerInfoAsync(null, TimeSpan.FromSeconds(10D), cancellationTokenSource.Token);
      task.ResponsiveWait();

      var isTimeOut = task.IsFaulted && task.Exception.InnerExceptions.Any(x => x is TimeoutException);
      if (isTimeOut)
      {
        Log("Waiting for NFC time-out", true);
        SetState(MFPStates.Idle);
      }
      else if (task.IsFaulted)
      {
        foreach (var ex in task.Exception.InnerExceptions)
        {
          Log(ex.Message, true);
        }
        SetState(MFPStates.Idle);
      }
      else if (!task.IsCanceled)
      {
        ownerInfo = task.Result ?? throw new Exception("Owner info was expected to be present!");
        X509Received(ownerInfo);
      }
    }

    private void X509Received(OwnerInfo ownerInfo)
    {
      var cer = ownerInfo.X509Certificate().Value;
      var partsSubject = cer.GetSubjectParts();

      Log($"Issued to name  : {partsSubject.O}");
      Log($"Issued to email : {partsSubject.CN}");
      Log($"Serial          : {cer.GetSerialNumberString()}");
      Log($"Hash            : {cer.GetCertHash().ToHEX()}");
      Log($"Key algorithm   : {cer.GetKeyAlgorithm()} {cer.GetKeyAlgorithmParametersString()}");
      Log($"Effective from  : {cer.GetEffectiveDateString()}");
      Log($"Expiration      : {cer.GetExpirationDateString()}");
      Log($"Public key      : {cer.PublicKey.Key.ToXmlString(false)}");

      SetState(MFPStates.SecureScanWaitForGO);

      Log("Put physical document on scanner and press GO button");
    }

    private void ExecuteSecureScanning()
    {
      SetState(MFPStates.CreatingSecureContainer);

      Log("Busy creating protected container...");

      var randomPassword = CryptoRandom.GetBytes(32);
      var randomPasswordStr = randomPassword.ToHEX();

      Log($"Pseudo-random (PRNG) 32 byte symmetric key (hex): {randomPasswordStr}", Color.DarkOliveGreen);

      var encryptedPassword = ownerInfo.X509Certificate().Value.EncryptWithPublicKey(randomPassword);
      var encryptedPasswordStr = Convert.ToBase64String(encryptedPassword, Base64FormattingOptions.InsertLineBreaks);

      Log($"Pseudo-random (PRNG) 32 byte symmetric key (Base64), encrypted with public key of x.509 (size: {encryptedPassword.Length}): {encryptedPasswordStr}", Color.DarkOliveGreen);

      var testfile = new FileInfo(ConfigurationManager.AppSettings["TESTFILE"]);

      Log($"Protecting test file '{Path.GetFileName(testfile.Name)}' using AES-GCM (size: {testfile.Length:N0} bytes)", Color.DarkOliveGreen);

      var pdfFileName = File.Exists(labelPDF.Text) ? labelPDF.Text : testfile.FullName;
      var bsSecureContainer = CreateSecureContainer(randomPassword, pdfFileName);
      var hashSecureContainer = bsSecureContainer.ComputeSHA1();

      Log($"Protected container created (size: {bsSecureContainer.Length:N0} bytes, sha1: {hashSecureContainer.ToHEX()})", Color.DarkOliveGreen);

      ExecuteSecureContainerCreatedWaitForNFC(bsSecureContainer, hashSecureContainer, encryptedPassword);
    }

    private byte[] CreateSecureContainer(byte[] randomPassword, string pdfFileName)
    {
      // Read PDF
      var bs = File.ReadAllBytes(pdfFileName);

      // Encrypt PDF
      var bsEncryptedData = symmetricEncryption.Encrypt(bs, randomPassword);

      // Build secure container
      var secureContainer = new SecureContainerModel(ownerInfo.X509Certificate().Value, bsEncryptedData, "MFP@" + Environment.MachineName);
      var bsSecureContainer = secureContainer.Write();
      return bsSecureContainer;
    }

    private void ExecuteSecureContainerCreatedWaitForNFC(byte[] bsSecureContainer, byte[] hashSecureContainer, byte[] encryptedPassword)
    {
      if (ownerInfo == null)
      {
        // Aborted
        SetState(MFPStates.Idle);
        return;
      }

      SetState(MFPStates.SecureContainerCreated);

      Log("Please hold your smartphone again to the NFC tag to receive the license.");

      cancellationTokenSource = new CancellationTokenSource();

      var task = secureScanNFC.SendSymmetricPasswordAndHash(hashSecureContainer, encryptedPassword, ownerInfo.X509Certificate().Value, TimeSpan.FromSeconds(10D), cancellationTokenSource.Token);
      task.ResponsiveWait();

      var isTimeOut = task.IsFaulted && task.Exception.InnerExceptions.Any(x => x is TimeoutException);
      if (isTimeOut)
      {
        Log("NFC time-out. Try again.", true);
        BeepError();
        ExecuteSecureContainerCreatedWaitForNFC(bsSecureContainer, hashSecureContainer, encryptedPassword);
      }
      else if (task.IsFaulted)
      {
        foreach (var ex in task.Exception.InnerExceptions)
        {
          Log(ex.Message, true);
        }

        Log("Please try again or abort.");
        ExecuteSecureContainerCreatedWaitForNFC(bsSecureContainer, hashSecureContainer, encryptedPassword);
      }
      else
      {
        if (!task.IsCanceled)
        {
          // Success!
          var docInfo = task.Result;
          Log($"Succes! Document-Id: {docInfo.DocumentNumber}");
          SendMail(docInfo, bsSecureContainer);
          ExecuteClearCacheAndSetIdle();
        }
        else
        {
          Log("Aborted!");
          ExecuteClearCacheAndSetIdle();
        }
      }
    }

    private void SendMail(DocumentInfo docInfo, byte[] bsSecureContainer)
    {
      var subject = ownerInfo.X509Certificate().Value.GetSubjectParts();
      ownerInfo.Email = subject.CN;
      ownerInfo.Name = subject.O;

      var message = new SecureScan.Email.MailInput
      {
        EmailTo = new SecureScan.Email.MailInput.EmailAddress { Email = ownerInfo.Email, Name = ownerInfo.Name },
        EmailFrom = new SecureScan.Email.MailInput.EmailAddress { Email = Settings.Default.FromAddress, Name = Settings.Default.FromName },
        Subject = $"Your securely scanned document. Document number: {docInfo.DocumentNumber}",
        BodyPlain = $@"Dear {ownerInfo.Name},

Your securely scanned document (number: {docInfo.DocumentNumber}) is added as an attachment to this email.
The document can only be decrypted using the private key that resides on your smartphone.
Please allow bluetooth to communicatie with your smartphone in order to retrieve the symmetric key for this secure document.

Kind regards,

Secure MFP"
      };

      message.Attachments.Add(new SecureScan.Email.MailInput.Attachment
      {
        ContentType = "application/ou-secure-document",
        FileName = $"secure-document-{docInfo.DocumentNumber}.enc",
        Content = bsSecureContainer
      });

      var sender = new SecureScan.Email.MailSender(Settings.Default.SmtpHost, Settings.Default.SmtpPort, Settings.Default.SmtpUserName, File.ReadAllText(Settings.Default.SmtpPasswordFile).Trim());
      var response = sender.SendMail(message);
      Log($"Sending email result: " + response);
    }

    private void ExecuteClearCacheAndSetIdle()
    {
      BeepOK();
      SetState(MFPStates.Idle);
      Log("Cache cleared and MFP set to idle!");
    }

    private void BeepError() => Task.Run(() =>
    {
      for (var i = 1; i <= 3; i++)
      {
        Console.Beep(2000, 250);
      }
    });

    private void BeepOK() => Task.Run(() => Console.Beep(1000, 500));

    private void buttonChoosePDF_Click(object sender, EventArgs e)
    {
      using (var od = new OpenFileDialog())
      {
        od.Filter = "PDF files (*.pdf)|*.pdf";
        if (od.ShowDialog() == DialogResult.OK)
        {
          labelPDF.Text = od.FileName;
        }
      }
    }

    private void buttonEnrollSmartphone_Click(object sender, EventArgs e)
    {
      SetState(MFPStates.EnrollShowQRWaitForNFC);
      qrControl1.ShowQR(CryptoRandom.GetBytes(32));

      Log($"Symmetric key in QR-code: 0x{qrControl1.QRKey.ToHEX()}");

      cancellationTokenSource = new CancellationTokenSource();

      var x509 = new X509Certificate2("MFP.pfx", "123");

      Log("Please hold your smartphone to the NFC tag.");
      var task = secureScanNFC.StartEnrolling(qrControl1.QRKey, x509, TimeSpan.FromMinutes(30D), cancellationTokenSource.Token);
      task.ResponsiveWait();

      var isTimeOut = task.IsFaulted && task.Exception.InnerExceptions.Any(x => x is TimeoutException);
      if (isTimeOut)
      {
        Log("Waiting for NFC time-out", true);
        SetState(MFPStates.Idle);
      }
      else if (task.IsFaulted)
      {
        foreach (var ex in task.Exception.InnerExceptions)
        {
          Log(ex.Message, true);
        }
        SetState(MFPStates.Idle);
      }
      else if (!task.IsCanceled)
      {
        ownerInfo = task.Result ?? throw new Exception("Owner info was expected to be present!");
        X509Received(ownerInfo);
      }
    }

    private void buttonTestEmail_Click(object _, EventArgs e)
    {
      var message = new SecureScan.Email.MailInput
      {
        EmailTo = new SecureScan.Email.MailInput.EmailAddress { Email = Settings.Default.TestmailToAddress, Name = Settings.Default.TestmailToName },
        EmailFrom = new SecureScan.Email.MailInput.EmailAddress { Email = Settings.Default.FromAddress, Name = Settings.Default.FromName },
        Subject = $"MFP test email",
        BodyPlain = $@"Dear mam, sir,

This is a test email from the MFP sent at {DateTime.Now:HH:mm:ss}.

Kind regards,

Secure MFP"
      };

      var sender = new SecureScan.Email.MailSender(Settings.Default.SmtpHost, Settings.Default.SmtpPort, Settings.Default.SmtpUserName, File.ReadAllText(Settings.Default.SmtpPasswordFile).Trim());
      var response = sender.SendMail(message);
      Log($"Sending email result: " + response);
    }
  }
}
 