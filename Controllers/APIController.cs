using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RepairTracker.DBModels;
using System.Diagnostics;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace RepairTracker.Controllers
{
    //[Route("api/[controller]")]
    //[ApiController]
    public class APIController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly GameRepairContext _context;

        public APIController(GameRepairContext context, ILogger<HomeController> logger)
        {
            Debug.WriteLine("APIController constructor");
            _context = context;
            _logger = logger;
        }

        [HttpPost]
        public IActionResult Logout()
        {
            Debug.WriteLine("APIController Logout");
            HttpContext.Session.Clear();
            return RedirectToAction("Index", "Home");
        }

        public async Task<IActionResult> LoginOwner(string userName, string userPassword)
        {
            Debug.WriteLine("APIController LoginOwner");
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
                    HttpContext.Session.SetString("UserName", owner.UserName!);
                    HttpContext.Session.SetString("UserRole", "Owner");
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
            Debug.WriteLine("APIController LoginTech");
            // Lookup the username
            // If the username is found, check the password
            // If the password is correct, redirect to the OwnersIndex page
            // If the password is incorrect, return an error message
            // If the username is not found, return an error message

            try
            {
                if (techName == null)
                {
                    return BadRequest("Username or password is incorrect (1)");
                }

                var technician = await _context.Technicians
                    .FirstOrDefaultAsync(m => m.TechnicianName == techName);
                if (technician == null)
                {
                    return BadRequest("Username or password is incorrect (2)");
                }

                // trim the passwords
                var p = string.Empty;
                if (technician.Password is not null)
                {
                    p = technician.Password.Trim();
                }

                var u = password.Trim();
                if (string.Compare(p, u) == 0)
                {
                    HttpContext.Session.SetString("UserName", technician.TechnicianName!);
                    HttpContext.Session.SetString("UserRole", "Technician");
                    return RedirectToAction(nameof(RepairsController.RepairsIndex), "Repairs");
                }
                else
                {
                    return BadRequest("Username or password is incorrect (3)");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error logging in owner");
                return BadRequest("Username or password is incorrect (4)");
            }
        }




        // GET: api/<AccountController>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<AccountController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<AccountController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<AccountController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<AccountController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
