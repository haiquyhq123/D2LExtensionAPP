using D2LExtensionWebAPPSSR.Models;

namespace D2LExtensionWebAPPSSR.Service
{
    public interface IEmailService
    {
        bool SendMail(MailData mailData);
    }
}
