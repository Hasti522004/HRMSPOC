namespace HRMSPOC.API.DTOs
{
    public class OrganizationDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public bool IsDeleted { get; set; }

        // Optionally, you can include the user organizations if needed
        public ICollection<UserOrganizationDto> UserOrganizations { get; set; } = new List<UserOrganizationDto>();
    }
}
