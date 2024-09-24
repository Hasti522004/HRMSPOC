using HRMSPOC.API.DTOs;

namespace HRMSPOC.API.Services.Interface
{
    public interface IAuthService
    { 
        Task<string> LoginAsync(AuthDTO loginDto);
    }
}
