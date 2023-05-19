using SecureScan.NFC.PCSC.Controller;

namespace SecureScan.NFC.Protocol.Messages
{
  internal static class MessageFactory
  {
    public static class Enrolling
    {
      public static EnrollMessage1SendX509OfMFP CreateEnrollMessage1SendX509OfMFP(PCSCConnection connection) => new EnrollMessage1SendX509OfMFP(connection);

      public static EnrollMessage2RetrieveX509OfSmartphone CreateEnrollMessage2RetrieveX509OfSmartphone(PCSCConnection connection) => new EnrollMessage2RetrieveX509OfSmartphone(connection);

      public static EnrollMessage3SendBindingSignatureToMFP CreateEnrollMessage3SendBindingSignatureToMFP(PCSCConnection connection) => new EnrollMessage3SendBindingSignatureToMFP(connection);
    }
  }
}
