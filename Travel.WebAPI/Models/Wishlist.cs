using System;
using System.ComponentModel.DataAnnotations;

namespace Travel.API.Models
{
    public class Wishlist
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "User is required")]
        public int UserId { get; set; }

        [Required(ErrorMessage = "Trip is required")]
        public int TripId { get; set; }

        [Required]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime? DesiredDateFrom { get; set; }

        public DateTime? DesiredDateTo { get; set; }

        [Required]
        public ApplicationUser User { get; set; }

        [Required]
        public Trip Trip { get; set; }
    }
}
