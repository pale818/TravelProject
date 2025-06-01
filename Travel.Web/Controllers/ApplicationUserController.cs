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

        /*
        [HttpGet]
        public async Task<IActionResult> Profile()
        {
            var token = HttpContext.Session.GetString("JWToken");

            // check if token is saved in session
            if (string.IsNullOrEmpty(token))
            {
                return RedirectToAction("Login", "Account");
            }
            Console.WriteLine($"Profile TOKEN: {token}");

            //_httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            //var userResponse = await _httpClient.GetFromJsonAsync<UserProfileViewModel>("api/ApplicationUser/me");


            var request = new HttpRequestMessage(HttpMethod.Get, "api/ApplicationUser/me");
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await _httpClient.SendAsync(request);
            var content = await response.Content.ReadAsStringAsync();
            Console.WriteLine("API Response:");
            Console.WriteLine(content);

            var userResponse = System.Text.Json.JsonSerializer.Deserialize<UpdateUser>(content);
            return View(userResponse);
        }
        */


        /*
        [HttpPut]
        public async Task<IActionResult> UpdateProfile(UpdateUser model)
        {
            var token = HttpContext.Session.GetString("JWToken");

            if (string.IsNullOrEmpty(token))
            {
                return RedirectToAction("Login", "Account");
            }

            var updateDto = new
            {
                model.Email,
                model.FirstName,
                model.LastName,
                model.PhoneNumber
            };

            var request = new HttpRequestMessage(HttpMethod.Put, "api/ApplicationUser/me")
            {
                Content = JsonContent.Create(updateDto)
            };

            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await _httpClient.SendAsync(request);

            if (response.IsSuccessStatusCode)
            {
                ViewBag.Message = "Profile updated successfully!";
                return RedirectToAction("Profile");
            }

            ViewBag.Message = "Update failed.";
            return View("Profile", model);
        }
        */


        // FOR USER PROFILE
        public IActionResult UserProfile()
        {
            return View();
        }






    }
}
