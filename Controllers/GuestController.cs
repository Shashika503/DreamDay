using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DreamDay.Data;
using DreamDay.Models;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace DreamDay.Controllers
{
    [Authorize(Roles = "Couple, Planner")]
    public class GuestController : Controller
    {
        private readonly ApplicationDbContext _context;

        public GuestController(ApplicationDbContext context)
        {
            _context = context;
        }

        // INDEX - List all guests for a wedding
        public async Task<IActionResult> Index(int weddingId)
        {
            var wedding = await _context.Weddings
                .Include(w => w.Guests)
                .FirstOrDefaultAsync(w => w.Id == weddingId);

            if (wedding == null)
            {
                return NotFound();
            }
           if (User.IsInRole("Planner") && wedding.PlannerId != User.FindFirstValue(ClaimTypes.NameIdentifier))
{
    return Unauthorized(); // Ensure only the assigned planner can access the wedding details
}


            ViewBag.WeddingId = weddingId;

            var guests = wedding.Guests.ToList();


            return View(guests);
        }

        // CREATE - Show the form to add a new guest
        [HttpGet]
        public IActionResult Create(int weddingId)
        {
            ViewBag.WeddingId = weddingId;
            return View();
        }

        // CREATE - Post new guest to the database
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Guest guest)
        {
            if (ModelState.IsValid)
            {
                _context.Guests.Add(guest);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index", new { weddingId = guest.WeddingId });
            }
            return View(guest);
        
        
        }

        // EDIT - Show the form to edit a guest's details
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var guest = await _context.Guests.FindAsync(id);
            if (guest == null)
            {
                return NotFound();
            }
            ViewBag.WeddingId = guest.WeddingId;
            return View(guest);
        }

        // EDIT - Post updated guest details to the database
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Guest guest)
        {
            if (id != guest.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(guest);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!GuestExists(guest.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Index", new { weddingId = guest.WeddingId });
            }
            return View(guest);
        }

        // DELETE - Show confirmation to delete a guest
        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var guest = await _context.Guests
                .FirstOrDefaultAsync(m => m.Id == id);
            if (guest == null)
            {
                return NotFound();
            }

            return View(guest);
        }

        // DELETE - Post to delete a guest from the database
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var guest = await _context.Guests.FindAsync(id);
            _context.Guests.Remove(guest);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index", new { weddingId = guest.WeddingId });
        }

        private bool GuestExists(int id)
        {
            return _context.Guests.Any(e => e.Id == id);
        }
    }
}
