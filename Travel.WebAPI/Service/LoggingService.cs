using Travel.API.Data;
using Travel.API.Models;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace Travel.API.Services
{
    public interface ILoggingService
    {
        Task LogAction(HttpContext httpContext, string action, string entity, int entityId);
    }

    public class LoggingService : ILoggingService
    {
        private readonly ApplicationDbContext _context;

        public LoggingService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task LogAction(HttpContext httpContext, string action, string entity, int entityId)
        {
            var userIdClaim = httpContext.User.FindFirst("userId")?.Value;
            if (userIdClaim == null || !int.TryParse(userIdClaim, out int userId))
                return; // skip if no user

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
}
