using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DreamDay.Data;
using DreamDay.Models;
using Microsoft.AspNetCore.Identity;

namespace DreamDay.Controllers
{
    public class PlannerController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public PlannerController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // INDEX - Get the list of planners
        public async Task<IActionResult> Index()
        {
            var planners = (await _userManager.GetUsersInRoleAsync("Planner")).ToList();
            return View(planners);
        }

        // Dashboard - Show all weddings assigned to the planner
        public async Task<IActionResult> Dashboard()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound();
            }

            // Get all weddings where the current user is the planner
            var weddings = await _context.Weddings
                .Where(w => w.PlannerId == user.Id)
                .Include(w => w.Vendors)
                .Include(w => w.TimelineEvents)
                .Include(w => w.ChecklistItems)
                .Include(w => w.Guests)
                .ToListAsync();

            return View(weddings); // Pass the list of weddings to the view
        }
    
    }
}
