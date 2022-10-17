using System;
using System.Windows.Forms;

namespace SecureScan.Base.WaitForm
{
  internal class WaitForm : IWaitForm
  {
    private readonly FormWaitModal formWaitModal;

    public WaitForm(FormWaitModal formWaitModal) => this.formWaitModal = formWaitModal;

    public void Hide() => formWaitModal.Hide();

    public void Show(string text, Form onTopOf = null, Action onAbort= null)
    {
      formWaitModal.SetText(text);
      formWaitModal.SetOnAbort(onAbort);
      formWaitModal.Show(onTopOf);
    }
  }
}
