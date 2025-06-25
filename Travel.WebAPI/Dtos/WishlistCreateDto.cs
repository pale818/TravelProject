namespace Travel.API.Dtos
{
    public class WishlistCreateDto
    {
        public int TripId { get; set; }
        public DateTime? DesiredDateFrom { get; set; }
        public DateTime? DesiredDateTo { get; set; }
    }
}
