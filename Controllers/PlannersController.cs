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
    }
}
