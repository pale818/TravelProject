using System.ComponentModel.DataAnnotations;

namespace Travel.API.Models
{
    /// <summary>
    /// This is trip model and it corresponds with the Trip table
    /// </summary>
    public class Trip
    {
        public int Id { get; set; }

        //[Required(ErrorMessage = "Trip name is required")]
        //[StringLength(100, ErrorMessage = "Trip name cannot be longer than 100 characters")]
        public string Name { get; set; }
        public string? Description { get; set; }

        //[Range(0, double.MaxValue, ErrorMessage = "Price must be non-negative")]
        public decimal Price { get; set; }

        // part responsible for the bridge table between Trip and Guide
        public ICollection<Guide> Guides { get; set; } = new List<Guide>();

        public ICollection<Destination> Destinations { get; set; } = new List<Destination>();
    }
}
