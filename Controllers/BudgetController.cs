using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DreamDay.Data;
using DreamDay.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace DreamDay.Controllers
{
    [Authorize(Roles = "Couple")]
    public class BudgetController : Controller
    {
        private readonly ApplicationDbContext _context;

        public BudgetController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Budget for a specific Wedding
        public async Task<IActionResult> Index(int weddingId)
        {
            var wedding = await _context.Weddings
                .Include(w => w.BudgetItems)
                .FirstOrDefaultAsync(w => w.Id == weddingId);

            if (wedding == null)
            {
                return NotFound();
            }

            // Convert HashSet to List before passing to the view
            var budgetItems = wedding.BudgetItems.ToList();

            // Pass weddingId to the view
            ViewBag.WeddingId = weddingId;

            return View(budgetItems);
        }


        // GET: Create a new BudgetItem
        [HttpGet]
        public IActionResult Create(int weddingId)
        {
            // Predefined categories for the budget item
            var categories = new List<string>
    {
        "Venue",
        "Catering",
        "Photography",
        "Decor",
        "Entertainment",
        "Transportation",
        "Other"
    };

            // Pass the categories to the view
            ViewBag.Categories = new SelectList(categories);

            // Pass the WeddingId to the view
            ViewBag.WeddingId = weddingId;

            return View();
        }


        // POST: Create a new BudgetItem
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(BudgetItem budgetItem)
        {
            if (ModelState.IsValid)
            {
                _context.BudgetItems.Add(budgetItem);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index", new { weddingId = budgetItem.WeddingId });
            }
            return View(budgetItem);
        }

        // GET: Edit a BudgetItem
        [HttpGet]
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var budgetItem = await _context.BudgetItems.FindAsync(id);
            if (budgetItem == null)
            {
                return NotFound();
            }

            // Predefined categories for the budget item
            var categories = new List<string>
    {
        "Venue",
        "Catering",
        "Photography",
        "Decor",
        "Entertainment",
        "Transportation",
        "Other"
    };

            // Pass the categories to the view
            ViewBag.Categories = new SelectList(categories, budgetItem.Category);

            // Pass the WeddingId to the view
            ViewBag.WeddingId = budgetItem.WeddingId;

            return View(budgetItem);
        }


        // POST: Edit a BudgetItem
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, BudgetItem budgetItem)
        {
            if (id != budgetItem.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(budgetItem);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BudgetItemExists(budgetItem.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Index", new { weddingId = budgetItem.WeddingId });
            }
            return View(budgetItem);
        }

        // GET: Delete a BudgetItem
        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var budgetItem = await _context.BudgetItems
                .FirstOrDefaultAsync(b => b.Id == id);
            if (budgetItem == null)
            {
                return NotFound();
            }
            return View(budgetItem);
        }

        // POST: Delete a BudgetItem
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var budgetItem = await _context.BudgetItems.FindAsync(id);
            _context.BudgetItems.Remove(budgetItem);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index", new { weddingId = budgetItem.WeddingId });
        }

        private bool BudgetItemExists(int id)
        {
            return _context.BudgetItems.Any(e => e.Id == id);
        }
    }
}
