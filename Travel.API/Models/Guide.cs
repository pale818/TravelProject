namespace Travel.API.Models
{
    public class Guide
    {
        public int Id { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string? Biography { get; set; }
        public string Email { get; set; } = string.Empty;

        // part responsible for the bridge table between rip and Guide
        public ICollection<Trip> Trips { get; set; } = new List<Trip>();

    }
}
