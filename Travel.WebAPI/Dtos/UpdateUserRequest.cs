namespace Travel.API.Dtos
{
    public class UpdateUserRequest
    {

        public string Email { get; set; } = string.Empty;
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? PhoneNumber { get; set; }
        
    }
}
