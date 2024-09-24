using System;

namespace HRMSPOC.API.Models
{
    public class UserOrganization
    {
        public string UserId { get; set; }  // Still a string because IdentityUser uses string Id
        public Guid OrganizationId { get; set; }  // Guid for Organization

        public ApplicationUser ApplicationUser { get; set; }
        public Organization Organization { get; set; }
    }
}
