using System;
using System.Drawing;

namespace SecureScan.Base.Logger
{
  internal class LoggerImpl : ILogger
  {
    public event EventHandler<LogEventArgs> OnLog;

    public void Log(string message, bool isError = false) =>
      OnLog?.Invoke(this, new LogEventArgs { Message = message, IsError = isError });

    public void Log(string message, Color color) =>
      OnLog?.Invoke(this, new LogEventArgs { Message = message, IsError = false, Color = color });
  }
}
