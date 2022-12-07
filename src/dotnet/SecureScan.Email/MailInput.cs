using System.Collections.Generic;

namespace SecureScan.Email
{
  public class MailInput
  {
    public class EmailAddress
    {
      public string Email { get; set; } 
      public string Name { get; set; }
    }

    public class Attachment
    {
      public string FileName { get; set; }
      public string ContentType { get; set; }
      public byte[] Content { get; set; }
    }

    public EmailAddress EmailFrom { get; set; }

    public EmailAddress EmailTo { get; set; }

    public string Subject { get; set; }

    public string BodyPlain { get; set; }

    public IList<Attachment> Attachments { get; } = new List<Attachment>();
  }
}
