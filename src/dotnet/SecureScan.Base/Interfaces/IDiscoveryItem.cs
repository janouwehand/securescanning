using System;

namespace SecureScan.Base.Interfaces
{
  public interface IDiscoveryItem : IDisposable
  {
    ulong Id { get; }

    string Name { get; }
  }
}
