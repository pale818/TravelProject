using Microsoft.AspNetCore.Mvc;
using Travel.Web.Models;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;



namespace Travel.Web.Controllers
{
    public class AccountController : Controller
    {
        private readonly HttpClient _httpClient;

        public AccountController(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient();
            _httpClient.BaseAddress = new Uri("https://localhost:7172/"); // Your Travel.API base URL
        }

        public IActionResult Login()
        {
            return View();
        }

        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(ApplicationUser model)
        {
            var response = await _httpClient.PostAsJsonAsync("api/auth/register", model);
            Console.WriteLine($"REGISTER RESPONSE: {response.Content}");
            Console.WriteLine($"REGISTER IsSuccessStatusCode: {response.IsSuccessStatusCode}");

            if (response.IsSuccessStatusCode)
            {
                    return RedirectToAction("Login");
            }
            //ModelState.AddModelError("", "Registration failed.");
            TempData["Error"] = "Registration failed.";
            return View(model);
        }



        [HttpPost]
        public async Task<IActionResult> Login(LoginRequest model)
        {
            var response = await _httpClient.PostAsJsonAsync("api/auth/login", model);

            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadFromJsonAsync<JwtResponse>();
                if (result != null)
                {

                    HttpContext.Session.SetString("JWToken", result.Token);
                    Console.WriteLine($"Profile TOKEN: {result.Token}");
                    TempData["Token"] = result.Token;
                    TempData["SuccessMessage"] = "Successful log in.";

                    // this is .Net redirection function meaning: "Dashboard" is razor page (cshtml) and "Home" is folder where
                    // Dashboard.cshtml is located
                    // SO IF BACKEND CONFIRMS USER'S CREDENTIOALS (Usernam + password) IT RETURNS 200 OK
                    // JWT token also and code comes here where Home/Dashboard,cshtml is loaded
                    return RedirectToAction("Dashboard", "Home");
                }
            }


            TempData["ErrorMessage"] = "Login failed. Invalid username or password.";
            return View(model);
        }




        [HttpPost]
        public IActionResult Logout()
        {
            Console.WriteLine("Logout executed");

            HttpContext.Session.Remove("JWToken");
            HttpContext.Session.Clear();
            return RedirectToAction("Login", "Account");
        }

    }
}
