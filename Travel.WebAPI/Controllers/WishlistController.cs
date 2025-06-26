using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Travel.API.Data;
using Travel.API.Dtos;
using Travel.API.Models;
using System.Diagnostics;
using Travel.WebAPI.Dtos;
using System;
using Travel.API.Services;
using Humanizer;

namespace Travel.API.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class WishlistController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public WishlistController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/wishlist
        [HttpGet]
        public async Task<ActionResult<IEnumerable<WishlistReadDto>>> GetWishlist()
        {
            return await _context.Wishlists
                .Include(w => w.Trip)
                .Select(w => new WishlistReadDto
                {
                    Id = w.Id,
                    TripId = w.TripId,
                    TripName = w.Trip.Name,
                    TripDescription = w.Trip.Description,
                    TripPrice = w.Trip.Price,
                    DesiredDateFrom = w.DesiredDateFrom,
                    DesiredDateTo = w.DesiredDateTo,
                    CreatedAt = w.CreatedAt
                })
                .ToListAsync();
        }


        // GET: api/wishlist/admin
        [HttpGet("admin")]
        public async Task<ActionResult<IEnumerable<WishlistReadAdminDto>>> GetWishlistAdmin()
        {
            return await _context.Wishlists
                .Include(w => w.Trip)
                .Include(w => w.User)
                .Select(w => new WishlistReadAdminDto
                {
                    Id = w.Id,
                    TripId = w.TripId,
                    TripName = w.Trip.Name,
                    TripDescription = w.Trip.Description,
                    TripPrice = w.Trip.Price,
                    DesiredDateFrom = w.DesiredDateFrom,
                    DesiredDateTo = w.DesiredDateTo,
                    User = w.User.FirstName + " " + w.User.LastName,
                    CreatedAt = w.CreatedAt
                })
                .ToListAsync();
        }


        // GET: api/wishlist/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Wishlist>> GetWishlist(int id)
        {
            var userIdClaim = User.FindFirst("userId")?.Value;
            if (userIdClaim == null || !int.TryParse(userIdClaim, out int userId))
            {
                return Unauthorized("Invalid user ID");
            }

            var wishlist = await _context.Wishlists
                .Include(w => w.Trip)
                .FirstOrDefaultAsync(w => w.Id == id && w.UserId == userId);

            if (wishlist == null)
            {
                return NotFound();
            }

            return wishlist;
        }

        // POST: api/wishlist
        [HttpPost]
        public async Task<ActionResult<WishlistReadDto>> PostWishlist(WishlistCreateDto dto)
        {
            var userIdClaim = User.FindFirst("userId")?.Value;
            if (userIdClaim == null || !int.TryParse(userIdClaim, out int userId))
            {
                return Unauthorized("Invalid user ID");
            }

            var exists = await _context.Wishlists
                .AnyAsync(w => w.UserId == userId && w.TripId == dto.TripId);

            if (exists)
            {
                return Conflict("Trip already exists in wishlist.");
            }

            var wishlist = new Wishlist
            {
                UserId = userId,
                TripId = dto.TripId,
                DesiredDateFrom = dto.DesiredDateFrom,
                DesiredDateTo = dto.DesiredDateTo,
                CreatedAt = DateTime.UtcNow
            };

            _context.Wishlists.Add(wishlist);
            await _context.SaveChangesAsync();

            // Reload with Trip
            var fullWishlist = await _context.Wishlists
                .Include(w => w.Trip)
                .FirstOrDefaultAsync(w => w.Id == wishlist.Id);

            Debug.WriteLine($"full wishilist ");
            // Return DTO with Trip info
            var result = new WishlistReadDto
            {
                Id = fullWishlist.Id,
                TripId = fullWishlist.TripId,
                TripName = fullWishlist.Trip?.Name,
                TripDescription = fullWishlist.Trip?.Description,
                TripPrice = fullWishlist.Trip?.Price ?? 0,
                DesiredDateFrom = fullWishlist.DesiredDateFrom,
                DesiredDateTo = fullWishlist.DesiredDateTo,
                CreatedAt = fullWishlist.CreatedAt
            };

            // logging
            await LoggingService.LogAction(_context, HttpContext, "Create", "Wishlist", dto.TripId);

            return CreatedAtAction(nameof(GetWishlist), new { id = result.Id }, result);
        }



        // DELETE: api/wishlist/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteWishlist(int id)
        {
            var userIdClaim = User.FindFirst("userId")?.Value;
            if (userIdClaim == null || !int.TryParse(userIdClaim, out int userId))
            {
                return Unauthorized("Invalid user ID");
            }

            var wishlist = await _context.Wishlists
                .FirstOrDefaultAsync(w => w.Id == id && w.UserId == userId);

            if (wishlist == null)
            {
                return NotFound();
            }

            _context.Wishlists.Remove(wishlist);
            await _context.SaveChangesAsync();

            // logging
            await LoggingService.LogAction(_context, HttpContext, "Delete", "Wishlist", id);

            return NoContent();
        }
    }
}