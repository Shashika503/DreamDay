using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DreamDay.Data;
using DreamDay.Models;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace DreamDay.Controllers
{
    [Authorize(Roles = "Couple, Planner")]
    public class ChecklistController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ChecklistController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Checklist for a specific Wedding
        public async Task<IActionResult> Index(int weddingId)
        {
            var wedding = await _context.Weddings
                .Include(w => w.ChecklistItems)
                .FirstOrDefaultAsync(w => w.Id == weddingId);

            if (wedding == null)
            {
                return NotFound();
            }

            if (User.IsInRole("Planner") && wedding.PlannerId != User.FindFirstValue(ClaimTypes.NameIdentifier))
{
    return Unauthorized(); // Ensure only the assigned planner can access the wedding details
}



            var checkItems = wedding.ChecklistItems.ToList();

            // Pass weddingId to the View to create the links
            ViewBag.WeddingId = weddingId;
            return View(checkItems);
        }

        // GET: Create a new ChecklistItem
        [HttpGet]
        public IActionResult Create(int weddingId)
        {
            ViewBag.WeddingId = weddingId;
            return View();
        }

        // POST: Create a new ChecklistItem
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ChecklistItem checklistItem)
        {
            if (ModelState.IsValid)
            {
                _context.ChecklistItems.Add(checklistItem);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index", new { weddingId = checklistItem.WeddingId });
            }
            return View(checklistItem);
        }

        // GET: Edit a ChecklistItem
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var checklistItem = await _context.ChecklistItems.FindAsync(id);
            if (checklistItem == null)
            {
                return NotFound();
            }

            // Pass the WeddingId to the view
            ViewBag.WeddingId = checklistItem.WeddingId;
            return View(checklistItem);
        }

        // POST: Edit a ChecklistItem
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, ChecklistItem checklistItem)
        {
            if (id != checklistItem.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(checklistItem);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ChecklistItemExists(checklistItem.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Index", new { weddingId = checklistItem.WeddingId });
            }
            return View(checklistItem);
        }

        // GET: Delete a ChecklistItem
        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var checklistItem = await _context.ChecklistItems
                .FirstOrDefaultAsync(c => c.Id == id);
            if (checklistItem == null)
            {
                return NotFound();
            }
            return View(checklistItem);
        }

        // POST: Delete a ChecklistItem
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var checklistItem = await _context.ChecklistItems.FindAsync(id);
            _context.ChecklistItems.Remove(checklistItem);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index", new { weddingId = checklistItem.WeddingId });
        }

        private bool ChecklistItemExists(int id)
        {
            return _context.ChecklistItems.Any(e => e.Id == id);
        }
    }
}
