namespace HRMSPOC.WEB.ViewModel
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
        public string? Password { get; set;}

        // Additional properties for actions
        //public bool CanEdit { get; set; }      // Determines if the user can be edited
        //public bool CanDelete { get; set; }    // Determines if the user can be deleted
    }
}
