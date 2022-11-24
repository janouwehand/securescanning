namespace SecureScanMFP
{
  public enum MFPStates
  {
    Idle,
    CopyingDocument,
    SecureScanInitiated,
    SecureScanWaitForGO,
    CreatingSecureContainer,
    SecureContainerCreated
  }
}
