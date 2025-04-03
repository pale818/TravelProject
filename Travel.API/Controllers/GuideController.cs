using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Travel.API.Data;
using Travel.API.Models;

namespace Travel.API.Controllers
{
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
    }
}