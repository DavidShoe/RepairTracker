using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using RepairTracker.DBModels;
using System.Diagnostics;

namespace RepairTracker.Controllers
{
    public class AddCategoryViewItemModel
    {
        GameRepairContext Context;
        public AddCategoryViewItemModel(GameRepairContext context)
        {
            Context = context;
        }

        public String? Category { get; set; }
    }

    public class PartsController : Controller
    {
        private readonly GameRepairContext _context;

        public PartsController(GameRepairContext context)
        {
            _context = context;
        }

        // GET: Parts
        public async Task<IActionResult> PartsIndex()
        {
            var gameRepairContext = _context.Parts.Include(p => p.Category);
            return View(await gameRepairContext.ToListAsync());
        }

        // GET: Parts/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var part = await _context.Parts
                .Include(p => p.Category)
                .FirstOrDefaultAsync(m => m.PartId == id);
            if (part == null)
            {
                return NotFound();
            }

            return View(part);
        }

        // GET: Parts/Create
        public IActionResult Create()
        {
            ViewData["Categories"] = new SelectList(_context.Categories, "CategoryId", "CategoryName");
            return View();
        }

        // POST: Parts/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("PartId,PartName,Cost,Sale,Supplier,CategoryId")] Part part)
        {
            if (ModelState.IsValid)
            {
                _context.Add(part);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(PartsIndex));
            }
            ViewData["Categories"] = new SelectList(_context.Categories, "CategoryId", "CategoryName", part.CategoryId);
            return View(part);
        }

        // GET: Parts/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var part = await _context.Parts.FindAsync(id);
            if (part == null)
            {
                return NotFound();
            }
            ViewData["Categories"] = new SelectList(_context.Categories, "CategoryId", "CategoryName");
            return View(part);
        }

        // POST: Parts/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("PartId,PartName,Cost,Sale,Supplier,CategoryId")] Part part)
        {
            if (id != part.PartId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(part);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PartExists(part.PartId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(PartsIndex));
            }
            ViewData["Categories"] = new SelectList(_context.Categories, "CategoryId", "CategoryName");
            return View(part);
        }

        // GET: Parts/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var part = await _context.Parts
                .Include(p => p.Category)
                .FirstOrDefaultAsync(m => m.PartId == id);
            if (part == null)
            {
                return NotFound();
            }

            return View(part);
        }

        // POST: Parts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var part = await _context.Parts.FindAsync(id);
            if (part != null)
            {
                _context.Parts.Remove(part);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(PartsIndex));
        }

        [HttpPost, ActionName("CreateCategory")]
        public async Task<IActionResult> CreateCategory(string CategoryName)
        {
            Debug.WriteLine($"CreateCagetory -> CategoryName: {CategoryName}");
            if (string.IsNullOrEmpty(CategoryName))
            {
                return Json(new { success = false, message = "Category name is required." });
            }

            var category = new Category { CategoryName = CategoryName };
            _context.Categories.Add(category);
            await _context.SaveChangesAsync();

            return Json(new { success = true, category });
        }

        private bool PartExists(int id)
        {
            return _context.Parts.Any(e => e.PartId == id);
        }

        public IActionResult AddCategoryPartialView()
        {
            var viewModel = new AddCategoryViewItemModel(_context);
            return PartialView("_AddCagetoryPartialView", viewModel);
        }
    }
}
