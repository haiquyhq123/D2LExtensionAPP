using D2LExtensionWebAPPSSR.Data;
using D2LExtensionWebAPPSSR.Models.PomodoroFeatures;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Build.Framework;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics.Eventing.Reader;

namespace D2LExtensionWebAPPSSR.Controllers
{
    public class PomodoroSessionsController : Controller
    {
        private readonly D2LDBContext db;

        public PomodoroSessionsController(D2LDBContext db)
        {
            this.db = db;
        }

        public async Task<IActionResult> Index()
        {
            var tasks = await db.PomodoroTasks.ToListAsync();
            return View(tasks);
        }

        public IActionResult CreateTask() // GET for display create form
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken] // make sure this post requirst came from my webstie, not from a fake page on another site tp prevent CSRF attacks
        public async Task<IActionResult> CreateTask(PomodoroTask pt)
        {
            if (ModelState.IsValid) // check incoming data from request valid with model rules
            {
                await db.PomodoroTasks.AddAsync(pt);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(pt);
        }

        public IActionResult UpdateTask(int id)
        {
            var task = db.PomodoroTasks.Find(id);
            if(task == null)
            {
                return NotFound();
            }
            return View(task);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateTask(PomodoroTask pt)
        {
            var task = await db.PomodoroTasks.FindAsync(pt.PromodoroTaskId);
            if (task == null)
                return NotFound();

            task.Title = pt.Title;
            task.ExpectedPomodoroSession = pt.ExpectedPomodoroSession;
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
            
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteTask(int id)
        {
            var task = await db.PomodoroTasks.FindAsync(id);
            if(task == null)
            {
                return NotFound();
            }
            db.PomodoroTasks.Remove(task);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> StartPomodoro(int id)
        {
            var pTask = await db.PomodoroTasks.FindAsync(id);
            return View(pTask);
        }
       
    }
}
