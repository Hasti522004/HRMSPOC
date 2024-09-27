using System;

namespace HRMSPOC.API.Models
{
    public class UserOrganization
    {
        public string UserId { get; set; } 
        public Guid OrganizationId { get; set; }

        public ApplicationUser ApplicationUser { get; set; }
        public Organization Organization { get; set; }
    }
}
