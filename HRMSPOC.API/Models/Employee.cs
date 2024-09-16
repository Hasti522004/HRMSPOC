using System.ComponentModel.DataAnnotations;

namespace HRMSPOC.API.Models
{
    public class Employee
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        [EmailAddress]
        [Required]
        public string Email { get; set; }
        public string Address { get; set; }
        [Phone]
        [Required]
        public string PhoneNumber { get; set; }
        public int OrganizationId { get; set; }
        public Organization Organization { get; set; }
    }
}
