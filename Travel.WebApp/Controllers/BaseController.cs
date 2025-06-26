using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Mvc;

namespace Travel.Web.Controllers
{
    public class BaseController : Controller
    {
        protected void SetIsAdminViewData()
        {
            var token = HttpContext.Session.GetString("JWToken");
            if (string.IsNullOrEmpty(token))
            {
                ViewData["IsAdmin"] = false;
                return;
            }

            var handler = new JwtSecurityTokenHandler();
            var jwtToken = handler.ReadJwtToken(token);
            var isAdmin = jwtToken.Claims.FirstOrDefault(c => c.Type == "isAdmin")?.Value;

            // Set ViewData so it's available in the Layout
            ViewData["IsAdmin"] = isAdmin == "True" || isAdmin == "true" || isAdmin == "1";
        }
    }
}