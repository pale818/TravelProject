using Microsoft.AspNetCore.Mvc;
using Travel.Web.Models;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using System.Net.Http.Headers;

namespace Travel.Web.Controllers
{
    public class ApplicationUserController : Controller
    {
        private readonly HttpClient _httpClient;

        public ApplicationUserController(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient();
            _httpClient.BaseAddress = new Uri("https://localhost:7172/");
        }


        public IActionResult Index()
        {
            return View();
        }

       

        // FOR USER PROFILE
        public IActionResult UserProfile()
        {
            return View();
        }

        




    }
}
