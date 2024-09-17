using HRMSPOC.API.DTOs;

namespace HRMSPOC.API.Repositories.Interfaces
{
    public interface IAuthRepository
    {
        Task<string> RegisterAsync(AuthDTO registerDto, string role);
        Task<string> LoginAsync(AuthDTO loginDto);
        Task<bool> UserExistsAsync(string email);
    }
}
