using D2LExtensionWebAPPSSR.Configuration;
using D2LExtensionWebAPPSSR.Models;
using D2LExtensionWebAPPSSR.Models.PomodoroFeatures;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace D2LExtensionWebAPPSSR.Data
{
    public class D2LDBContext : IdentityDbContext<User>
    {
        
        public D2LDBContext(DbContextOptions<D2LDBContext> options) : base(options)
        {
        }
        public D2LDBContext() : base()
        {
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfiguration(new RoleConfiguration());
            // seed some simple data
            modelBuilder.Entity<PomodoroTask>().HasData(
                new PomodoroTask
                {
                    PromodoroTaskId = 1,
                    Title = "Learning To Build",
                    Description = "Nah",
                    Status = "Active",
                    ExpectedPomodoroSession = 4,
                    CompletedPomodoroSession = 2,
                    CreatedTime = DateTime.Today.AddDays(0),
                    CompletedTime = DateTime.Today.AddDays(1)
                });
            modelBuilder.Entity<Session>().HasData(
                new Session
                {
                    SessionId = 1,
                    PomodoroTaskId = 1,
                    Duration = 25,
                    Completed = true,
                });

        }
        public DbSet<PomodoroTask> PomodoroTasks { get; set; }
        public DbSet<Session> Sessions { get; set; }

        public DbSet<Notification> Notifications { get; set; }

    }
}
