// Controllers/DestinationController.cs
using Azure.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Travel.API.Data;
using Travel.API.Models;
using Travel.API.Services;

namespace Travel.API.Controllers
{
    // secure authorization, use JWT
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class DestinationController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public DestinationController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/destination
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Destination>>> GetDestinations()
        {
            return await _context.Destinations.ToListAsync();
        }

        // GET: api/destination/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Destination>> GetDestination(int id)
        {
            var destination = await _context.Destinations.FindAsync(id);
            if (destination == null) return NotFound();
            return destination;
        }

        // POST: api/destination
        [HttpPost]
        public async Task<ActionResult<Destination>> PostDestination(Destination destination)
        {
            
            if (_context.Destinations.Any(d => d.Name == destination.Name))
            {
                ModelState.AddModelError("name", "A destination with this name already exists.");
                return ValidationProblem(ModelState);
            }


            _context.Destinations.Add(destination);
            await _context.SaveChangesAsync();

            // logging
            await LoggingService.LogAction(_context, HttpContext, "Create", "Destination", destination.Id);

            return CreatedAtAction(nameof(GetDestination), new { id = destination.Id }, destination);
        }

        // PUT: api/destination/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutDestination(int id, Destination destination)
        {
            if (id != destination.Id) return BadRequest();

            _context.Entry(destination).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Destinations.Any(e => e.Id == id))
                    return NotFound();
                throw;
            }

            // logging
            await LoggingService.LogAction(_context, HttpContext, "Update", "Destination", destination.Id);
            return NoContent();
        }

        // DELETE: api/destination/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDestination(int id)
        {
            /*var destination = await _context.Destinations.FindAsync(id);
            if (destination == null) return NotFound();

            _context.Destinations.Remove(destination);
            await _context.SaveChangesAsync();

            return NoContent();
            */

            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                var destination = await _context.Destinations.FindAsync(id);
                if (destination == null)
                {
                    return NotFound();
                }

                var tripDestinations = await _context.Set<Dictionary<string, object>>("TripDestination")
                   .Where(td => (int)td["DestinationId"] == id)
                   .ToListAsync();
                _context.RemoveRange(tripDestinations);
                _context.Destinations.Remove(destination);

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                // logging
                await LoggingService.LogAction(_context, HttpContext, "Delete", "Destination", destination.Id);
                return NoContent();
            }
            catch (Exception e)
            {
                await transaction.RollbackAsync();
                return StatusCode(500, $"Error deleting destination and related data: {e.Message}");
            }
        }
    }
}