using System.ComponentModel.DataAnnotations;

namespace HRMSPOC.WEB.DTOs
{
    public class AuthDTO
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
    }
}
