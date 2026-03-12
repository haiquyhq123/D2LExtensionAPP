using MailKit.Net.Smtp;
using MimeKit;
using Microsoft.Extensions.Options;
using D2LExtensionWebAPPSSR.Models;
using System.IO;
using System.Threading.Tasks;
using System;

namespace D2LExtensionWebAPPSSR.Service
{
    public class EmailService : IEmailService
    {
        MailSettings mailSettings = null;
        public EmailService(IOptions<MailSettings> options)
        {
            mailSettings = options.Value;
        }

        public async Task<bool> SendMail(MailData mailData)
        {
            try
            {
                MimeMessage emailMessage = new MimeMessage();
                MailboxAddress emailFrom = new MailboxAddress(mailSettings.Name, mailSettings.EmailId);
                emailMessage.From.Add(emailFrom);
                MailboxAddress emailTo = new MailboxAddress(mailData.EmailToName, mailData.EmailToId);
                emailMessage.To.Add(emailTo);
                emailMessage.Subject = mailData.EmailSubject;

                BodyBuilder emailBodyBuilder = new BodyBuilder();
                emailBodyBuilder.TextBody = mailData.EmailBody;

                // ATTACHMENT LOGIC
                if (!string.IsNullOrEmpty(mailData.AttachmentPath) && File.Exists(mailData.AttachmentPath))
                {
                    emailBodyBuilder.Attachments.Add(mailData.AttachmentPath);
                }

                emailMessage.Body = emailBodyBuilder.ToMessageBody();

                using (MailKit.Net.Smtp.SmtpClient mailClient = new SmtpClient())
                {
                    await mailClient.ConnectAsync(mailSettings.Host, mailSettings.Port, MailKit.Security.SecureSocketOptions.SslOnConnect);
                    await mailClient.AuthenticateAsync(mailSettings.UserName, mailSettings.Password);
                    mailClient.Send(emailMessage);
                    mailClient.Disconnect(true);
                }
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in Email Service: {ex.Message}");
                return false;
            }
        }
    }
}