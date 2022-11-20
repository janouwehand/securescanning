using System;
using System.Drawing;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using SecureScan.Base.Extensions;
using SecureScan.Base.Logger;
using SecureScan.NFC;

namespace SecureScanMFP
{
  public partial class FormMFP : Form
  {
    private MFPStates state = MFPStates.Idle;
    private readonly ISecureScanNFC secureScanNFC;
    private OwnerInfo ownerInfo;
    private CancellationTokenSource cancellationTokenSource;

    public FormMFP(ILogger logger, ISecureScanNFC secureScanNFC)
    {
      InitializeComponent();
      this.secureScanNFC = secureScanNFC;
      UpdateUI();

      logger.OnLog += (s, e) => Log(e.Message, e.IsError);
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

    /// <summary>
    /// Log method that allows calls from a non-UI thread to synchronize with the UI thread.
    /// </summary>
    private void Log(string s, bool isError = false, bool isStateChange = false)
    {
      if (InvokeRequired)
      {
        Invoke(new Action<string, bool, bool>(Log), new object[] { s, isError, isStateChange });
      }
      else
      {
        if (isError)
        {
          edtlog.SelectionColor = Color.Red;
          s = string.Concat("Error: ", s);
        }
        else if (!isStateChange)
        {
          edtlog.SelectionColor = Color.Green;
        }

        edtlog.AppendText($"{DateTime.Now:HH:mm:ss} {s}\r\n");
        edtlog.ScrollToCaret();

        if (isError || !isStateChange)
        {
          edtlog.SelectionColor = Color.Black;
        }
      }
    }

    private void SetState(MFPStates state)
    {
      if (this.state == state)
      {
        return;
      }

      if (state != MFPStates.SecureScanWaitForGO)
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

      var task = secureScanNFC.RetrieveOwnerInfoAsync(TimeSpan.FromSeconds(10D), cancellationTokenSource.Token);
      Log("Please hold your smartphone to the NFC tag.");
      task.ResponsiveWait();

      var isTimeOut = task.IsFaulted && task.Exception.InnerExceptions.Any(x => x is TimeoutException);
      if (isTimeOut)
      {
        Log("Waiting for NFC time-out", true);
      }
      else if (task.IsFaulted)
      {
        foreach (var ex in task.Exception.InnerExceptions)
        {
          Log(ex.Message, true);
        }
      }
      else if (!task.IsCanceled)
      {
        //ownerInfo = task.Result ?? throw new Exception("Owner info was expected to be present!");
      }

      SetState(MFPStates.Idle);
    }
  }
}
