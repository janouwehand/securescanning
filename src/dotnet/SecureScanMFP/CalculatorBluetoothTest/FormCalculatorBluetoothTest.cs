using System;
using System.Collections.Generic;
using System.Windows.Forms;
using SecureScan.Bluetooth;

namespace SecureScanMFP.CalculatorBluetoothTest
{
  public partial class FormCalculatorBluetoothTest : Form
  {
    public class Constants
    {
      public static readonly Guid CalcServiceUuid = Guid.Parse("caecface-e1d9-11e6-bf01-fe55135034f0");
      public static readonly Guid Op1CharacteristicUuid = Guid.Parse("caec2ebc-e1d9-11e6-bf01-fe55135034f1");
      public static readonly Guid Op2CharacteristicUuid = Guid.Parse("caec2ebc-e1d9-11e6-bf01-fe55135034f2");
      public static readonly Guid OperatorCharacteristicUuid = Guid.Parse("caec2ebc-e1d9-11e6-bf01-fe55135034f3");
      public static readonly Guid ResultCharacteristicUuid = Guid.Parse("caec2ebc-e1d9-11e6-bf01-fe55135034f4");
    };

    private enum CalculatorOperators
    {
      Add = 1,
      Subtract = 2,
      Multiply = 3,
      Divide = 4
    }

    public FormCalculatorBluetoothTest() => InitializeComponent();

    private GattService service;
    private readonly Dictionary<string, int> values = new Dictionary<string, int>();

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

    private async void PublishButton_Click(object sender, EventArgs e)
    {
      service = new GattService(Constants.CalcServiceUuid);
      service.ValueReceived += Service_ValueReceived;
      service.ValueRequested += Service_ValueRequested;
      service.RegisterCharacteristic(new GattCharacteristicDefinition(Constants.Op1CharacteristicUuid, "Operand 1", GattCharacteristicModes.Write));
      service.RegisterCharacteristic(new GattCharacteristicDefinition(Constants.Op2CharacteristicUuid, "Operand 2", GattCharacteristicModes.Write));
      service.RegisterCharacteristic(new GattCharacteristicDefinition(Constants.OperatorCharacteristicUuid, "Operator", GattCharacteristicModes.Write));
      service.RegisterCharacteristic(new GattCharacteristicDefinition(Constants.ResultCharacteristicUuid, "Result", GattCharacteristicModes.Read));
      await service.StartAsync();
      Log("Started");
    }

    private void Service_ValueReceived(object sender, CharacteristicReceiveValueEventArgs e)
    {
      var value = BitConverter.ToInt32(e.ReceivedValue, 0);
      values[e.GattCharacteristicDefinition.Name] = value;
      Calculate();
      Log($"Waarde ontvangen voor '{e.GattCharacteristicDefinition.Name}': {value}. Het resultaat is nu: {values["Result"]}");
    }

    private void Calculate()
    {
      if (!values.TryGetValue("Operand 1", out var operand1))
      {
        operand1 = 0;
      }

      if (!values.TryGetValue("Operand 2", out var operand2))
      {
        operand2 = 0;
      }

      if (!values.TryGetValue("Operator", out var op))
      {
        op = 1;
      }

      var @operator = (CalculatorOperators)op;
      int result;
      switch (@operator)
      {
        case CalculatorOperators.Add:
          result = operand1 + operand2;
          break;
        case CalculatorOperators.Subtract:
          result = operand1 - operand2;
          break;
        case CalculatorOperators.Multiply:
          result = operand1 * operand2;
          break;
        case CalculatorOperators.Divide:
          result = operand1 / operand2;
          break;
        default:
          throw new NotSupportedException();
      }

      values["Result"] = result;
    }

    private void Service_ValueRequested(object sender, CharacteristicRequestValueEventArgs e)
    {
      if (e.GattCharacteristicDefinition.Name == "Result")
      {
        e.Value = BitConverter.GetBytes(values["Result"]);
        Log($"Waarde versturen voor '{e.GattCharacteristicDefinition.Name}'. Het resultaat is nu: {values["Result"]}");
      }
      else
      {
        e.Error = GattErrors.AttributeNotFound;
      }
    }

  }
}
