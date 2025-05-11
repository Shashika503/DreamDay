using Microsoft.AspNetCore.Mvc;

namespace DreamDay.Controllers
{
    public class AboutController : Controller
    {
        // INDEX - Display information about the website/company
        public IActionResult Index()
        {
            return View();
        }
    }
}
