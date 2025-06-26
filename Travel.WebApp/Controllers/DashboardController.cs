using Microsoft.AspNetCore.Mvc;

namespace Travel.Web.Controllers
{
    public class DashboardController : BaseController
    {

        // those functions are onl to show corresponding pages (cshtml files)
        // for example UserManagement() function will load UserManagement.cshtml file and
        public IActionResult UserManagement()
        {
            SetIsAdminViewData();
            return View();
        }

        public IActionResult TripManagement()
        {
            SetIsAdminViewData(); 
            return View();
        }

        public IActionResult GuideManagement()
        {
            SetIsAdminViewData(); 
            return View();
        }

        public IActionResult TripGuideManagement()
        {
            SetIsAdminViewData(); 
            return View();
        }

        public IActionResult DestinationManagement()
        {
            SetIsAdminViewData(); 
            return View();
        }

        public IActionResult WishlistManagement()
        {
            SetIsAdminViewData(); 
            return View();
        }

        public IActionResult WishlistManagementAdmin()
        {
            SetIsAdminViewData(); 
            return View();
        }

        public IActionResult LogManagement()
        {
            SetIsAdminViewData(); 
            return View();
        }
    }
}