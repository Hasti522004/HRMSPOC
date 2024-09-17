using Microsoft.AspNetCore.Identity;

namespace HRMSPOC.API.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string Role { get; set; }
    }
}
