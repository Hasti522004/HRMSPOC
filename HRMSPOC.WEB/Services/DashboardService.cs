using HRMSPOC.WEB.ViewModel;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;

namespace HRMSPOC.WEB.Services
{
    public class DashboardService
    {
        private readonly HttpClient _httpClient;

        public DashboardService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<IEnumerable<UserViewModel>> GetUsersByOrganizationIdAsync(Guid organizationId)
        {
            var response = await _httpClient.GetAsync($"https://localhost:7095/api/user/organization/{organizationId}");
            if (response.IsSuccessStatusCode)
            {
                var responseContent = await response.Content.ReadAsStringAsync();
                Console.WriteLine(responseContent); // Log for debugging

                try
                {
                    using var jsonDocument = JsonDocument.Parse(responseContent);
                    var users = jsonDocument.RootElement.GetProperty("$values");

                    var userViewModels = new List<UserViewModel>();
                    foreach (var user in users.EnumerateArray())
                    {
                        userViewModels.Add(new UserViewModel
                        {
                            Id = user.GetProperty("id").GetString(),
                            FirstName = user.GetProperty("firstName").GetString(),
                            LastName = user.GetProperty("lastName").GetString(),
                            Email = user.GetProperty("email").GetString(),
                            PhoneNumber = user.GetProperty("phoneNumber").GetString(),
                            Address = user.GetProperty("address").GetString(),
                            UserName = user.GetProperty("userName").GetString(),
                            RoleName = user.GetProperty("roleName").GetString()
                        });
                    }

                    return userViewModels;
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"JSON deserialization error: {ex.Message}");
                    // You can also log the responseContent here if needed
                }
            }

            return new List<UserViewModel>();
        }
        public async Task<bool> CreateUserAsync(UserViewModel newUser)
        {
            var response = await _httpClient.PostAsJsonAsync("https://localhost:7095/api/user/{newUser.RoleName}", newUser);
            return response.IsSuccessStatusCode;
        }

        // Edit User
        public async Task<bool> EditUserAsync(UserViewModel updatedUser)
        {
            var response = await _httpClient.PutAsJsonAsync($"https://localhost:7095/api/user", updatedUser);
            return response.IsSuccessStatusCode;
        }

        // Delete User
        public async Task<bool> DeleteUserAsync(Guid userId)
        {
            var response = await _httpClient.DeleteAsync($"https://localhost:7095/api/user/{userId}");
            return response.IsSuccessStatusCode;
        }
    }
}
