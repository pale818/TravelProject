using System;

namespace Travel.API.Models
{
    public class Log
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string? Action { get; set; }
        public string? Entity { get; set; }
        public int EntityId { get; set; }
        public DateTime Timestamp { get; set; } = DateTime.Now;
        public ApplicationUser? User { get; set; }
    }
}