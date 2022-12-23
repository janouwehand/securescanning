using System;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using SecureScan.Base.Extensions;
using SecureScan.Base.Interfaces;
using SecureScan.Bluetooth.Extensions;
using SecureScan.Bluetooth.Server;

namespace SecureScan.Bluetooth.UI
{
  public class BluetoothUIFunctions : IBluetoothUIFunctions
  {
    private readonly CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();

    public (byte[] key, string error) RetrieveKeyForSecureDocument(byte[] secureDocument, X509Certificate2 certificate)
    {
      if (secureDocument is null)
      {
        throw new ArgumentNullException(nameof(secureDocument));
      }

      var sha1 = secureDocument.ComputeSHA1();
      var sha1str = BitConverter.ToString(sha1, 0, sha1.Length).Replace("-", "").ToLower();

      var form = new BluetoothForm();
      try
      {
        form.AddLog($"Secure container SHA1: {sha1str}");

        form.FormClosing += (s, e) => cancellationTokenSource.Cancel();

        form.Show();

        var task = Task.Run(() => ExecuteBluetoothAsync(sha1, certificate, str => form.AddLog(str)));
        while (!task.IsCompleted)
        {
          Application.DoEvents();
          Thread.Sleep(200);
        }

        return task.Result;
      }
      finally
      {
        form.Close();
        form.Dispose();
      }
    }

    private async Task<(byte[] key, string error)> ExecuteBluetoothAsync(byte[] secureContainerSHA1, X509Certificate2 certificate, Action<string> log)
    {
      try
      {
        using (var gatt = new GattServer(Constants.SECURESCANSERVICE))
        {
          gatt.OnLog += (s, e) => log(e);
          var gattConnection = await gatt.ScanAsync(TimeSpan.FromMinutes(2), cancellationTokenSource.Token);

          var documentAvailable = await SendSecureContainerHashGetDocumentAvailableAsync(gattConnection, secureContainerSHA1, log);
          if (!documentAvailable)
          {
            return (null, "Document not available");
          }

          await SendCertificateAsync(gattConnection, certificate, log);

          var approved = await WaitForApprovalAsync(gattConnection, log);

          if (!approved)
          {
            return (null, "Request denied by user!");
          }
          else
          {
            var key = await ReceiveKeyAsync(gattConnection, log);
            return (key, null);
          }
        }
      }
      catch (ObjectDisposedException)
      {
        return (null, "Communication aborted");
      }
      catch (Exception ex)
      {
        return (null, ex.Message);
      }
    }

    private async Task<bool> SendSecureContainerHashGetDocumentAvailableAsync(GattConnection gattConnection, byte[] secureContainerSHA1, Action<string> log)
    {
      var characteristic = gattConnection.Characteristics.FirstOrDefault(x => x.Uuid == Constants.SENDSECURECONTAINERHASH) ?? throw new Exception("Cannot find InitRequest characteristic");

      log("Sending InitRequest");

      await characteristic.WriteAsync(secureContainerSHA1);

      log("Querying whether the document is available");
      var status = await GetStatusAsync(gattConnection);

      if (status.status == Constants.STATUS_DOCUMENT_NOT_AVAILABLE)
      {
        throw new Exception("Document not available");
      }
      else if (status.status == Constants.STATUS_DOCUMENT_AVAILABLE)
      {
        log("Document available");
        return true;
      }
      else
      {
        throw new Exception("Unknown status");
      }
    }

    private async Task SendCertificateAsync(GattConnection gattConnection, X509Certificate2 certificate, Action<string> log)
    {
      log("Sending public certificate");

      var characteristic = gattConnection.Characteristics.FirstOrDefault(x => x.Uuid == Constants.PUBLICCERT) ?? throw new Exception("Cannot find PublicCert characteristic");

      var bs = certificate.GetRawCertData();
      var lists = bs.SplitList(255);
      foreach (var list in lists)
      {
        await characteristic.WriteAsync(list.ToArray());
      }

      await characteristic.WriteAsync(new byte[0]);
    }

    private async Task<byte[]> ReceiveKeyAsync(GattConnection gattConnection, Action<string> log)
    {
      log("Retrieving key");

      var characteristic = gattConnection.Characteristics.FirstOrDefault(x => x.Uuid == Constants.GETKEY) ?? throw new Exception("Cannot find Key characteristic");

      var key = await characteristic.ReadAsync();

      return key;
    }

    private async Task<(byte status, string statusDescription)> GetStatusAsync(GattConnection gattConnection)
    {
      var characteristic = gattConnection.Characteristics.FirstOrDefault(x => x.Uuid == Constants.GETSTATUS) ?? throw new Exception("Cannot find Status characteristic");

      var status = (await characteristic.ReadAsync()).FirstOrDefault();

      switch (status)
      {
        case Constants.STATUS_IDLE:
          return (status, "Idle");

        case Constants.STATUS_DOCUMENT_AVAILABLE:
          return (status, "Document available");

        case Constants.STATUS_DOCUMENT_NOT_AVAILABLE:
          return (status, "Document not available");

        case Constants.STATUS_REQUEST_WAITFORUSER:
          return (status, "Waiting for user's approval");

        case Constants.STATUS_REQUEST_ACCEPTED:
          return (status, "Access request accepted");

        case Constants.STATUS_REQUEST_DENIED:
          return (status, "Access request denied");

        default:
          throw new Exception("Unknown status!");
      }
    }

    private async Task<bool> WaitForApprovalAsync(GattConnection gattConnection, Action<string> log)
    {
      log("Waiting for user's approval (max. 30 seconds)...");

      var startedOn = DateTime.Now;
      var status = Constants.STATUS_REQUEST_WAITFORUSER;

      while (status == Constants.STATUS_REQUEST_WAITFORUSER && DateTime.Now - startedOn < TimeSpan.FromSeconds(30))
      {
        var _status = await GetStatusAsync(gattConnection);
        status = _status.status;

        if (status == Constants.STATUS_REQUEST_WAITFORUSER)
        {
          await Task.Delay(500);
        }
      }

      if (status == Constants.STATUS_REQUEST_WAITFORUSER)
      {
        throw new Exception("Timeout waiting for user's approval...");
      }
      else if (status == Constants.STATUS_REQUEST_ACCEPTED)
      {
        log("Approved!");
        return true;
      }
      else if (status == Constants.STATUS_REQUEST_DENIED)
      {
        log("Denied!");
        return false;
      }
      else
      {
        throw new Exception($"Invalid status: {status}");
      }
    }

  }
}
