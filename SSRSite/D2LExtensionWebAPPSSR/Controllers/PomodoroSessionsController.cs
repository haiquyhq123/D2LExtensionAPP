using Microsoft.AspNetCore.Mvc;

namespace D2LExtensionWebAPPSSR.Controllers
{
    public class PomodoroSessionsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
