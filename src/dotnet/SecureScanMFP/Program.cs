using System;
using System.Windows.Forms;
using Microsoft.Extensions.DependencyInjection;

namespace SecureScanMFP
{
  internal static class Program
  {
    private static IServiceProvider ServiceProvider { get; set; }

    /// <summary>
    /// The main entry point for the application.
    /// </summary>
    [STAThread]
    private static void Main()
    {
      Application.EnableVisualStyles();
      Application.SetCompatibleTextRenderingDefault(false);

      var services = new ServiceCollection();
      Services.RegisterServices(services);
      ServiceProvider = services.BuildServiceProvider();
      Application.Run(ServiceProvider.GetService<FormMFP>());
    }
  }
}
