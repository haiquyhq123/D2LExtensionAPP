using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using D2LExtensionWebAPPSSR.Models;
using Microsoft.AspNetCore.Authorization;
using D2LExtensionWebAPPSSR.Service;

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

    public IActionResult Index(string userId)
    {
        return View();
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
