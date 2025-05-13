using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using DreamDay.Models;
using DreamDay.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using DreamDay.ViewModels;


namespace DreamDay.Controllers;

[Authorize(Roles = "Couple , Planner")]
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
        // Log errors if the model is invalid
        Debug.WriteLine("❌ ModelState is invalid.");
        foreach (var entry in ModelState)
        {
            var key = entry.Key;
            var errors = entry.Value.Errors;
            foreach (var error in errors)
            {
                Debug.WriteLine($"Error in '{key}': {error.ErrorMessage}");
            }
        }

        // Repopulate necessary data (vendors and planners)
        model.AvailablePlanners = (await _userManager.GetUsersInRoleAsync("Planner")).ToList();
        model.AvailableVendors = await _context.Vendors.ToListAsync();

        return View(model); // Return form with errors + data
    }

    var user = await _userManager.GetUserAsync(User);

    // Log selected data
    Debug.WriteLine($"✔️ Creating wedding for user: {user.Email}");
    Debug.WriteLine($"➡️ Title: {model.Title}");
    Debug.WriteLine($"➡️ Date: {model.Date}");
    Debug.WriteLine($"➡️ PlannerId: {model.PlannerId}");
    Debug.WriteLine("➡️ SelectedVendorIds: " + string.Join(", ", model.SelectedVendorIds ?? new List<int>()));

    // Fetch the selected vendors
    var vendors = await _context.Vendors
        .Where(v => model.SelectedVendorIds.Contains(v.Id))
        .ToListAsync();

    if (!vendors.Any())
    {
        Debug.WriteLine("⚠️ No vendors selected.");
        ModelState.AddModelError(string.Empty, "Please select at least one vendor."); // Add a custom error
        model.AvailablePlanners = (await _userManager.GetUsersInRoleAsync("Planner")).ToList();
        model.AvailableVendors = await _context.Vendors.ToListAsync();
        return View(model); // Return form with error message
    }

    // Create wedding object and save
    var wedding = new Wedding
    {
        Title = model.Title,
        Date = model.Date.Value,
        CoupleId = user.Id,
        PlannerId = model.PlannerId,
        Vendors = vendors
    };

    _context.Weddings.Add(wedding);
    await _context.SaveChangesAsync();

    Debug.WriteLine("✅ Wedding saved successfully.");
    return RedirectToAction("Dashboard");
}

}
