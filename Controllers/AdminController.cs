using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DreamDay.Data;
using DreamDay.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Rendering;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;



namespace DreamDay.Controllers
{
    [Authorize(Roles = "Admin")]  // Ensure this is protected for Admins
    public class AdminController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public AdminController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // Action to export data to PDF
        public IActionResult ExportToPdf()
        {
            //Set license type (required by QuestPDF)
            QuestPDF.Settings.License = LicenseType.Community;
            var weddingsCount = _context.Weddings.Count();
            var vendorsCount = _context.Vendors.Count();
            var plannersCount = _userManager.GetUsersInRoleAsync("Planner").Result.Count;

            var stream = new MemoryStream();

            QuestPDF.Fluent.Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Size(PageSizes.A4);
                    page.Margin(50);
                    page.DefaultTextStyle(x => x.FontSize(14));

                    page.Header()
                        .PaddingBottom(15)
                        .Text("Admin Dashboard Report")
                        .FontSize(20)
                        .Bold()
                        .AlignCenter();

                    page.Content().Table(table =>
                    {
                        table.ColumnsDefinition(columns =>
                        {
                            columns.RelativeColumn();
                            columns.ConstantColumn(100);
                        });

                        table.Header(header =>
                        {
                            header.Cell().Element(CellStyle).Text("Category").Bold();
                            header.Cell().Element(CellStyle).Text("Count").Bold();
                        });

                        table.Cell().Element(CellStyle).Text("Weddings");
                        table.Cell().Element(CellStyle).Text(weddingsCount.ToString());

                        table.Cell().Element(CellStyle).Text("Vendors");
                        table.Cell().Element(CellStyle).Text(vendorsCount.ToString());

                        table.Cell().Element(CellStyle).Text("Planners");
                        table.Cell().Element(CellStyle).Text(plannersCount.ToString());
                    });

                    static IContainer CellStyle(IContainer container) =>
                        container.PaddingVertical(5).BorderBottom(1).BorderColor(Colors.Grey.Lighten2);
                });
            })
            .GeneratePdf(stream);

            stream.Position = 0;
            return File(stream, "application/pdf", "AdminDashboard.pdf");
        }
        // Action to export data to Excel
        public async Task<IActionResult> ExportExcel()
        {
            var weddingsCount = _context.Weddings.Count();
            var vendorsCount = _context.Vendors.Count();
            var plannersCount = await _userManager.GetUsersInRoleAsync("Planner");

            var stream = new MemoryStream(); // DO NOT use using() here

            using (var workbook = new ClosedXML.Excel.XLWorkbook())
            {
                var worksheet = workbook.Worksheets.Add("Admin Report");

                // Header
                worksheet.Cell(1, 1).Value = "Category";
                worksheet.Cell(1, 2).Value = "Count";

                // Data rows
                worksheet.Cell(2, 1).Value = "Weddings";
                worksheet.Cell(2, 2).Value = weddingsCount;

                worksheet.Cell(3, 1).Value = "Vendors";
                worksheet.Cell(3, 2).Value = vendorsCount;

                worksheet.Cell(4, 1).Value = "Planners";
                worksheet.Cell(4, 2).Value = plannersCount.Count;

                worksheet.Row(1).Style.Font.Bold = true;

                workbook.SaveAs(stream);
            }

            stream.Position = 0; // Reset stream position for reading

            return File(
                stream,
                "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                "AdminReport.xlsx"
            );
        }

        // GET: Admin Dashboard
        public async Task<IActionResult> Dashboard()
        {
            // Get the number of weddings, vendors, and planners
            var numberOfWeddings = await _context.Weddings.CountAsync();
            var numberOfVendors = await _context.Vendors.CountAsync();
            var numberOfPlanners = await _userManager.GetUsersInRoleAsync("Planner");

            // Pass the data to the view
            ViewBag.NumberOfWeddings = numberOfWeddings;
            ViewBag.NumberOfVendors = numberOfVendors;
            ViewBag.NumberOfPlanners = numberOfPlanners.Count;

            return View();
        }

        // ----- Vendors CRUD -----

        // GET: Vendors
        public async Task<IActionResult> IndexVendors()
        {
            var vendors = await _context.Vendors.ToListAsync();
            return View(vendors);
        }

        // GET: Vendors/Create
        public IActionResult CreateVendor()
        {
            var categories = new List<string> { "Venue", "Catering", "Photography", "Decor", "Entertainment", "Transportation" };
            ViewBag.Categories = new SelectList(categories); // Pass the categories to the view
            return View();
        }

        // POST: Vendors/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateVendor([Bind("Name,Category,Description,PriceRange")] Vendor vendor)
        {
            if (ModelState.IsValid)
            {
                _context.Add(vendor);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(IndexVendors));
            }
            return View(vendor);
        }

        // GET: Vendors/Edit/5
        public async Task<IActionResult> EditVendor(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var vendor = await _context.Vendors.FindAsync(id);
            if (vendor == null)
            {
                return NotFound();
            }

            var categories = new List<string> { "Venue", "Catering", "Photography", "Decor", "Entertainment", "Transportation" };
            ViewBag.Categories = new SelectList(categories, vendor.Category); // Pass selected category to the view

            return View(vendor);
        }

        // POST: Vendors/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditVendor(int id, [Bind("Id,Name,Category,Description,PriceRange")] Vendor vendor)
        {
            if (id != vendor.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(vendor);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!VendorExists(vendor.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(IndexVendors));
            }
            return View(vendor);
        }


        // GET: Vendors/Delete/5
        public async Task<IActionResult> DeleteVendor(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var vendor = await _context.Vendors
                .FirstOrDefaultAsync(m => m.Id == id);
            if (vendor == null)
            {
                return NotFound();
            }

            return View(vendor);
        }

        // POST: Vendors/Delete/5
        [HttpPost, ActionName("DeleteVendor")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteVendorConfirmed(int id)
        {
            var vendor = await _context.Vendors.FindAsync(id);
            _context.Vendors.Remove(vendor);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(IndexVendors));
        }

        private bool VendorExists(int id)
        {
            return _context.Vendors.Any(e => e.Id == id);
        }


        // ----- Planners CRUD -----

        // GET: Planners (admin can see all planners)
        public async Task<IActionResult> IndexPlanners()
        {
            var planners = await _userManager.GetUsersInRoleAsync("Planner");
            return View(planners);
        }

        // GET: Planners/Create
        public IActionResult CreatePlanner()
        {
            return View();
        }

        // POST: Planners/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreatePlanner([Bind("FullName,Email")] ApplicationUser planner)
        {
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser { UserName = planner.Email, Email = planner.Email, FullName = planner.FullName };
                var result = await _userManager.CreateAsync(user, "DefaultPassword123!");

                if (result.Succeeded)
                {
                    // Assign Planner role
                    await _userManager.AddToRoleAsync(user, "Planner");
                    return RedirectToAction(nameof(IndexPlanners));
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }
            return View(planner);
        }

        // GET: Planners/Edit/5
        public async Task<IActionResult> EditPlanner(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var planner = await _userManager.FindByIdAsync(id);
            if (planner == null)
            {
                return NotFound();
            }
            return View(planner);
        }

        // POST: Planners/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditPlanner(string id, [Bind("FullName,Email")] ApplicationUser planner)
        {
            if (id != planner.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var plannerToUpdate = await _userManager.FindByIdAsync(id);
                    plannerToUpdate.FullName = planner.FullName;
                    plannerToUpdate.Email = planner.Email;

                    await _userManager.UpdateAsync(plannerToUpdate);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PlannerExists(planner.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(IndexPlanners));
            }
            return View(planner);
        }

        // GET: Planners/Delete/5
        public async Task<IActionResult> DeletePlanner(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var planner = await _userManager.FindByIdAsync(id);
            if (planner == null)
            {
                return NotFound();
            }

            return View(planner);
        }

        // POST: Planners/Delete/5
        [HttpPost, ActionName("DeletePlanner")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeletePlannerConfirmed(string id)
        {
            var planner = await _userManager.FindByIdAsync(id);
            await _userManager.DeleteAsync(planner);
            return RedirectToAction(nameof(IndexPlanners));
        }

        private bool PlannerExists(string id)
        {
            return _userManager.Users.Any(e => e.Id == id);
        }
    }
}
