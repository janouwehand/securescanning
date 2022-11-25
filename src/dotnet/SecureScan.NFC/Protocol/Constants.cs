using SecureScan.NFC.PCSC;

namespace SecureScan.NFC.Protocol
{
  internal static class Constants
  {
    public static readonly AID APPLICATIONID = AID.Parse("F4078D5A92B5B8");

    public const string APPVERSIONPREFIX = "APPV://";

    /// <summary>
    /// Retrieve public x.509 from smartphone.
    /// </summary>
    public static readonly Command CMDGETX509 = new Command(nameof(CMDGETX509), 0x50); 

    /// <summary>
    /// Make the smartphone proof that it has the private key belonging to the x.509's public key.
    /// </summary>
    public static readonly Command CMDCHALLENGE = new Command(nameof(CMDCHALLENGE), 0x60);

    /// <summary>
    /// Send initial license file to smartphone
    /// </summary>
    public static readonly Command CMDSENDLICENSE = new Command(nameof(CMDSENDLICENSE), 0x70);
  }
}
