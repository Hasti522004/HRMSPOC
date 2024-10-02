namespace HRMSPOC.API.DTOs
{
    public class ApplicationUserDto
    {
        public string? Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string? Address { get; set; }
        public DateTime CreatedAt { get; set; }
        public bool IsDeleted { get; set; } = false;
        public string? Password { get; set; }
        public string Email { get; set; }
        public Guid CreatedBy { get; set; }
        public string? PhoneNumber { get; set; }

        // Optionally, you can include the organization data if needed
        public ICollection<UserOrganizationDto> UserOrganizations { get; set; } = new List<UserOrganizationDto>();
    }
}