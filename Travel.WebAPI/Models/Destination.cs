using System.ComponentModel.DataAnnotations;

namespace Travel.API.Models
{
    public class Destination
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Destination name is required")]
        [StringLength(100, ErrorMessage = "Destination name cannot be longer than 100 characters")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Country is required")]
        [StringLength(100, ErrorMessage = "Country name cannot be longer than 100 characters")]
        public string Country { get; set; }

        public ICollection<Trip> Trips { get; set; } = new List<Trip>();
    }
}
