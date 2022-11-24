using System;
using System.Drawing;

namespace SecureScan.Base.Logger
{
  public class LogEventArgs : EventArgs
  {
    public string Message { get; set; }

    public bool IsError { get; set; }

    public Color Color { get; internal set; }
  }
}
