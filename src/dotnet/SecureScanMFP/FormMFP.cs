using System;
using System.Configuration;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Threading;
using System.Windows.Forms;
using SecureScan.Base.Crypto;
using SecureScan.Base.Crypto.Symmetric;
using SecureScan.Base.Extensions;
using SecureScan.Base.Logger;
using SecureScan.NFC;

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
          break;

        case MFPStates.CopyingDocument:
          buttonAbort.Enabled = true;
          buttonGO.Enabled = false;
          buttonSecureScan.Enabled = false;
          break;

        case MFPStates.SecureScanInitiated:
          buttonAbort.Enabled = true;
          buttonGO.Enabled = false;
          buttonSecureScan.Enabled = false;
          break;

        case MFPStates.SecureScanWaitForGO:
          buttonAbort.Enabled = true;
          buttonGO.Enabled = true;
          buttonSecureScan.Enabled = false;
          break;

        case MFPStates.CreatingSecureContainer:
          buttonAbort.Enabled = true;
          buttonGO.Enabled = false;
          buttonSecureScan.Enabled = false;
          break;

        default:
          break;
      }

      buttonAbort.BackColor = buttonAbort.Enabled ? Color.Red : Color.DarkGray;
      buttonGO.BackColor = buttonGO.Enabled ? Color.FromArgb(0, 192, 0) : Color.DarkGray;
      buttonSecureScan.BackColor = buttonSecureScan.Enabled ? Color.Yellow : Color.DarkGray;

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
      SetState(MFPStates.Idle);
    }

    private void buttonSecureScan_Click(object sender, EventArgs e)
    {
      SetState(MFPStates.SecureScanInitiated);

      cancellationTokenSource = new CancellationTokenSource();

      Log("Please hold your smartphone to the NFC tag.");
      var task = secureScanNFC.RetrieveOwnerInfoAsync(TimeSpan.FromSeconds(10D), cancellationTokenSource.Token);
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

      //Thread.Sleep(2000);

      var randomPassword = CryptoRandom.GetBytes(32);
      var randomPasswordStr = Convert.ToBase64String(randomPassword, Base64FormattingOptions.InsertLineBreaks);

      Log($"Pseudo-random (PRNG) 32 byte symmetric key (Base64): {randomPasswordStr}", Color.DarkOliveGreen);

      var encryptedPassword = ownerInfo.X509Certificate().Value.EncryptWithPublicKey(randomPassword);
      var encryptedPasswordStr = Convert.ToBase64String(encryptedPassword, Base64FormattingOptions.InsertLineBreaks);

      Log($"Pseudo-random (PRNG) 32 byte symmetric key (Base64), encrypted with public key of x.509 (size: {encryptedPassword.Length}): {encryptedPasswordStr}", Color.DarkOliveGreen);

      var testfile = new FileInfo(ConfigurationManager.AppSettings["TESTFILE"]);

      Log($"Protecting test file '{Path.GetFileName(testfile.Name)}' using AES-GCM (size: {testfile.Length:N0} bytes)", Color.DarkOliveGreen);

      var bs = File.ReadAllBytes(testfile.FullName);
      var bsenc = symmetricEncryption.Encrypt(bs, randomPassword);
      var hash = bsenc.ComputeSHA256();

      Log($"Protected container created (size: {bsenc.Length:N0} bytes, sha256: {hash.ToHEX()})", Color.DarkOliveGreen);

      SetState(MFPStates.SecureContainerCreated);

      Log("Please hold your smartphone again to the NFC tag to receive the license.");
    }

  }
}
