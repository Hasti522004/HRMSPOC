using HRMSPOC.WEB.DTOs;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;

namespace HRMSPOC.WEB.Services
{
    public class AuthService
    {
        private readonly HttpClient _httpClient;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public AuthService(HttpClient httpClient, IHttpContextAccessor httpContextAccessor)
        {
            _httpClient = httpClient;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<string> LoginAsync(LoginDTO loginDto)
        {
            var response = await _httpClient.PostAsJsonAsync("api/Auth/login", loginDto);

            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadFromJsonAsync<TokenResponse>();
                return result.Token;
            }
            return null; // Consider logging the failure
        }

        public async Task<string> HandleLoginToken(string token)
        {
            // Store token in session
            _httpContextAccessor.HttpContext.Session.SetString("JWTToken", token);

            // Decode the JWT token
            var handler = new JwtSecurityTokenHandler();
            var jwtToken = handler.ReadJwtToken(token);

            // Retrieve roles from claims
            var roles = jwtToken.Claims.Where(c => c.Type == ClaimTypes.Role).Select(c => c.Value).ToList();
            var userId = jwtToken.Claims.FirstOrDefault(c => c.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier")?.Value;

            if (!string.IsNullOrEmpty(userId))
            {
                _httpContextAccessor.HttpContext.Session.SetString("UserId", userId);
            }

            // Store roles in session
            _httpContextAccessor.HttpContext.Session.SetString("UserRoles", string.Join(",", roles));

            if (roles.Contains("SuperAdmin"))
            {
                return "SuperAdmin";
            }
            else
            {
                var response = await _httpClient.GetAsync($"https://localhost:7095/api/UserOrganization/organization/{userId}");
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    var organizationId = JsonConvert.DeserializeObject<OrganizationResponse>(content).OrganizationId;

                    // Assuming organizationId is a Guid
                    _httpContextAccessor.HttpContext.Session.Set("OrganizationId", organizationId.ToByteArray());
                    _httpContextAccessor.HttpContext.Session.SetString("CreatedById", userId);
                    return "User";
                }
            }

            return null; // No specific action
        }

        public class TokenResponse
        {
            public string Token { get; set; } // Change from Guid to string
        }
    }

    internal class OrganizationResponse
    {
        public Guid OrganizationId { get; set; } // Change from object to Guid
    }
}
