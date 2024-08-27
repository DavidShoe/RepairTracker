using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using RepairTracker.DBModels;

namespace RepairTracker.Controllers
{
    public class RepairsViewModel
    {
        private readonly GameRepairContext _context;
        public RepairsViewModel(GameRepairContext context)
        {
            _context = context;
        }
        public List<Repair>? ActiveRepairs { get; set; }
        public List<Repair>? Backlog { get; set; }
        public List<Repair>? History { get; set; }
    }
    public class RepairsController : Controller
    {
        private readonly GameRepairContext _context;

        public RepairsController(GameRepairContext context)
        {
            _context = context;
        }

        // GET: Repairs
        public IActionResult RepairsIndex()
        {
            var repairsViewModel = new RepairsViewModel(_context);
            // Include related data for active repairs
            var repairs = _context.Repairs
                .Include(r => r.Game)
                .Include(r => r.Owner)
                .Include(r => r.Technician)
                .ToList();

            var activeRepairs = repairs.Where(r => (r.FinishedDate == null && r.StartDate != null)).ToList();
            var backlog = repairs.Where(r => r.StartDate == null).ToList();
            var history = repairs.Where(r => r.FinishedDate != null).ToList();

            repairsViewModel.ActiveRepairs = activeRepairs;
            repairsViewModel.Backlog = backlog;
            repairsViewModel.History = history;
            return View(repairsViewModel);
            //return View(await gameRepairContext.ToListAsync());
        }

        [HttpGet]
        public async Task<IActionResult> WorkOnRepair(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var repair = await _context.Repairs
                .FirstOrDefaultAsync(m => m.RepairId == id);
            if (repair == null)
            {
                return NotFound();
            }

            ViewData["GameId"] = new SelectList(_context.Games, "GameId", "GameName", repair.GameId);

            return View(repair);
        }

        // GET: Repairs/Start/5
        public async Task<IActionResult> Start(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var repair = await _context.Repairs
                .FirstOrDefaultAsync(m => m.RepairId == id);
            if (repair == null)
            {
                return NotFound();
            }

            repair.StartDate = DateTime.Now;
            _context.Update(repair);

            return View(nameof(WorkOnRepair));
        }


        // GET: Repairs/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var repair = await _context.Repairs
                .Include(r => r.Game)
                .Include(r => r.Owner)
                .Include(r => r.Technician)
                .FirstOrDefaultAsync(m => m.RepairId == id);
            if (repair == null)
            {
                return NotFound();
            }

            return View(repair);
        }

        [HttpPost]
        public IActionResult AddGame(string gameName)
        {
            Debug.WriteLine("AddGame called with gameName: " + gameName);

            if (string.IsNullOrEmpty(gameName)) 
            {
                return BadRequest("Game name is required.");
            }

            // Check to see if the game already exists
            var game = _context.Games.FirstOrDefault(g => g.GameName == gameName);
            if (game != null)
            {
                return BadRequest("Game already exists.");
            }

            var newGame = new Game { GameName = gameName };
            _context.Games.Add(newGame);
            _context.SaveChanges();

            return Json(new { gameId = newGame.GameId, gameName = newGame.GameName });
        }

        [HttpPost]
        public IActionResult AddOwner(string ownerName)
        {
            Debug.WriteLine("AddOwner called with OwnerName: " + ownerName);

            if (string.IsNullOrEmpty(ownerName))
            {
                return BadRequest("Owner name is required.");
            }

            // Check to see if the Owner already exists
            var Owner = _context.Owners.FirstOrDefault(g => g.OwnerName == ownerName);
            if (Owner != null)
            {
                return BadRequest("Owner already exists.");
            }

            var newOwner = new Owner { OwnerName = ownerName };
            _context.Owners.Add(newOwner);
            _context.SaveChanges();

            return Json(new { OwnerId = newOwner.OwnerId, OwnerName = newOwner.OwnerName });
        }



        // GET: Repairs/Create
        public IActionResult Create()
        {
            ViewData["GameNames"] = new SelectList(_context.Games, "GameId", "GameName");
            ViewData["OwnerNames"] = new SelectList(_context.Owners, "OwnerId", "OwnerName");
            ViewData["TechnicianNames"] = new SelectList(_context.Technicians, "TechnicianId", "TechnicianName");
            return View();
        }

        // POST: Repairs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("RepairId,GameId,TechnicianId,FinishedDate,ReceivedDate,OwnerId,StartDate,CustomerStates")] Repair repair)
        {
            Debug.WriteLine("Create called with repair: " + repair);
            if (ModelState.IsValid)
            {
                _context.Add(repair);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(RepairsIndex));
            }
            ViewData["GameId"] = new SelectList(_context.Games, "GameId", "GameId", repair.GameId);
            ViewData["OwnerId"] = new SelectList(_context.Owners, "OwnerId", "OwnerId", repair.OwnerId);
            ViewData["TechnicianId"] = new SelectList(_context.Technicians, "TechnicianId", "TechnicianId", repair.TechnicianId);
            return View(repair);
        }

        // GET: Repairs/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var repair = await _context.Repairs.FindAsync(id);
            if (repair == null)
            {
                return NotFound();
            }
            ViewData["GameId"] = new SelectList(_context.Games, "GameId", "GameId", repair.GameId);
            ViewData["OwnerId"] = new SelectList(_context.Owners, "OwnerId", "OwnerId", repair.OwnerId);
            ViewData["TechnicianId"] = new SelectList(_context.Technicians, "TechnicianId", "TechnicianId", repair.TechnicianId);
            return View(repair);
        }

        // POST: Repairs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("RepairId,GameId,TechnicianId,FinishedDate,ReceivedDate,OwnerId,StartDate")] Repair repair)
        {
            if (id != repair.RepairId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(repair);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RepairExists(repair.RepairId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(RepairsIndex));
            }
            ViewData["GameId"] = new SelectList(_context.Games, "GameId", "GameId", repair.GameId);
            ViewData["OwnerId"] = new SelectList(_context.Owners, "OwnerId", "OwnerId", repair.OwnerId);
            ViewData["TechnicianId"] = new SelectList(_context.Technicians, "TechnicianId", "TechnicianId", repair.TechnicianId);
            return View(repair);
        }

        // GET: Repairs/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var repair = await _context.Repairs
                .Include(r => r.Game)
                .Include(r => r.Owner)
                .Include(r => r.Technician)
                .FirstOrDefaultAsync(m => m.RepairId == id);
            if (repair == null)
            {
                return NotFound();
            }

            return View(repair);
        }

        // POST: Repairs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var repair = await _context.Repairs.FindAsync(id);
            if (repair != null)
            {
                _context.Repairs.Remove(repair);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(RepairsIndex));
        }

        private bool RepairExists(int id)
        {
            return _context.Repairs.Any(e => e.RepairId == id);
        }
    }
}
