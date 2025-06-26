using Travel.API.Data;
using Travel.API.Models;
using Microsoft.AspNetCore.Http;

namespace Travel.API.Services
{
    public static class LoggingService
    {
        public static async Task LogAction(ApplicationDbContext context, HttpContext httpContext, string action, string entity, int entityId)
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

            context.Logs.Add(log);
            await context.SaveChangesAsync();
        }
    }
}
