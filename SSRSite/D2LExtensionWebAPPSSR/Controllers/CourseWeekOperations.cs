using D2LExtensionWebAPPSSR.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace D2LExtensionWebAPPSSR.Controllers
{
    public class CourseWeekOperations : Controller
    {
        private readonly ICourseWeekOperations _cwo;
        public CourseWeekOperations(ICourseWeekOperations cwo)
        {
            _cwo = cwo;
        }
        [Authorize]
        [HttpGet("CourseWeekOperations/Index/{CourseId}")]
        public IActionResult Index(int CourseId, [FromQuery] string title)
        {
            ViewData["CourseTitle"] = title;
            ViewData["CourseId"] = CourseId;
            List<string> res = _cwo.GetCourseWeeksByCourse(CourseId);
            return View(res);
        }
        [Authorize]
        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public IActionResult CreateCourseWeeks(int CourseId, string Title, string? Description = null)
        {
            _cwo.CreateCourseWeek(CourseId, Title, Description);
            return RedirectToAction(nameof(CourseWeekOperations.Index), "CourseWeekOperations", new { CourseId = CourseId });
        }
        [Authorize]
        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public IActionResult DeleteCourseWeek(int WeekId, int CourseId)
        {
            _cwo.DeleteCourseWeek(WeekId);
            return RedirectToAction(nameof(CourseWeekOperations.Index), "CourseWeekOperations", new { CourseId = CourseId });
        }

    }
}
