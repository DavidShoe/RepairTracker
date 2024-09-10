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

        public async Task<IActionResult> LoginOwner(string userName, string userPassword)
        {
            // Lookup the username
            // If the username is found, check the password
            // If the password is correct, redirect to the OwnersIndex page
            // If the password is incorrect, return an error message
            // If the username is not found, return an error message

            try
            {
                if (userName == null)
                {
                    return NotFound();
                }

                var owner = await _context.Owners
                    .FirstOrDefaultAsync(m => m.UserName == userName);
                if (owner == null)
                {
                    return NotFound();
                }

                // trim the passwords
                var p = string.Empty;
                if (owner.Password is not null)
                {
                    p = owner.Password.Trim();
                }

                var u = userPassword.Trim();
                if (string.Compare(p, u) == 0)
                {
                    return RedirectToAction(nameof(OwnersController.OwnersIndex), "Owners");
                }
                else
                {
                    return NotFound();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error logging in owner");
                return NotFound();
            }
        }

        public async Task<IActionResult> LoginTech(string techName, string password)
        {
            // Lookup the username
            // If the username is found, check the password
            // If the password is correct, redirect to the OwnersIndex page
            // If the password is incorrect, return an error message
            // If the username is not found, return an error message

            try
            {
                if (techName == null)
                {
                    return NotFound();
                }

                var owner = await _context.Technicians
                    .FirstOrDefaultAsync(m => m.TechnicianName == techName);
                if (owner == null)
                {
                    return NotFound();
                }

                // trim the passwords
                var p = string.Empty;
                if (owner.Password is not null)
                {
                    p = owner.Password.Trim();
                }

                var u = password.Trim();
                if (string.Compare(p, u) == 0)
                {
                    return RedirectToAction(nameof(RepairsController.RepairsIndex), "Repairs");
                }
                else
                {
                    return NotFound();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error logging in owner");
                return NotFound();
            }
        }
    }
}
