using D2LExtensionWebAPPSSR.Data;
using D2LExtensionWebAPPSSR.Hubs;
using D2LExtensionWebAPPSSR.Models;
using Microsoft.AspNetCore.SignalR;

namespace D2LExtensionWebAPPSSR.Service
{
    public class NotificationService : INotificationService
    {
        private readonly D2LDBContext _db;
        private readonly IHubContext<NotificationHub> _hc;
        private readonly IEmailService _ms;
        // Constructor to initialize non-nullable fields
        public NotificationService(D2LDBContext db, IHubContext<NotificationHub> hc, IEmailService ms)
        {
            _db = db;
            _hc = hc;
            _ms = ms;
        }
        public async Task CreateNotificationAsync(string userId, string title, string message, string userEmail)
        {
            Notification notification = new Notification
            {
                UserId = userId,
                Title = title,
                Message = message,
                CreatedAt = DateTime.UtcNow,
                IsRead = false
            };

            _db.Notifications.Add(notification);


            await _db.SaveChangesAsync();
        }
        public async Task DailyRemidersDueDateAssignment(string email, string content)
        {
            var mailData = new MailData
            {
                EmailToId = email,
                EmailToName = "StudyPlan Remider",
                EmailSubject = "Assignment Due Date",
                EmailBody = content
            };
            _ms.SendMail(mailData);
        }
    }
}
