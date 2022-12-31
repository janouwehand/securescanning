using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecureScan.Bluetooth.Server
{
  public class GattServiceClient
  {
    private readonly GattDiscovery.Item discoveredItem;
    private readonly Action<string> log;

    public GattServiceClient(GattDiscovery.Item discoveredItem, Action<string> log)
    {
      this.discoveredItem = discoveredItem;
      this.log = log;
    }

  }
}
