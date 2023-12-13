using System;
using System.IO;
using System.Net;
using MailKit.Net.Smtp;
using MimeKit;
using ContentDisposition = MimeKit.ContentDisposition;

namespace SecureScan.Email
{
  public class MailSender
  {
    public MailSender(string smtpHostOrIP, int smtpPort, string userName, string password)
    {
      SmtpHostOrIP = smtpHostOrIP;
      SmtpPort = smtpPort;
      UserName = userName;
      Password = password;
    }

    public string SmtpHostOrIP { get; }

    public int SmtpPort { get; }
    public string UserName { get; }
    public string Password { get; }

    public string SendMail(MailInput input)
    {
      var message = new MimeMessage();
      message.From.Add(new MailboxAddress(input.EmailFrom.Name, input.EmailFrom.Email));
      message.To.Add(new MailboxAddress(input.EmailTo.Name, input.EmailTo.Email));
      message.Subject = input.Subject;

      var body = new TextPart("plain")
      {
        Text = input.BodyPlain
      };

      var multipart = new Multipart("mixed")
      {
        body
      };

      foreach (var attachmentInput in input.Attachments)
      {
        var ms = new MemoryStream(attachmentInput.Content);
        var attachment = new MimePart(attachmentInput.ContentType)
        {
          Content = new MimeContent(ms, ContentEncoding.Default),
          ContentDisposition = new ContentDisposition(ContentDisposition.Attachment),
          ContentTransferEncoding = ContentEncoding.Base64,
          FileName = Path.GetFileName(attachmentInput.FileName)
        };
        multipart.Add(attachment);
      }

      message.Body = multipart;

      string serverResponse;

      try
      {
        using (var client = new SmtpClient())
        {
          ServicePointManager.ServerCertificateValidationCallback = (s, c, h, e) => true;
          client.Connect(SmtpHostOrIP, SmtpPort, false);
          client.Authenticate(UserName, Password);
          serverResponse = client.Send(message);
          client.Disconnect(true);
        }
      }
      catch (Exception ex)
      {
        serverResponse = "Error sending email: " + (string.IsNullOrWhiteSpace(ex.Message) ? ex.ToString() : ex.Message);
      }

      return serverResponse;
    }
  }
}
