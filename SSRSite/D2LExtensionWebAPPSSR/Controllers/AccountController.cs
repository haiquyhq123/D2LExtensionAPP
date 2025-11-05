using D2LExtensionWebAPPSSR.Models;
using Microsoft.AspNetCore.Mvc;

namespace D2LExtensionWebAPPSSR.Controllers
{
    public class AccountController : Controller
    {
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Register(UserRegistrationModel userModel)
        {
            return View();
        }
    }
}
