using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Travel.API.Data;
using Travel.API.Dtos;
using Travel.API.Models;

namespace Travel.API.Controllers
{
    // secure authorization, use JWT
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class GuideController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public GuideController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/guide
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Guide>>> GetGuides()
        {
            return await _context.Guides.ToListAsync();
        }

        // GET: api/guide/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<Guide>> GetGuide(int id)
        {
            var guide = await _context.Guides.FindAsync(id);
            if (guide == null)
            {
                return NotFound();
            }
            return guide;
        }

        // POST: api/guide
        [HttpPost]
        public async Task<ActionResult<Guide>> CreateGuide(Guide guide)
        {
            if (_context.Guides.Any(g => g.Email == guide.Email && g.FirstName == guide.FirstName && g.LastName == guide.LastName))
            {
                ModelState.AddModelError("name", "A Guide with this Email already exists.");
                return ValidationProblem(ModelState);
            }

            _context.Guides.Add(guide);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetGuide), new { id = guide.Id }, guide);
        }

        // PUT: api/guide/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateGuide(int id, Guide guide)
        {
            if (id != guide.Id)
            {
                return BadRequest();
            }

            _context.Entry(guide).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Guides.Any(e => e.Id == id))
                {
                    return NotFound();
                }
                throw;
            }

            return NoContent();
        }

        // DELETE: api/guide/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteGuide(int id)
        {
            var guide = await _context.Guides.FindAsync(id);
            if (guide == null)
            {
                return NotFound();
            }

            _context.Guides.Remove(guide);
            await _context.SaveChangesAsync();
            return NoContent();
        }


        // delete guides form some trip
        [HttpPut("{id}/guides")]
        public async Task<IActionResult> UpdateTripGuides(int id, [FromBody] TripGuideUpdate dto)
        {
            var trip = await _context.Trips.Include(t => t.Guides).FirstOrDefaultAsync(t => t.Id == id);
            if (trip == null) return NotFound();

            // update with possibly empty list — will remove all guides
            var guides = await _context.Guides.Where(g => dto.GuideIds.Contains(g.Id)).ToListAsync();
            trip.Guides = guides;

            await _context.SaveChangesAsync();
            return NoContent();
        }

    }
}