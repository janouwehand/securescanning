using System;

namespace SecureScan.Base.Logger
{
  public interface ILogger
  {
    event EventHandler<LogEventArgs> OnLog;

    void Log(string message, bool isError = false);
  }
}
