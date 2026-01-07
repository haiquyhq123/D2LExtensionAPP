using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using D2LExtensionWebAPPSSR.Models;
using Microsoft.AspNetCore.Authorization;
using D2LExtensionWebAPPSSR.Service;
using System.Security.Claims;


namespace D2LExtensionWebAPPSSR.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly ICourseOperations _co;

    public HomeController(ILogger<HomeController> logger, ICourseOperations CO)
    {
        _logger = logger;
        _co = CO;
    }
    [Authorize]
    public IActionResult Index()
    {
        string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        List<string> ListOfCourse = _co.GetCoursesByUser(userId);
        return View(ListOfCourse);
    }
    [Authorize]
    [HttpGet]
    public IActionResult CreateCourse()
    {
        return View();
    }
    [Authorize]
    [HttpPost]
    [AutoValidateAntiforgeryToken]
    public IActionResult CreateCourse(string title, string coursecode, string? description = null, string? semester = null, string? professor = null)
    {
        string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        _co.CreateCourse(userId, title, coursecode, description, semester, professor);
        return RedirectToAction("Index");
    }
    [Authorize]
    [HttpGet]
    public IActionResult UpdateCourse(int Id)
    {
        string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        List<string> ListOfCourse = _co.GetCoursesByUser(userId);
        string[] courseNeedToUpdate = [];
        foreach (var item in ListOfCourse)
        {
            string[] res = item.Split("|");
            if (res[0] == Id.ToString())
            {
                courseNeedToUpdate = res;
                break;
            }
        }
        return View(courseNeedToUpdate);
    }
    [Authorize]
    [HttpPost]
    [AutoValidateAntiforgeryToken]
    public IActionResult UpdateCourse(int Id,string title,string courseCode, string? description, string? semester, string? professor)
    {
        _co.UpdateCourse(Id, title, description, semester, professor, courseCode);
        return RedirectToAction("Index");
    }
    [Authorize]
    [HttpPost]
    [AutoValidateAntiforgeryToken]
    public IActionResult DeleteCourse(int Id)
    {
        _co.DeleteCourse(Id);
        return RedirectToAction("Index");
    }
    [Authorize]
    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
