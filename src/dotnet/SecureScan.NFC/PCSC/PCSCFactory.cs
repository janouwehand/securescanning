using SecureScan.NFC.PCSC.Controller;

namespace SecureScan.NFC.PCSC
{
  public static class PCSCFactory
  {
    public static PCSCController CreateController(AID aid) => new PCSCController(aid);
  }
}
