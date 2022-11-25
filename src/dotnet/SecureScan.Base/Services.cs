using Microsoft.Extensions.DependencyInjection;
using SecureScan.Base.Crypto.Symmetric;
using SecureScan.Base.Crypto.Symmetric.AESGCM;
using SecureScan.Base.Logger;
using SecureScan.Base.WaitForm;

namespace SecureScan.Base
{
  public static class Services
  {
    public static void RegisterServices(ServiceCollection services)
    {
      services.AddSingleton<ILogger, LoggerImpl>();
      services.AddSingleton<FormWaitModal>();
      services.AddSingleton<IWaitForm, WaitForm.WaitForm>();
      services.AddTransient<ISymmetricEncryption, AESGCMSymmetricEncryption>();
    }
  }
}
