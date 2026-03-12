using D2LExtensionWebAPPSSR.Models;
using D2LExtensionWebAPPSSR.Repositories;
using D2LExtensionWebAPPSSR.Service;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace D2LExtensionWebAPPSSR.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WeeklyReportController : ControllerBase
    {
        private readonly IPlannerDataRepository _repo;
        private readonly IAiStudyAdvisorService _aiService;
        private readonly IEmailService _emailService;
        private readonly IStaticReportGeneratorService _staticReportService;
        private readonly IConfiguration _config;
        private readonly ILogger<WeeklyReportController> _logger;

        public WeeklyReportController(
            IPlannerDataRepository repo,
            IAiStudyAdvisorService aiService,
            IEmailService emailService,
            IStaticReportGeneratorService staticReportService,
            IConfiguration config,
            ILogger<WeeklyReportController> logger)
        {
            _repo = repo;
            _aiService = aiService;
            _emailService = emailService;
            _staticReportService = staticReportService;
            _config = config;
            _logger = logger;
        }

        [HttpPost("test-send")]
        public async Task<IActionResult> TriggerReportForUser([FromBody] TestReportRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.UserId) || string.IsNullOrWhiteSpace(request.TestEmailAddress))
                return BadRequest(new { Error = "UserId and TestEmailAddress are required." });

            try
            {
                var startOfWeek = DateTime.Today;
                var endOfWeek = startOfWeek.AddDays(7);

                var assignments = await _repo.GetAssignmentsDueBetweenAsync(request.UserId, startOfWeek, endOfWeek);

                if (!assignments.Any())
                    return Ok(new { Message = "User has no assignments due this week." });

                var aiAdvice = await _aiService.GetStudyRecommendationsAsync(assignments);

                Guid reportId = Guid.NewGuid();
                var generatedFile = await _staticReportService.GenerateStaticHtmlAsync(
                    reportId, startOfWeek, endOfWeek, assignments, aiAdvice);

                var report = new ReportEntity
                {
                    Id = reportId,
                    UserId = request.UserId,
                    WeekStartDate = startOfWeek,
                    WeekEndDate = endOfWeek,
                    AiRecommendations = aiAdvice,
                    CreatedAt = DateTime.UtcNow
                };
                await _repo.SaveReportAsync(report);

                var totalTasks = assignments.Count();
                var totalHours = assignments.Sum(a => a.EstimatedHours);

                var mailData = new MailData
                {
                    EmailToName = request.TestUserName ?? "Test User",
                    EmailToId = request.TestEmailAddress,
                    EmailSubject = "Your Weekly Studying Plan Overview",
                    AttachmentPath = generatedFile.FilePath,
                    EmailBody = $@"
                    Hello {request.TestUserName ?? "Test User"},

                    Here is your quick overview for the week:
                    - Tasks Due: {totalTasks}
                    - Estimated Workload: {totalHours} hours

                    We have attached your detailed AI study plan and workload heatmap to this email. 
                    Simply download and open the attached .html file in any web browser!

                    Good luck this week,
                    The Studying Plan Team"
                };

                bool success = await _emailService.SendMail(mailData);

                if (success)
                {
                    return Ok(new
                    {
                        Message = "Static file generated, attached, and email sent successfully!",
                        FileName = generatedFile.FileName
                    });
                }

                return StatusCode(500, new { Error = "Static file generated, but email failed to send." });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error generating static report.");
                return StatusCode(500, new { Error = ex.Message });
            }
        }
    }
}