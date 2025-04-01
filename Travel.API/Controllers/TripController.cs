/*
 * Trip controller
 */

// Gives access to MVC features, especially [ApiController], ControllerBase, and ActionResult.
using Microsoft.AspNetCore.Mvc;
// Required for ToListAsync() and other EF Core async database methods.
using Microsoft.EntityFrameworkCore;
using Travel.API.Data;
using Travel.API.Models;

namespace Travel.API.Controllers
{
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
        [HttpGet] // Marks the method as handling HTTP GET requests for /api/trip.
        /*
         * The method is asynchronous.
	     • It returns a list of trips (IEnumerable<Trip>) wrapped in an ActionResult (so you can return status codes later, if needed).
         */
        public async Task<ActionResult<IEnumerable<Trip>>> GetTrips()
        {
            // await _context.Trips.ToListAsync(); - Uses Entity Framework Core to asynchronously fetch all records from the Trips table.
            // Returns the list of trips as JSON.
            return await _context.Trips.ToListAsync();
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
            _context.Trips.Add(trip);
            await _context.SaveChangesAsync();

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
            var trip = await _context.Trips.FindAsync(id);
            if (trip == null)
            {
                return NotFound();
            }

            _context.Trips.Remove(trip);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
