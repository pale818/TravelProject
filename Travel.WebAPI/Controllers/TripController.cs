/*
 * Trip controller
 */

// Gives access to MVC features, especially [ApiController], ControllerBase, and ActionResult.
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
// Required for ToListAsync() and other EF Core async database methods.
using Microsoft.EntityFrameworkCore;
using Travel.API.Data;
using Travel.API.Models;
using Travel.API.Dtos;
using System;

namespace Travel.API.Controllers
{
    // authorize with JWT
    [Authorize]
    [ApiController] // Tells ASP.NET Core this is a Web API controller, not an MVC one.

    /*
    * Sets the base route for this controller.
	• "[controller]" is a placeholder that resolves to trip, based on the class name TripController.
	• So this controller will respond to:
    * /api/trip
    */
    [Route("api/[controller]")]
    public class TripController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public TripController(ApplicationDbContext context)
        {
            _context = context;
        }


        // **********************************************************

        // GET: api/trip
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Trip>>> GetTrips()
        {
            return await _context.Trips.ToListAsync();
        }


        // GET: api/trip
        [HttpGet("guides")] // Marks the method as handling HTTP GET requests for /api/trip/guides.
        /*
         * The method is asynchronous.
	     • It returns a list of trips (IEnumerable<Trip>) wrapped in an ActionResult (so you can return status codes later, if needed).
         */
        public async Task<ActionResult<IEnumerable<Trip>>> GetTripsGuides()
        {
            // await _context.Trips.ToListAsync(); - Uses Entity Framework Core to asynchronously fetch all records from the Trips table.
            // Returns the list of trips as JSON.
            //return await _context.Trips.ToListAsync();

            var trips = await _context.Trips
                .Include(t => t.Guides)
                .ToListAsync();

            var result = trips.Select(t => new TripDto
            {
                Id = t.Id,
                Name = t.Name,
                Guides = t.Guides.Select(g => new GuideDto
                {
                    Id = g.Id,
                    FirstName = g.FirstName,
                    LastName = g.LastName
                }).ToList()
            }).ToList();

            return Ok(result);
        }

        /*
         * When you make a GET request to:
         * https://localhost:port/api/trip
         * It will:
	     • Query the Trips table from your database
	     • Return the data as JSON
	     • Show it in Swagger (or allow use by frontend like JavaScript)
         */



        // **********************************************************
        // GET: api/trip/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<Trip>> GetTrip(int id)
        {
            var trip = await _context.Trips.FindAsync(id);

            if (trip == null)
            {
                return NotFound();
            }

            return trip;
        }


        // **********************************************************
        // POST: api/trip
        [HttpPost]
        public async Task<ActionResult<Trip>> CreateTrip(Trip trip)
        {
            System.Diagnostics.Debug.WriteLine($"TRIP:: {trip.Name}");

            if (_context.Trips.Any(t => t.Name == trip.Name))
            {
                ModelState.AddModelError("name", "A Trip with this name already exists.");
                return ValidationProblem(ModelState);
            }

            _context.Trips.Add(trip);
            await _context.SaveChangesAsync();

            // logging
            await LogAction("Create", "Trip", trip.Id);


            return CreatedAtAction(nameof(GetTrip), new { id = trip.Id }, trip);
        }


        // **********************************************************
        /*	•	Verifies the id in URL matches trip.Id from body
	    •	Marks the trip as modified
	    •	Handles update conflicts
	    •	Returns:
	    •	204 No Content if successful
	    •	404 if the trip doesn’t exist
	    •	400 if ID mismatch
        * 
        */
        // PUT: api/trip/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTrip(int id, Trip trip)
        {
            if (id != trip.Id)
            {
                return BadRequest();
            }

            _context.Entry(trip).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TripExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            // logging
            await LogAction("Update", "Trip", trip.Id);


            return NoContent();
        }

        private bool TripExists(int id)
        {
            return _context.Trips.Any(e => e.Id == id);
        }


        // **********************************************************
        /*
         * 	Finds the trip by ID
	     •	If not found, returns 404 Not Found
	     •	If found, deletes and returns 204 No Content
         */
        // DELETE: api/trip/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTrip(int id)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                var trip = await _context.Trips.FindAsync(id);
                if (trip == null)
                {
                    return NotFound();
                }

                // Delete from TripGuide (shadow table)
                var tripGuides = await _context.Set<Dictionary<string, object>>("TripGuide")
                    .Where(tg => (int)tg["TripId"] == id)
                    .ToListAsync();
                _context.RemoveRange(tripGuides);

                // Delete from TripDestination (shadow table)
                var tripDestinations = await _context.Set<Dictionary<string, object>>("TripDestination")
                    .Where(td => (int)td["TripId"] == id)
                    .ToListAsync();
                _context.RemoveRange(tripDestinations);

                // Delete from Wishlist
                var wishlists = await _context.Wishlists
                    .Where(w => w.TripId == id)
                    .ToListAsync();
                _context.Wishlists.RemoveRange(wishlists);

                // Delete the Trip itself
                _context.Trips.Remove(trip);

                // Save all changes
                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                // Log the deletion
                await LogAction("Delete", "Trip", trip.Id);

                return NoContent();
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                return StatusCode(500, $"Error deleting trip and related data: {ex.Message}");
            }
        }



        [HttpPut("{id}/guides")]
        public async Task<IActionResult> UpdateTripGuides(int id, [FromBody] TripGuideUpdate dto)
        {
            var trip = await _context.Trips.Include(t => t.Guides).FirstOrDefaultAsync(t => t.Id == id);
            if (trip == null) return NotFound();

            var guides = await _context.Guides.Where(g => dto.GuideIds.Contains(g.Id)).ToListAsync();
            trip.Guides = guides;

            await _context.SaveChangesAsync();
            return NoContent();
        }



        // FOR PAGINATION
        [HttpGet("search")]
        public async Task<ActionResult<IEnumerable<Trip>>> SearchTrips(
            [FromQuery] string? query = null,
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10)
        {
            if (page < 1 || pageSize < 1) return BadRequest("Invalid paging parameters.");

            var tripQuery = _context.Trips.AsQueryable();

            if (!string.IsNullOrWhiteSpace(query))
            {
                tripQuery = tripQuery.Where(t => t.Name.Contains(query) || (t.Description != null && t.Description.Contains(query)));
            }

            var totalCount = await tripQuery.CountAsync();
            var trips = await tripQuery
                .OrderBy(t => t.Id)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();


            var response = new
            {
                TotalCount = totalCount,
                Page = page,
                PageSize = pageSize,
                Data = trips,
            };

            return Ok(response);
        }

        // GET: api/trip/{id}/destinations
        [HttpGet("{id}/destinations")]
        public async Task<ActionResult<IEnumerable<DestinationDto>>> GetTripDestinations(int id)
        {
            var trip = await _context.Trips
                .Include(t => t.Destinations)
                .FirstOrDefaultAsync(t => t.Id == id);

            if (trip == null) return NotFound();

            var result = trip.Destinations.Select(d => new DestinationDto
            {
                Id = d.Id,
                Name = d.Name,
                Country = d.Country
            }).ToList();

            return Ok(result);
        }



        // PUT: api/trip/{id}/destinations
        [HttpPut("{id}/destinations")]
        public async Task<IActionResult> UpdateTripDestinations(int id, [FromBody] List<int> destinationIds)
        {
            var trip = await _context.Trips
                .Include(t => t.Destinations)
                .FirstOrDefaultAsync(t => t.Id == id);

            if (trip == null)
                return NotFound();

            var destinations = await _context.Destinations
                .Where(d => destinationIds.Contains(d.Id))
                .ToListAsync();

            trip.Destinations = destinations;
            await _context.SaveChangesAsync();

            return NoContent();
        }



        // FOR LOGGING TRIPS
        private async Task LogAction(string action, string entity, int entityId)
        {
            var userIdClaim = User.FindFirst("userId")?.Value;
            if (userIdClaim == null || !int.TryParse(userIdClaim, out int userId))
                return; // skip logging if userId not found

            var log = new Log
            {
                UserId = userId,
                Action = action,
                Entity = entity,
                EntityId = entityId,
                Timestamp = DateTime.Now
            };

            _context.Logs.Add(log);
            await _context.SaveChangesAsync();
        }


    }



    // DTOs for GUide Trip relation
    public class TripDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<GuideDto> Guides { get; set; } = new();
    }

    public class GuideDto
    {
        public int Id { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
    }
}
