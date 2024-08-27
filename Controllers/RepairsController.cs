using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using GameRepairApp.Models;
using RepairTracker.Data;

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
//            var gameRepairContext = _context.Repairs.Include(r => r.Game).Include(r => r.Technician);

            var repairs = new RepairsViewModel(_context);

            var activeRepairs = _context.Repairs.Where(r => r.FinishedDate == null).ToList();
            var backlog = _context.Repairs.Where(r => r.StartDate == null).ToList();
            var history = _context.Repairs.Where(r => r.FinishedDate != null).ToList();

            repairs.ActiveRepairs = activeRepairs;
            repairs.Backlog = backlog;
            repairs.History = history;

            return View(repairs);

            //return View(await gameRepairContext.ToListAsync());
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
                .Include(r => r.Technician)
                .FirstOrDefaultAsync(m => m.RepairID == id);
            if (repair == null)
            {
                return NotFound();
            }

            return View(repair);
        }

        // GET: Repairs/Create
        public IActionResult Create()
        {
            ViewData["GameID"] = new SelectList(_context.Games, "GameID", "GameID");
            ViewData["TechnicianID"] = new SelectList(_context.Technicians, "TechnicianID", "TechnicianID");
            return View();
        }

        // POST: Repairs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("RepairID,GameID,TechnicianID,ReceivedDate,FinishedDate,StartDate")] Repair repair)
        {
            if (ModelState.IsValid)
            {
                _context.Add(repair);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(RepairsIndex));
            }
            ViewData["GameID"] = new SelectList(_context.Games, "GameID", "GameID", repair.GameID);
            ViewData["TechnicianID"] = new SelectList(_context.Technicians, "TechnicianID", "TechnicianID", repair.TechnicianID);
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
            ViewData["GameID"] = new SelectList(_context.Games, "GameID", "GameID", repair.GameID);
            ViewData["TechnicianID"] = new SelectList(_context.Technicians, "TechnicianID", "TechnicianID", repair.TechnicianID);
            return View(repair);
        }

        // POST: Repairs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("RepairID,GameID,TechnicianID,ReceivedDate,FinishedDate,StartDate")] Repair repair)
        {
            if (id != repair.RepairID)
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
                    if (!RepairExists(repair.RepairID))
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
            ViewData["GameID"] = new SelectList(_context.Games, "GameID", "GameID", repair.GameID);
            ViewData["TechnicianID"] = new SelectList(_context.Technicians, "TechnicianID", "TechnicianID", repair.TechnicianID);
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
                .Include(r => r.Technician)
                .FirstOrDefaultAsync(m => m.RepairID == id);
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
            return _context.Repairs.Any(e => e.RepairID == id);
        }
    }
}
