using Microsoft.AspNetCore.Mvc;
using D2LExtensionWebAPPSSR.Repositories;
using D2LExtensionWebAPPSSR.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace D2LExtensionWebAPPSSR.Controllers
{
    public class ReportController : Controller
    {
        private readonly IPlannerDataRepository _repo;

        public ReportController(IPlannerDataRepository repo)
        {
            _repo = repo;
        }

        [HttpGet("Report/Weekly/{reportId:guid}")]
        public async Task<IActionResult> Weekly(Guid reportId)
        {
            var report = await _repo.GetReportByIdAsync(reportId);
            if (report == null) return NotFound();

            var assignments = await _repo.GetAssignmentsDueBetweenAsync(report.UserId, report.WeekStartDate, report.WeekEndDate);

            var vm = new WeeklyReportViewModel
            {
                ReportId = report.Id,
                WeekStartDate = report.WeekStartDate,
                WeekEndDate = report.WeekEndDate,
                AiRecommendations = report.AiRecommendations,
                Assignments = assignments.ToList(),
                AssignmentsPerDay = CalculateChartData(assignments)
            };

            return View(vm);
        }

        private List<int> CalculateChartData(IEnumerable<AssignmentItem> assignments)
        {
            var counts = new int[7];
            foreach (var a in assignments)
            {
                int dayIndex = (int)a.DueDate.DayOfWeek - 1;
                if (dayIndex < 0) dayIndex = 6;
                counts[dayIndex]++;
            }
            return counts.ToList();
        }
    }
}