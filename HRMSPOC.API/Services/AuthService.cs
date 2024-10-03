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
            if (loginDto == null)
            {
                throw new ArgumentNullException(nameof(loginDto), "Login data cannot be null.");
            }

            if (string.IsNullOrWhiteSpace(loginDto.Email))
            {
                throw new ArgumentException("Email cannot be null or empty.", nameof(loginDto.Email));
            }

            if (string.IsNullOrWhiteSpace(loginDto.Password))
            {
                throw new ArgumentException("Password cannot be null or empty.", nameof(loginDto.Password));
            }

            return await _authRepository.LoginAsync(loginDto);
        }
    }
}
