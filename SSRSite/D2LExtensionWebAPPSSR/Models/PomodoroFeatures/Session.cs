using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace D2LExtensionWebAPPSSR.Models.PomodoroFeatures
{
    public class Session
    {
        [Key]
        public int SessionId { get; set; }
        [Required]
        public int Duration { get; set; }
        public bool Completed { get; set; }

        //add fk for pomodoro tasks
        [ForeignKey("PomodoroTask")]
        public int PomodoroTaskId { get; set; }
        public PomodoroTask PomodoroTask { get; set; }

    }
}
