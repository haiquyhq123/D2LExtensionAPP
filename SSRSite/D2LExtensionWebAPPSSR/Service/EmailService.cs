using D2LExtensionWebAPPSSR.Models;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;
using MailKit.Net.Smtp;
namespace D2LExtensionWebAPPSSR.Service
{
    public class EmailService : IEmailService
    {
        MailSettings mailSettings = null;
        public EmailService(IOptions<MailSettings> options)
        {
            mailSettings = options.Value;

        }
        public bool SendMail(MailData mailData)
        {
            try
            {
                MimeMessage emailMessage = new MimeMessage();
                MailboxAddress emailFrom = new MailboxAddress(mailSettings.Name, mailSettings.EmailId);
                emailMessage.From.Add(emailFrom);
                MailboxAddress emailTo = new MailboxAddress(mailData.EmailToName, mailData.EmailToId);
                emailMessage.To.Add(emailTo);
                emailMessage.Subject = mailData.EmailSubject;
                BodyBuilder emailBodayBuilder = new BodyBuilder();
                emailBodayBuilder.TextBody = mailData.EmailBody;
                emailMessage.Body = emailBodayBuilder.ToMessageBody();
                using (SmtpClient mailClient = new SmtpClient())
                {
                   
                    if (mailSettings.Port == 587)
                    {
                        mailClient.Connect(mailSettings.Host, mailSettings.Port, SecureSocketOptions.StartTls);
                    }
                    else
                    {
       
                        mailClient.Connect(mailSettings.Host, mailSettings.Port, true);
                    }

                    mailClient.Authenticate(mailSettings.EmailId, mailSettings.Password);
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
