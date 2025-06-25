namespace Travel.API.Models
{
    public class Destination
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Country { get; set; }

        public ICollection<Trip> Trips { get; set; } = new List<Trip>();

    }
}
