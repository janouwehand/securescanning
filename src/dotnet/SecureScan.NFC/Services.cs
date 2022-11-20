using Microsoft.Extensions.DependencyInjection;
using SecureScan.NFC.Protocol;

namespace SecureScan.NFC
{
  public static class Services
  {
    public static void RegisterServices(ServiceCollection services) => services.AddTransient<ISecureScanNFC, SecureScanNFC>();
  }
}
