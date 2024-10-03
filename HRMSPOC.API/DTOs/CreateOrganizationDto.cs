namespace HRMSPOC.API.DTOs
{
    public class CreateOrganizationDto
    {
        public string Name { get; set; }
        public string Address { get; set; }
        public bool IsDeleted { get; set; } = false;
    }
}
