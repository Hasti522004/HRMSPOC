using HRMSPOC.API.DTOs;
using HRMSPOC.API.Repositories.Interfaces;
using HRMSPOC.API.Services.Interface;

namespace HRMSPOC.API.Services
{
    public class AuthService : IAuthService
    {
        private readonly IAuthRepository _authRepository;
        public AuthService(IAuthRepository authRepository)
        {
            _authRepository = authRepository;
        }
        public async Task<string> LoginAsync(AuthDTO loginDto)
        {
            return await _authRepository.LoginAsync(loginDto);
        }
    }
}
