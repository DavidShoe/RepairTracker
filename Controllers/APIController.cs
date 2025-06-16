using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RepairTracker.DBModels;
using System.Diagnostics;

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

        // LOGIN FUNCTIONALITY REMOVED: Use ASP.NET Core Identity for authentication.

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
