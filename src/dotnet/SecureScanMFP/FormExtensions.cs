using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SecureScanMFP
{
  public static class FormExtensions
  {
    public static void ResponsiveWait(this Task task)
    {
      while (!task.IsCompleted)
      {
        Application.DoEvents();
        Thread.Sleep(100);
      }
    }
  }
}
