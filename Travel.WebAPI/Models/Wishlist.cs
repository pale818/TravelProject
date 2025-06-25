namespace Travel.API.Models
{
    public class Wishlist
    {
        public int Id { get; set; }

        public int UserId { get; set; }
        public int TripId { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? DesiredDateFrom { get; set; }
        public DateTime? DesiredDateTo { get; set; }

        public ApplicationUser User { get; set; }
        public Trip Trip { get; set; }
    }
}
