using D2LExtensionWebAPPSSR.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace D2LExtensionWebAPPSSR.Controllers
{
    public class Assingment : Controller
    {
        private readonly IAssignmentOperations _ao;

        public Assingment(IAssignmentOperations ao)
        {
            _ao = ao;
        }

        [Authorize]
        [HttpGet("Assingment/Index/{courseWeekId}")]
        public IActionResult Index(int courseWeekId)
        {
            ViewData["CourseWeekId"] = courseWeekId;
            List<string> res = _ao.GetAssignmentsByWeek(courseWeekId);
            return View(res);
        }

        [Authorize]
        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public IActionResult CreateAssignment(int courseWeekId, string title, string? description, DateTime dueDate, int weight, decimal estimatedHours = 1.0m)
        {
            _ao.CreateAssignment(User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)!.Value, courseWeekId, title, description, dueDate, null, weight, "NotStarted", estimatedHours);
            return RedirectToAction(nameof(Index), new { courseWeekId });
        }
        [Authorize]
        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public IActionResult DeleteAssignment(int weekId, int assignmentId)
        {
            _ao.DeleteAssignment(weekId, assignmentId);
            return RedirectToAction(nameof(Index), new { courseWeekId = weekId });
        }
    }
}
