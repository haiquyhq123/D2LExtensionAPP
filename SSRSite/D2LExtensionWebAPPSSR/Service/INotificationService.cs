using D2LExtensionWebAPPSSR.Data;
using D2LExtensionWebAPPSSR.Models;
using Org.BouncyCastle.Pqc.Crypto.Lms;

namespace D2LExtensionWebAPPSSR.Service
{
    public interface INotificationService
    {

        public Task CreateNotification(string userId, string title, string message, string userEmail);
        public Task DailyRemidersDueDateAssignment(string email, string content);
       
    }
}
