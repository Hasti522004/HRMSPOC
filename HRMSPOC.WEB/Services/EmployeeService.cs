using HRMSPOC.WEB.DTOs;
using HRMSPOC.WEB.Models;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;

namespace HRMSPOC.WEB.Services
{
    public class EmployeeService
    {
        private readonly HttpClient _httpClient;

        public EmployeeService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<ApplicationUserViewModel>> GetUsersByHrIdAsync(Guid hrId)
        {
            var response = await _httpClient.GetStringAsync($"https://localhost:7095/api/User/createdby/{hrId}");
            return JsonConvert.DeserializeObject<List<ApplicationUserViewModel>>(response);
        }

        public async Task<ApplicationUserDto> GetUserByIdAsync(string id)
        {
            var response = await _httpClient.GetAsync($"https://localhost:7095/api/User/{id}");
            response.EnsureSuccessStatusCode();
            var responseContent = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<ApplicationUserDto>(responseContent);
        }

        public async Task<bool> CreateUserAsync(ApplicationUserViewModel user)
        {
            var response = await _httpClient.PostAsJsonAsync("https://localhost:7095/api/User", user);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> UpdateUserAsync(ApplicationUserDto userDto)
        {
            var response = await _httpClient.PutAsJsonAsync($"https://localhost:7095/api/User", userDto);
            return response.IsSuccessStatusCode;
        }

        public async Task<ApplicationUserViewModel> GetUserToDeleteAsync(Guid id)
        {
            var response = await _httpClient.GetAsync($"https://localhost:7095/api/User/{id}");
            response.EnsureSuccessStatusCode();
            var userContent = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<ApplicationUserViewModel>(userContent);
        }

        public async Task<bool> DeleteUserAsync(Guid id)
        {
            var response = await _httpClient.DeleteAsync($"https://localhost:7095/api/User/{id}");
            return response.IsSuccessStatusCode;
        }
    }
}
