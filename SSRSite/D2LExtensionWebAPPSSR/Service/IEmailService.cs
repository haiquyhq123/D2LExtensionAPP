using D2LExtensionWebAPPSSR.Models;

namespace D2LExtensionWebAPPSSR.Service
{
    public interface IEmailService
    {
        public Task<bool> SendMail(MailData mailData);
    }
}
