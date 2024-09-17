using System.ComponentModel.DataAnnotations;

namespace HRMSPOC.WEB.DTOs
{
    public class AuthDTO
    {
        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email address")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password is required")]
        [MinLength(6, ErrorMessage = "Password must be at least 6 characters long")]
        public string Password { get; set; }

        // This is for registration, not required during login
        public string ConfirmPassword { get; set; }
    }
}
