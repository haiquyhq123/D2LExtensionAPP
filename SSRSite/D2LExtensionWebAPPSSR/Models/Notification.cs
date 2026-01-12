using System.ComponentModel.DataAnnotations.Schema;

namespace D2LExtensionWebAPPSSR.Models
{
    public class Notification
    {
        public int Id { get; set; }
        public string Title { get; set; }

        public string Message { get; set; }
        public DateTime CreatedAt { get; set; }

        public bool IsRead { get; set; }

        // Add Relationship
        [ForeignKey("User")]
        public string UserId { get; set; }
        public virtual User User { get; set; }
    }
}
