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

            try
            {
                if (string.IsNullOrEmpty(userName))
                {
                    return BadRequest("Username cannot be empty.");
                }

                var owner = await _context.Owners
                    .FirstOrDefaultAsync(m => m.UserName == userName);
                if (owner == null)
                {
                    return BadRequest("Username not found.");
                }

                // Trim the passwords
                var p = owner.Password?.Trim() ?? string.Empty;
                var u = userPassword.Trim();

                if (string.Compare(p, u) == 0)
                {
                    Debug.WriteLine("APIController LoginOwner: Passwords match");
                    HttpContext.Session.SetString("UserName", owner.UserName!);
                    HttpContext.Session.SetString("UserRole", "Owner");
                    Debug.WriteLine($"Owner Username {owner.UserName} logged in.");
                    var foo = HttpContext.Session.GetString("UserName");
                    Debug.WriteLine($"Session: UserName: {foo}");

                    return Json(new { redirectUrl = Url.Action(nameof(OwnersController.OwnersIndex), "Owners") });
                }
                else
                {
                    return BadRequest("Incorrect password.");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error logging in owner");
                return StatusCode(500, "Internal server error. Please try again later.");
            }
        }


        public async Task<IActionResult> LoginTech(string techName, string password)
        {
            Debug.WriteLine("APIController LoginTech");

            try
            {
                if (string.IsNullOrEmpty(techName))
                {
                    return BadRequest("Technician name cannot be empty.");
                }

                if (string.IsNullOrEmpty(password))
                {
                    return BadRequest("Password cannot be empty.");
                }

                var technician = await _context.Technicians
                    .FirstOrDefaultAsync(m => m.TechnicianName == techName);
                if (technician == null)
                {
                    return BadRequest("Technician name not found.");
                }

                // Trim the passwords
                var p = technician.Password?.Trim() ?? string.Empty;
                var u = password.Trim();

                if (string.Compare(p, u) == 0)
                {
                    HttpContext.Session.SetString("UserName", technician.TechnicianName!);
                    HttpContext.Session.SetString("UserRole", "Technician");
                    Debug.WriteLine($"Tech Username {technician.TechnicianName} logged in.");
                    var foo = HttpContext.Session.GetString("UserName");
                    Debug.WriteLine($"Session: UserName: {foo}");
                    return Json(new { redirectUrl = Url.Action(nameof(RepairsController.RepairsIndex), "Repairs") });
                }
                else
                {
                    return BadRequest("Incorrect password.");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error logging in technician");
                return StatusCode(500, "Internal server error. Please try again later.");
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
