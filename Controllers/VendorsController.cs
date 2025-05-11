using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DreamDay.Data;
using DreamDay.Models;

namespace DreamDay.Controllers
{
    public class VendorsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public VendorsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // INDEX - Get the list of vendors
        public async Task<IActionResult> Index()
        {
            var vendors = await _context.Vendors.ToListAsync();
            return View(vendors);
        }
    }
}
