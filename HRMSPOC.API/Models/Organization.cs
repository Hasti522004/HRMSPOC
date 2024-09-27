﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace HRMSPOC.API.Models
{
    public class Organization
    {
        [Key]
        public Guid Id { get; set; }  // Use Guid for ID
        [Required]
        public string Name { get; set; }
        [Required]
        public string Address { get; set; }
        public bool isdelete { get; set; } = false;
        // Navigation Property
        [JsonIgnore]
        public ICollection<UserOrganization> UserOrganizations { get; set; } = new List<UserOrganization>();
    }
}
