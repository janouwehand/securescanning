using System;
using System.Windows.Forms;

namespace SecureScan.Base.WaitForm
{
  public interface IWaitForm
  {
    void Show(string text, Form onTopOf = null, Action onAbort = null);

    void Hide();
  }
}
