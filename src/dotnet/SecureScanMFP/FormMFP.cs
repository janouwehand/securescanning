using System;
using System.Net;
using System.Text;
using System.Windows.Forms;
using SecureScan.Base.WaitForm;
using SecureScan.Bluetooth;
using SecureScan.NFC.PCSC;
using SecureScan.NFC.PCSC.Controller;
using SecureScanMFP.Bluetooth;
using Timer = System.Windows.Forms.Timer;

namespace SecureScanMFP
{
  public partial class FormMFP : Form
  {
    private GattService service;
    private ProcessStates processState = ProcessStates.Initial;
    private string smartphoneInfo;
    private readonly IWaitForm waitForm;

    public FormMFP(IWaitForm waitForm)
    {
      InitializeComponent();
      this.waitForm = waitForm;
    }

    protected override void OnFormClosed(FormClosedEventArgs e) => service?.Dispose();

    /// <summary>
    /// Log method that allows calls from a non-UI thread to synchronize with the UI thread.
    /// </summary>
    private void Log(string s)
    {
      if (InvokeRequired)
      {
        Invoke(new Action<string>(Log), new object[] { s });
      }
      else
      {
        edtlog.AppendText($"{s}\r\n");
        edtlog.ScrollToCaret();
      }
    }

    private void buttonPlaceDocument_Click(object sender, EventArgs e)
    {
      using (var od = new OpenFileDialog())
      {
        od.Filter = "PDF files|*.pdf|MS Word files|*.docx|TIF files|*.tiff|Other files|*.*";
        if (od.ShowDialog() == DialogResult.OK)
        {
          labelDocumentFileName.Text = od.FileName;
          buttonPlaceDocument.Enabled = false;
          buttonInitiateSecureScanProcess.Enabled = true;
        }
      }
    }

    private void buttonReset_Click(object sender, EventArgs e)
    {
      labelDocumentFileName.Text = string.Empty;
      buttonPlaceDocument.Enabled = true;

      labelBluetoothStatus.Text = string.Empty;
      buttonInitiateSecureScanProcess.Enabled = false;

      service = null;
      edtlog.Clear();
    }

    private Timer waitTimer;

    private async void buttonInitiateSecureScanProcess_Click(object sender, EventArgs e)
    {
      buttonInitiateSecureScanProcess.Enabled = false;
      labelBluetoothStatus.Text = "Bluetooth started advertising. Open Secure Scan App on smartphone.";

      service = new GattService(SecureScanGattCharacteristics.SecureScanningMFPServiceUuid);

      waitForm.Show("BLE Broadcasting. Please use your smartphone to continue.", this, Abort);
      service.ValueReceived += Service_ValueReceived;
      service.ValueRequested += Service_ValueRequested;
      service.RegisterCharacteristic(SecureScanGattCharacteristics.SmartphoneInfoCharacteristicDefinition);
      await service.StartAdvertisingAsync();
      Log("Broadcasting Bluetooth Low Energy advertisements. Please use your smartphone to continue.");

      processState = ProcessStates.Advertising;

      waitTimer = new Timer
      {
        Interval = 30000,
        Enabled = true
      };
      waitTimer.Tick += FormMFP_Tick;
    }

    private void Abort()
    {
      processState = ProcessStates.Initial;
      smartphoneInfo = null;

      waitTimer?.Dispose();
      waitTimer = null;
      waitForm.Hide();

      Log("Time out!");
      service?.Dispose();
      service = null;
      buttonInitiateSecureScanProcess.Enabled = true;
      labelBluetoothStatus.Text = "Time out. try again.";
    }

    private void FormMFP_Tick(object sender, EventArgs e)
    {
      if (sender is Timer timer)
      {
        timer.Enabled = false;
        timer.Dispose();
      }

      Abort();
    }

    private void Service_ValueReceived(object sender, CharacteristicReceiveValueEventArgs e)
    {
      //Handle characteriscs based on state.
      switch (processState)
      {
        case ProcessStates.Initial:
          break;
        case ProcessStates.Advertising:
          ValueReceivedOnAdvertising(e);
          return;
        case ProcessStates.SmartphoneConnected:
          break;
        case ProcessStates.SmartphoneStartCommandReceived:
          break;
        case ProcessStates.KeyExchange:
          break;
        case ProcessStates.OwnerInfoReceived:
          break;
        case ProcessStates.ProtectedContainerCreated:
          break;
        case ProcessStates.LicenseSent:
          break;
        case ProcessStates.EmailSent:
          break;
        default:
          break;
      }

      throw new ProtocolViolationException("Protocol error");

      /*var value = BitConverter.ToInt32(e.ReceivedValue, 0);
      values[e.GattCharacteristicDefinition.Name] = value;
      Calculate();
      Log($"Waarde ontvangen voor '{e.GattCharacteristicDefinition.Name}': {value}. Het resultaat is nu: {values["Result"]}");*/
    }

    private void ValueReceivedOnAdvertising(CharacteristicReceiveValueEventArgs e)
    {
      // From this state we expect that
      // - no smartphone has registered
      // - smartphone info is received.

      if (!string.IsNullOrEmpty(smartphoneInfo))
      {
        Log("Smartphone already connected!");
        e.Error = GattErrors.UnlikelyError;
        return;
      }

      if (e.GattCharacteristicDefinition != SecureScanGattCharacteristics.SmartphoneInfoCharacteristicDefinition)
      {
        Log("Unexpected value received!");
        e.Error = GattErrors.UnlikelyError;
        return;
      }

      smartphoneInfo = Encoding.UTF8.GetString(e.ReceivedValue);


    }

    private void Service_ValueRequested(object sender, CharacteristicRequestValueEventArgs e)
    {

    }

    private void button1_Click(object sender, EventArgs e)
    {
      var pcsc = PCSCFactory.CreateController(AID.Parse("F4078D5A92B5B8"));
      var result = pcsc.WaitForConnection(connection =>
      {
        if (!connection.IsConnected)
        {
          Console.WriteLine("nyet");
        }
        else
        {
          var str = Encoding.UTF8.GetString(connection.ReturnData);

          for (var i = 0; i < 10; i++)
          {
            var response = connection.Transceiver.Transceive(0x00, 0x00, 0x00, 0x00, null);
            str = Encoding.UTF8.GetString(response.Data);
            Console.WriteLine(str);
            Application.DoEvents();
          }
        }
      });

      Console.WriteLine(result);
    }

  }
}
