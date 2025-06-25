namespace Travel.WebAPI.Dtos
{
    public class WishlistReadAdminDto
    {
        public int Id { get; set; }
        public int TripId { get; set; }
        public string TripName { get; set; }
        public string? TripDescription { get; set; }
        public decimal TripPrice { get; set; }
        public DateTime? DesiredDateFrom { get; set; }
        public DateTime? DesiredDateTo { get; set; }
        public DateTime CreatedAt { get; set; }
        public string User { get; set; }

    }
}
