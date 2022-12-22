using System;
using System.Security.Cryptography.X509Certificates;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using SecureScan.Base.Extensions;
using SecureScan.Base.Interfaces;
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
      //var str = BitConverter.ToString(sha1, 0, sha1.Length).Replace("-", "").ToLower();     

      var form = new BluetoothForm();
      try
      {
        form.FormClosing += (s, e) =>
        {
          cancellationTokenSource.Cancel();
        };

        form.Show();

        var task = Task.Run(() => ExecuteBluetoothAsync(sha1, certificate));
        while (!task.IsCompleted)
        {
          Application.DoEvents();
          Thread.Sleep(200);
        }
      }
      finally
      {
        form.Close();
        form.Dispose();
      }

      return (null, "niets gedaan");
    }

    private async Task ExecuteBluetoothAsync(byte[] secureContainerSHA1, X509Certificate2 certificate)
    {
      using (var gatt = new GattServer(Constants.SECURESCANSERVICE))
      {
        var smartphone = await gatt.ScanAsync(TimeSpan.FromMinutes(20), cancellationTokenSource.Token);
      }
    }

  }
}
