using SecureScan.NFC.PCSC.Controller;

namespace SecureScan.NFC.PCSC
{
  public static class PCSCFactory
  {
    public static PCSCController CreateController() => new PCSCController();
  }
}
