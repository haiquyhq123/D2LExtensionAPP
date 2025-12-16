using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace D2LExtensionWebAPPSSR.Models.PomodoroFeatures
{
    public class PomodoroTask
    {
        [Key]
        public int PromodoroTaskId { get; set; }
        [Required]
        [MaxLength(200)]
        public string Title { get; set; }
        [MaxLength(1000)]
        public string Description { get; set; }
        public string Status { get; set; }
        [Required]
        public int  ExpectedPomodoroSession { get; set; }
        public int  CompletedPomodoroSession { get; set; }
        [Column(TypeName = "DateTime2")]
        public DateTime CreatedTime { get; set; }
        [Column(TypeName = "DateTime2")]
        public DateTime CompletedTime { get; set; }

        public ICollection<Session> Sessions { get; set; } = new List<Session>();

    }
}
