namespace HRMSPOC.API.Models
{
    public class Organization
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string CreatedBy { get; set; } // Admin Who Create the Organization
    }
}
