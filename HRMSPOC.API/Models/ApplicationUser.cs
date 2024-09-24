using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace HRMSPOC.API.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string? Address { get; set; }
        public Guid CreatedBy { get; set; }  // Assuming CreatedBy refers to another user's ID
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [JsonIgnore]
        public ICollection<UserOrganization> UserOrganizations { get; set; } = new List<UserOrganization>();
    }
}
