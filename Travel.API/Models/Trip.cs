
/**
 *This is trip model and it corresponds with the Trip table
 */
namespace Travel.API.Models
{
    public class Trip
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public DateTime DateFrom { get; set; }
        public DateTime DateTo { get; set; }
        public decimal Price { get; set; }
        public int DestinationId { get; set; }

        // part responsible for the bridge table between Trip and Guide
        public ICollection<Guide> Guides { get; set; } = new List<Guide>();
        public ICollection<Destination> Destinations { get; set; } = new List<Destination>();
    }
}
