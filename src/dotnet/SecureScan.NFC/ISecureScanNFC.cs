﻿using System;
using System.Security.Cryptography.X509Certificates;
using System.Threading;
using System.Threading.Tasks;

namespace SecureScan.NFC
{
  public interface ISecureScanNFC
  {
    Task<OwnerInfo> StartEnrolling(byte[] qrSessionKey, X509Certificate2 certificateOfMFP, TimeSpan waitForNFCTimeout, CancellationToken cancellationToken);

    Task<OwnerInfo> RetrieveOwnerInfoAsync(byte[] aesKey, TimeSpan waitForNFCTimeout, CancellationToken cancellationToken);

    Task<DocumentInfo> SendSymmetricPasswordAndHash(byte[] secureContainerSHA1, byte[] password, X509Certificate2 certificate, TimeSpan waitForNFCTimeout, CancellationToken cancellationToken);
  }
}
