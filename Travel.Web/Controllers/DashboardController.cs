using Microsoft.AspNetCore.Mvc;

namespace Travel.Web.Controllers
{
    public class DashboardController : Controller
    {

        // those functions are onl to show corresponding pages (cshtml files)
        // for example UserManagement() function will load UserManagement.cshtml file and
        public IActionResult UserManagement()
        {
            return View();
        }

        public IActionResult TripManagement()
        {
            return View();
        }

        public IActionResult GuideManagement()
        {
            return View();
        }

        public IActionResult TripGuideManagement()
        {
            return View();
        }

        public IActionResult DestinationManagement()
        {
            return View();
        }

        public IActionResult WishlistManagement()
        {
            return View();
        }
    }
}