﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using RepairTracker.DBModels;
using RepairTracker.Models;

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

        private async Task<Repair?> LoadRepair(int repairId)
        {
            var repair = await _context.Repairs
            .Include(r => r.Game)
            .Include(r => r.Owner)
            .Include(r => r.Technician)
            .Include(Game => Game.RepairNotes)
            .Include(Game => Game.RepairParts)
            .ThenInclude(rp => rp.Part)
            .FirstOrDefaultAsync(m => m.RepairId == repairId);

            return repair;
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
        public async Task<IActionResult> WorkOnRepair(int id)
        {
            var repair = await LoadRepair(id);
            if (repair == null)
            {
                return NotFound();
            }

            ViewData["Parts"] = await _context.Parts.ToListAsync();

            return View(repair);
        }

        // GET: Repairs/Start/5
        public async Task<IActionResult> Start(int id)
        {
            var repair = await LoadRepair(id);
            if (repair == null)
            {
                return NotFound();
            }

            // Update the start date to now
            repair.StartDate = DateTime.Now;
            // Update the context
            _context.Update(repair);
            // save it to the back end
            await _context.SaveChangesAsync();

            // And open the work on repair page
            return View(nameof(WorkOnRepair), repair);
        }


        // GET: Repairs/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var repair = await LoadRepair(id);
            if (repair == null)
            {
                return NotFound();
            }

            ViewData["IsReview"] = true;

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
            ViewData["GameId"] = new SelectList(_context.Games, "GameId", "GameName", repair.GameId);
            ViewData["OwnerId"] = new SelectList(_context.Owners, "OwnerId", "OwnerName", repair.OwnerId);
            ViewData["TechnicianId"] = new SelectList(_context.Technicians, "TechnicianId", "TechnicianName", repair.TechnicianId);
            return View(repair);
        }

        // POST: Repairs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("RepairId,GameId,TechnicianId,FinishedDate,ReceivedDate,OwnerId,StartDate,CustomerStates")] Repair repair)
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

        private bool RepairExists(int id)
        {
            return _context.Repairs.Any(e => e.RepairId == id);
        }

        [HttpGet]

        public async Task<IActionResult> LookupPart(string query)
        {
            if (string.IsNullOrWhiteSpace(query))
            {
                return Json(new { success = false, message = "Query is empty" });
            }

            var parts = _context.Parts.AsQueryable();
            var items = await parts
                .Where(i => i.PartName!.ToString().Contains(query))
                .ToListAsync();

            if (items == null || !items.Any())
            {
                return Json(new { success = false, message = "No items found" });
            }

            return Json(new { success = true, items = items });
        }

        [HttpPost]
        public async Task<IActionResult> AddPartLineItem(int repairId, int partId, int quantity=1)
        {
            try
            {
                var repair = await LoadRepair(repairId);

                if (repair == null)
                {
                    return View("Error", new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
                }

                var part = await _context.Parts.FindAsync(partId);
                if (part == null)
                {
                   return View("Error", new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
                }

                var existingPart = repair.RepairParts.FirstOrDefault(rp => rp.PartId == partId);
                if (existingPart == null)
                {
                    // part is not on the repair yet, add it
                    var repairPart = new RepairPart(repairId, quantity, part);
                    repairPart.LineTotal = quantity * part.Sale;
                    repair.RepairParts.Add(repairPart);
                    _context.Update(part);
                }
                else
                {
                    // part is already on the repair, update the quantity
                    existingPart.Quantity += 1;
                    existingPart.LineTotal = existingPart.Quantity * existingPart.Sale;
                    _context.Update(existingPart);
                }

                await _context.SaveChangesAsync();

                return PartialView("_PartsUsedTablePartialView", repair.RepairParts);
            }
            catch (Exception e)
            {
                return Json(new { success = false, message = "Error what?" + e.Message });
            }
        }


        [HttpPost]
        public async Task<IActionResult> DeletePartLineItem(int repairId, int repairPartId)
        {
            try
            {
                // Get the repair to remove the line from
                var repair = await LoadRepair(repairId);
                if (repair == null)
                {
                    return View("Error", new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
                }

                // Logic to delete the line item from the database
                var partToRemove = repair.RepairParts.FirstOrDefault(rp => rp.RepairPartId == repairPartId);
                if (partToRemove == null)
                {
                    return View("Error", new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
                }

                repair.RepairParts.Remove(partToRemove);

                // save the change
                await _context.SaveChangesAsync();

                // Notify the caller that the delete was successful
                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        [HttpPost]
        public async Task<IActionResult> AddNoteLine(int repairId)
        {
            try
            {
                var repair = await LoadRepair(repairId);
                if (repair == null)
                {
                    return View("Error", new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
                }

                // Add a new note
                var note = new RepairNote(repairId);
                note.TechnicianId = 1;
                note.Note = "Add note content";
                note.NoteDate = DateTime.Now;
                repair.RepairNotes.Add(note);
                _context.Update(repair);

                await _context.SaveChangesAsync();

                return PartialView("_NotesTablePartialView", repair.RepairNotes);
            }
            catch (Exception e)
            {
                return Json(new { success = false, message = e.Message });
            }
        }

        [HttpPost]
        public async Task<IActionResult> DeleteNoteLine(int repairId, int noteId)
        {
            try
            {
                // Get the repair to remove the line from
                var repair = await LoadRepair(repairId);
                if (repair == null)
                {
                    return View("Error", new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
                }

                // Logic to delete the line item from the database
                var noteToRemove = repair.RepairNotes.FirstOrDefault(rn => rn.RepairNoteId == noteId);
                if (noteToRemove == null)
                {
                    return View("Error", new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
                }

                repair.RepairNotes.Remove(noteToRemove);

                // save the change
                await _context.SaveChangesAsync();

                // Notify the caller that the delete was successful
                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }
        
        [HttpPost]
        public async Task<IActionResult> EditPartLine(int repairPartId, decimal sale, int quantity)
        {
            try
            {
                var part = await _context.RepairParts.FindAsync(repairPartId);
                if (part == null)
                {
                    return Json(new { success = false, message = "Note not found" });
                }

                part.Quantity = quantity;
                part.Sale = sale;
                part.LineTotal = sale * quantity;

                _context.Update(part);
                await _context.SaveChangesAsync();

                return Json(new { success = true, total = part.LineTotal });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }


        [HttpPost]
        public async Task<IActionResult> EditNoteLine(int noteId, string noteContent)
        {
            try
            {
                var note = await _context.RepairNotes.FindAsync(noteId);
                if (note == null)
                {
                    return Json(new { success = false, message = "Note not found" });
                }

                note.Note = noteContent;
                _context.Update(note);
                await _context.SaveChangesAsync();

                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        // POST: Repairs/SaveWork/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SaveWork(int repairId, Repair repair)
        {
            try
            {
                // notes are missing
                if (repairId != repair.RepairId)
                {
                    return NotFound();
                }

                if (ModelState.IsValid)
                {
                    try
                    {
                        _context.Update(repair);

                        // Update the parts
                        foreach (var part in repair.RepairParts)
                        {
                            if (_context.Entry(part).State == EntityState.Detached)
                            {
                                _context.Add(part); // For new lines
                            }
                            else
                            {
                                _context.Update(part);
                            }
                        }

                        // update the notes
                        foreach (var note in repair.RepairNotes)
                        {
                            if (_context.Entry(note).State == EntityState.Detached)
                            {
                                _context.Add(note); // For new lines
                            }
                            else
                            {
                                _context.Update(note);
                            }
                        }

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
            }
            catch (Exception e)
            {
                return Json(new { success = false, message = e.Message });
            }

            return View(nameof(WorkOnRepair), repair);
        }

        // POST: Repairs/FinishRepair/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> FinishRepair(int repairId, Repair repair)
        {
            try
            {
                // notes are missing
                if (repairId != repair.RepairId)
                {
                    return NotFound();
                }

                if (ModelState.IsValid)
                {
                    try
                    {
                        repair.FinishedDate = DateOnly.FromDateTime(DateTime.Now);
                        _context.Update(repair);

                        // Update the parts
                        foreach (var part in repair.RepairParts)
                        {
                            if (_context.Entry(part).State == EntityState.Detached)
                            {
                                _context.Add(part); // For new lines
                            }
                            else
                            {
                                _context.Update(part);
                            }
                        }

                        // update the notes
                        foreach (var note in repair.RepairNotes)
                        {
                            if (_context.Entry(note).State == EntityState.Detached)
                            {
                                _context.Add(note); // For new lines
                            }
                            else
                            {
                                _context.Update(note);
                            }
                        }

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
            }
            catch (Exception e)
            {
                return Json(new { success = false, message = e.Message });
            }

            return View(nameof(WorkOnRepair), repair);
        }


    }
}
