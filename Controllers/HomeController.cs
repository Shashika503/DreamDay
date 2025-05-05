using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using DreamDay.Models;
using DreamDay.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;


namespace DreamDay.Controllers;


public class HomeController : Controller
{
    private readonly ApplicationDbContext _context;
    private readonly UserManager<ApplicationUser> _userManager;

    public HomeController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
    {
        _context = context;
        _userManager = userManager;
    }

    public async Task<IActionResult> Index()
    {
        var vendors = await _context.Vendors.Take(6).ToListAsync();

       var planners = (await _userManager.GetUsersInRoleAsync("Planner")).ToList();


        var pastWeddings = await _context.Weddings
            .Include(w => w.Couple)
            .Where(w => w.Date < DateTime.Now)
            .OrderByDescending(w => w.Date)
            .Take(3)
            .ToListAsync();

        var viewModel = new LandingPageViewModel
        {
            Vendors = vendors,
            Planners = planners,
            PastWeddings = pastWeddings
        };

        return View(viewModel);
    }
}