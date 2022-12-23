﻿using System;

namespace SecureScan.Bluetooth
{
  public static class Constants
  {
    public static readonly Guid SECURESCANSERVICE = Guid.Parse("00000999-1999-1999-8999-009999999999");

    public static readonly Guid INITREQUEST = Guid.Parse("00000999-1999-1999-7999-009999999999"); // SEND SHA1 OF SECURE CONTAINER

    public static readonly Guid PUBLICCERT = Guid.Parse("00000999-1999-1999-6999-009999999999"); // APPEND BYTES OF PUBLIC CERT

    public static readonly Guid GETKEY = Guid.Parse("00000999-1999-1999-5999-009999999999");

    public static readonly Guid GETSTATUS = Guid.Parse("00000999-1999-1999-4999-009999999999");

    public const byte STATUS_IDLE = 0xA0;
    public const byte STATUS_REQUEST_WAITFORUSER = 0xA1;
    public const byte STATUS_REQUEST_ACCEPTED = 0xA2;
    public const byte STATUS_REQUEST_DENIED = 0xA3;

  }
}
