using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Travel.API.Data;
using Travel.API.Models;

namespace Travel.API.Controllers
{
    // secure authorization, use JWT
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class ApplicationUserController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public ApplicationUserController(ApplicationDbContext context)
        {
            _context = context;
        }

        /*
         * Use **ActionResult<T>** when you're returning an object, and you still want to have flexibility to return error responses 
         * (NotFound, BadRequest, etc.)
         */
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ApplicationUser>>> GetUsers()
        {
            return await _context.ApplicationUsers.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ApplicationUser>> GetUser(int id)
        {
            var user = await _context.ApplicationUsers.FindAsync(id);

            if (user == null) return NotFound();

            return user;
        }

        [HttpPost]
        public async Task<ActionResult<ApplicationUser>> PostUser(ApplicationUser user)
        {
            _context.ApplicationUsers.Add(user);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetUser), new { id = user.Id }, user);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutUser(int id, ApplicationUser user)
        {
            if (id != user.Id) return BadRequest();

            _context.Entry(user).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var user = await _context.ApplicationUsers.FindAsync(id);
            if (user == null) return NotFound();

            _context.ApplicationUsers.Remove(user);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
