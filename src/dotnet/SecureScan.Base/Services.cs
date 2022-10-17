using Microsoft.Extensions.DependencyInjection;
using SecureScan.Base.WaitForm;

namespace SecureScan.Base
{
  public static class Services
  {
    public static void RegisterServices(ServiceCollection services)
    {
      services.AddSingleton<FormWaitModal>();
      services.AddSingleton<IWaitForm, WaitForm.WaitForm>();
    }
  }
}
