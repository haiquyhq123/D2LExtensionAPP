using D2LExtensionWebAPPSSR.Models;
using D2LExtensionWebAPPSSR.Repositories;
using Microsoft.Extensions.Configuration;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace D2LExtensionWebAPPSSR.Service
{
    public class WeeklyReportJob
    {
        private readonly IPlannerDataRepository _repo;
        private readonly IAiStudyAdvisorService _aiService;
        private readonly IEmailService _emailService;
        private readonly IStaticReportGeneratorService _staticReportService;
        private readonly IConfiguration _config;

        public WeeklyReportJob(
            IPlannerDataRepository repo,
            IAiStudyAdvisorService aiService,
            IEmailService emailService,
            IStaticReportGeneratorService staticReportService,
            IConfiguration config)
        {
            _repo = repo;
            _aiService = aiService;
            _emailService = emailService;
            _staticReportService = staticReportService;
            _config = config;
        }

        public async Task GenerateAndEmailWeeklyReportsAsync()
        {
            var startOfWeek = DateTime.Today;
            var endOfWeek = startOfWeek.AddDays(7);

            var users = await _repo.GetAllUsersAsync();

            foreach (var user in users)
            {
                var assignments = await _repo.GetAssignmentsDueBetweenAsync(user.Id, startOfWeek, endOfWeek);
                if (!assignments.Any()) continue;

                var aiAdvice = await _aiService.GetStudyRecommendationsAsync(assignments);
                Guid reportId = Guid.NewGuid();

                var generatedFile = await _staticReportService.GenerateStaticHtmlAsync(
                    reportId, startOfWeek, endOfWeek, assignments, aiAdvice);

                var report = new ReportEntity
                {
                    Id = reportId,
                    UserId = user.Id,
                    WeekStartDate = startOfWeek,
                    WeekEndDate = endOfWeek,
                    AiRecommendations = aiAdvice,
                    CreatedAt = DateTime.UtcNow
                };
                await _repo.SaveReportAsync(report);

                var mailData = new MailData
                {
                    EmailToName = user.UserName,
                    EmailToId = user.Email,
                    EmailSubject = "Your Weekly Studying Plan Overview",
                    AttachmentPath = generatedFile.FilePath,
                    EmailBody = $@"
                    Hello {user.UserName},

                    Here is your quick overview for the week:
                    - Tasks Due: {assignments.Count()}
                    - Estimated Workload: {assignments.Sum(a => a.EstimatedHours)} hours

                    We have attached your detailed AI study plan and workload heatmap to this email. 
                    Simply download and open the attached .html file in any web browser!

                    Good luck this week,
                    The Studying Plan Team"
                };

                await _emailService.SendMail(mailData);
            }
        }
    }
}