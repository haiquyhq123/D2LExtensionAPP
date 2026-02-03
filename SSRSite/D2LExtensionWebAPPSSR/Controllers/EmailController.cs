using D2LExtensionWebAPPSSR.Models;
using D2LExtensionWebAPPSSR.Service;
using MailKit;
using Microsoft.AspNetCore.Mvc;

namespace D2LExtensionWebAPPSSR.Controllers
{
    // For testing with postman
    public class EmailController : ControllerBase
    {
        private readonly IEmailService _ms;
        public EmailController(IEmailService ms)
        {
            _ms = ms;
        }
        [HttpPost]
        public bool SendMail([FromBody]MailData mailData)
        {
            return _ms.SendMail(mailData);
        }
    }
}
