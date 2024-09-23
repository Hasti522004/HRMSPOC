﻿using System.ComponentModel.DataAnnotations;

namespace HRMSPOC.WEB.Models
{
    public class Employee
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public string PhoneNumber { get; set; }
        public int OrganizationId { get; set; }
        public Organization Organization { get; set; }
    }
}
