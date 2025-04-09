using Microsoft.AspNetCore.Mvc;
using Travel.API.Data;
using Travel.API.Helpers;
using Travel.API.Models;
using Travel.API.Dtos;
using Microsoft.EntityFrameworkCore;

namespace Travel.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly JwtTokenGenerator _jwtTokenGenerator;

        public AuthController(ApplicationDbContext context, JwtTokenGenerator jwtTokenGenerator)
        {
            _context = context;
            _jwtTokenGenerator = jwtTokenGenerator;
        }

        /*
         * Use **IActionResult** when you just return status codes or small info, not a data model
         * 
         * Why use a DTO (RegisterRequest) instead of the entity?
            Very important:

            Direct ApplicationUser	DTO (RegisterRequest)
            Risky: exposes all fields of the model	Safe: only the fields you want from the client
            Tightly coupled to DB schema	Decoupled, flexible
            Could accidentally allow bad fields (e.g., Id, CreatedAt, Token)	You control exactly what comes in
            That's why DTOs (Data Transfer Objects) are the modern, safe way to handle request/response data.

            Aspect	            PostUser(ApplicationUser user)          Register([FromBody] RegisterRequest request)
            Return type	        ActionResult<ApplicationUser>	        IActionResult
            Return value	    Sends back created user	                Sends status, maybe token
            Input type	        Entity (ApplicationUser)	            DTO (RegisterRequest)
            [FromBody] needed?	Optional	                            Recommended
            Use case	        CRUD (EF-based scaffolding)	            Custom logic (auth, validation)
            Secure/flexible     Risky if exposed directly	            Best practice

         */
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequest request)
        {
            if (_context.ApplicationUsers.Any(u => u.Email == request.Email))
                return BadRequest("Email already exists");

            var user = new ApplicationUser
            {
                UserName = request.UserName,
                Email = request.Email,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Password),
                FirstName = request.FirstName,
                LastName = request.LastName,
                PhoneNumber = request.PhoneNumber,
                IsAdmin = request.IsAdmin
            };

            _context.ApplicationUsers.Add(user);
            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            var user = await _context.ApplicationUsers
                .FirstOrDefaultAsync(u => u.UserName == request.UserName);

            if (user == null)
            {
                Console.WriteLine("User not found");
                return Unauthorized();
            }

            Console.WriteLine($"User found: {user.UserName}");
            Console.WriteLine($"Incoming password: {request.Password}");
            Console.WriteLine($"Stored hash: {user.PasswordHash}");
            Console.WriteLine($"Password match: {BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash)}");

            if (!BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash))
                return Unauthorized();

            var token = _jwtTokenGenerator.Generate(user);
            return Ok(new { token });
        }
    }
}