using HRMSPOC.API.DTOs;

namespace HRMSPOC.API.Services.Interface
{
    public interface IAuthService
    {
        Task<string> RegisterAsync(AuthDTO registerDto);
        Task<string> LoginAsync(AuthDTO loginDto);
    }
}
