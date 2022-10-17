using System;
using System.Windows.Forms;

namespace SecureScan.Base.WaitForm
{
  public partial class FormWaitModal : Form
  {
    private Action onAbort;

    public FormWaitModal() => InitializeComponent();

    internal void HandleAbort() => onAbort?.Invoke();

    internal void SetOnAbort(Action onAbort) => this.onAbort = onAbort;

    internal void SetText(string text) => labelText.Text = text;

    private void buttonAbort_Click(object sender, EventArgs e) => HandleAbort();
  }
}
