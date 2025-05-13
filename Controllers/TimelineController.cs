using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DreamDay.Data;
using DreamDay.Models;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace DreamDay.Controllers
{
    [Authorize(Roles = "Couple, Planner")]
    public class TimelineController : Controller
    {
        private readonly ApplicationDbContext _context;

        public TimelineController(ApplicationDbContext context)
        {
            _context = context;
        }

        // READ - List Timeline Events for a Wedding
       public async Task<IActionResult> Index(int weddingId)
{
    var wedding = await _context.Weddings
        .Include(w => w.TimelineEvents)
        .FirstOrDefaultAsync(w => w.Id == weddingId);

    if (wedding == null)
    {
        return NotFound();
    }

            if (wedding.PlannerId != User.FindFirstValue(ClaimTypes.NameIdentifier))
            {
                return Unauthorized(); // Ensure only the assigned planner can access the wedding details
            }



            ViewBag.WeddingId = weddingId;

            // Convert the HashSet to a List before passing to the view
            var timelineEvents = wedding.TimelineEvents.ToList();

    // Pass the List to the view
    return View(timelineEvents);
}


        // CREATE - Show the form to add a new Timeline Event
        [HttpGet]
        public IActionResult Create(int weddingId)
        {
            ViewBag.WeddingId = weddingId;
            return View(new TimelineEvent { WeddingId = weddingId });
        }

        // CREATE - Post new Timeline Event to the Database
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(TimelineEvent timelineEvent)
        {
            if (ModelState.IsValid)
            {
                _context.TimelineEvents.Add(timelineEvent);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index", new { weddingId = timelineEvent.WeddingId });
            }
            return View(timelineEvent);
        }

        // UPDATE - Edit Timeline Event
        [HttpGet]
        public async Task<IActionResult> Edit(int id, int weddingId)
        {
            var timelineEvent = await _context.TimelineEvents
                .FirstOrDefaultAsync(t => t.Id == id && t.WeddingId == weddingId);

            if (timelineEvent == null)
            {
                return NotFound();
            }

            // Pass the weddingId to the view if needed for form submission
            ViewBag.WeddingId = weddingId;
            return View(timelineEvent);
        }


        // UPDATE - Post updated Timeline Event to the Database
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, TimelineEvent timelineEvent)
        {
            if (id != timelineEvent.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(timelineEvent);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TimelineEventExists(timelineEvent.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Index", new { weddingId = timelineEvent.WeddingId });
            }
            return View(timelineEvent);
        }

        // DELETE - Delete Timeline Event
        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var timelineEvent = await _context.TimelineEvents
                .FirstOrDefaultAsync(m => m.Id == id);
            if (timelineEvent == null)
            {
                return NotFound();
            }

            return View(timelineEvent);
        }

        // DELETE - Post to delete Timeline Event from the Database
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var timelineEvent = await _context.TimelineEvents.FindAsync(id);
            _context.TimelineEvents.Remove(timelineEvent);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index", new { weddingId = timelineEvent.WeddingId });
        }

        private bool TimelineEventExists(int id)
        {
            return _context.TimelineEvents.Any(e => e.Id == id);
        }
    }
}
