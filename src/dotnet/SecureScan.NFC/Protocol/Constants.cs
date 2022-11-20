using SecureScan.NFC.PCSC;

namespace SecureScan.NFC.Protocol
{
  internal static class Constants
  {
    public static readonly AID APPLICATIONID = AID.Parse("F4078D5A92B5B8");

    public const string APPVERSIONPREFIX = "APPV://";

    public static readonly Command CMDOWNERNAME = new Command(nameof(CMDOWNERNAME), 0x10);

    public static readonly Command CMDOWNEREMAIL = new Command(nameof(CMDOWNEREMAIL), 0x20);

    public static readonly Command CMDPUBKEY = new Command(nameof(CMDPUBKEY), 0x50, 16); // 16 * 255
  }
}
