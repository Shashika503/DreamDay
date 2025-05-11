using Microsoft.AspNetCore.Mvc;

namespace DreamDay.Controllers
{
    public class ContactController : Controller
    {
        // INDEX - Display contact information
        public IActionResult Index()
        {
            return View();
        }
    }
}
