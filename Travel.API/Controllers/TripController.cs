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

        // GET Endpoint:
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
    }
}
