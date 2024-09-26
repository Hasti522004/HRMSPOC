using HRMSPOC.WEB.Models;
using Newtonsoft.Json;

namespace HRMSPOC.WEB.Services
{
    public class OrganizationService
    {
        private readonly HttpClient _httpClient;

        public OrganizationService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<OrganizationViewModel>> GetOrganizationsAsync()
        {
            var response = await _httpClient.GetStringAsync("api/Organization");
            return JsonConvert.DeserializeObject<List<OrganizationViewModel>>(response);
        }

        public async Task<OrganizationViewModel> GetOrganizationByIdAsync(Guid id)
        {
            var response = await _httpClient.GetStringAsync($"api/Organization/{id}");
            return JsonConvert.DeserializeObject<OrganizationViewModel>(response);
        }

        public async Task<bool> CreateOrganizationAsync(OrganizationViewModel organization)
        {
            var response = await _httpClient.PostAsJsonAsync("api/Organization", organization);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> UpdateOrganizationAsync(Guid id, OrganizationViewModel organization)
        {
            var response = await _httpClient.PutAsJsonAsync($"api/Organization/{id}", organization);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> DeleteOrganizationAsync(Guid id)
        {
            var response = await _httpClient.DeleteAsync($"api/Organization/{id}");
            return response.IsSuccessStatusCode;
        }
    }
}
