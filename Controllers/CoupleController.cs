using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using DreamDay.Models;
using DreamDay.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;


namespace DreamDay.Controllers;

[Authorize(Roles = "Couple")]
public class CoupleController : Controller
{
    private readonly ApplicationDbContext _context;
    private readonly UserManager<ApplicationUser> _userManager;

    public CoupleController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
    {
        _context = context;
        _userManager = userManager;
    }

    public async Task<IActionResult> Dashboard()
    {
        var user = await _userManager.GetUserAsync(User);
        var wedding = await _context.Weddings
            .Include(w => w.Vendors)
            .Include(w => w.Planner)
            .FirstOrDefaultAsync(w => w.CoupleId == user.Id);

        return View(wedding);
    }

    [HttpGet]
public async Task<IActionResult> CreateWedding()
{
    var planners = await _userManager.GetUsersInRoleAsync("Planner");
    var vendors = await _context.Vendors.ToListAsync();

    var viewModel = new WeddingCreateViewModel
    {
        AvailablePlanners = planners.ToList(),
        AvailableVendors = vendors
    };

    return View(viewModel);
}

[HttpPost]
public async Task<IActionResult> CreateWedding(WeddingCreateViewModel model)
{
    if (!ModelState.IsValid)
    {
        // Repopulate vendors and planners before returning to view
        model.AvailablePlanners = (await _userManager.GetUsersInRoleAsync("Planner")).ToList();
        model.AvailableVendors = await _context.Vendors.ToListAsync();

        return View(model); // Return form with errors + data
    }

    var user = await _userManager.GetUserAsync(User);

    var wedding = new Wedding
    {
        Title = model.Title,
        Date = model.Date.Value,
        CoupleId = user.Id,
        PlannerId = model.PlannerId,
        Vendors = await _context.Vendors
            .Where(v => model.SelectedVendorIds.Contains(v.Id))
            .ToListAsync()
    };

    _context.Weddings.Add(wedding);
    await _context.SaveChangesAsync();

    return RedirectToAction("Dashboard");
}


}
