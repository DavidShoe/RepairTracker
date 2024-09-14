using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RepairTracker.DBModels;
using RepairTracker.Models;
using System.Diagnostics;

namespace RepairTracker.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly GameRepairContext _context;

        public HomeController(GameRepairContext context, ILogger<HomeController> logger)
        {
            _context = context;
            _logger = logger;
        }

        public IActionResult Index()
        {
            // Get the user name from the session
            var userName = HttpContext.Session.GetString("UserName");

            // Get the unique session ID
            string sessionId = HttpContext.Session.Id;

            ViewBag.UserName = userName;
            ViewBag.SessionId = sessionId;

            // Use the session ID for logging or debugging
            Console.WriteLine($"Session ID: {sessionId}");

            return View();
        }

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
}
