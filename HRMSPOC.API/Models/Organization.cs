using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace HRMSPOC.API.Models
{
    public class Organization
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Address { get; set; }

        //Navigation Property
        public ICollection<Employee> Employees { get; set; } = new List<Employee>();
    }
}
