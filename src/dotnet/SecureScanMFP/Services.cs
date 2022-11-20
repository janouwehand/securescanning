using Microsoft.Extensions.DependencyInjection;

namespace SecureScanMFP
{
  internal static class Services
  {
    public static void RegisterServices(ServiceCollection services)
    {
      SecureScan.Base.Services.RegisterServices(services);
      SecureScan.NFC.Services.RegisterServices(services);

      services.AddTransient<FormMFP>();
    }
  }
}
