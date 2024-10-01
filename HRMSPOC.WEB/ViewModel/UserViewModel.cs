﻿namespace HRMSPOC.WEB.ViewModel
{
    public class UserViewModel
    {
        public string? Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Address { get; set; }
        public string? UserName { get; set; }
        public string? RoleName { get; set; }
        public string? PasswordHash { get; set;}

        public string? CreatedById { get; set; }
    }
}
