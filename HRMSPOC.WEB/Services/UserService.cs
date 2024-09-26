using HRMSPOC.WEB.DTOs;
using HRMSPOC.WEB.Models;
using Newtonsoft.Json;

namespace HRMSPOC.WEB.Services
{
    public class UserService
    {
        private readonly HttpClient _httpClient;

        public UserService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<ApplicationUserViewModel>> GetUsersByOrganizationIdAsync(Guid organizationId)
        {
            var response = await _httpClient.GetStringAsync($"api/User/createdby/{organizationId}");
            return JsonConvert.DeserializeObject<List<ApplicationUserViewModel>>(response);
        }

        public async Task<ApplicationUserDto> GetUserByIdAsync(string id)
        {
            var response = await _httpClient.GetAsync($"api/User/{id}");
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<ApplicationUserDto>(content);
            }
            return null; // Return null if the user is not found
        }

        public async Task<bool> CreateUserAsync(ApplicationUserViewModel user)
        {
            var response = await _httpClient.PostAsJsonAsync("api/User", user);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> UpdateUserAsync(ApplicationUserDto userDto)
        {
            var response = await _httpClient.PutAsJsonAsync("api/User", userDto);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> DeleteUserAsync(Guid id)
        {
            var response = await _httpClient.DeleteAsync($"api/User/{id}");
            return response.IsSuccessStatusCode;
        }
    }
}
