using System.ComponentModel.DataAnnotations;

namespace Travel.API.Dtos
{
    public class WishlistCreateDto
    {
        [Required]
        public int TripId { get; set; }

        public DateTime? DesiredDateFrom { get; set; }
        public DateTime? DesiredDateTo { get; set; }
    }
}
