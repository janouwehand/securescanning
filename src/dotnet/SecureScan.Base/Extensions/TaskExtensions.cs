using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SecureScan.Base.Extensions
{
  public static class TaskExtensions
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
