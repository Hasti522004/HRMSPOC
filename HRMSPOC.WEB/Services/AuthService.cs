using HRMSPOC.WEB.DTOs;

namespace HRMSPOC.WEB.Services
{
    public class AuthService
    {
        private readonly HttpClient _httpClient;
        public AuthService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        public async Task<string> RegisterAsync(RegisterDTO registerDTO)
        {
            var response = await _httpClient.PostAsJsonAsync("api/Auth/register", registerDTO);
            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadFromJsonAsync<TokenResponse>();
                return result.Token;
            }
            return null;
        }
        public async Task<string> LoginAsync(LoginDTO loginDto)
        {
            var response = await _httpClient.PostAsJsonAsync("api/Auth/login", loginDto);

            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadFromJsonAsync<TokenResponse>();
                return result.Token;
            }
            return null;
        }
        public class TokenResponse
        {
            public string Token { get; set; }
        }
    }
}
