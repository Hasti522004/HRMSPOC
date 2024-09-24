using HRMSPOC.API.DTOs;

namespace HRMSPOC.API.Repositories.Interfaces
{
    public interface IAuthRepository
    {
        Task<string> LoginAsync(AuthDTO loginDto);
    }
}
