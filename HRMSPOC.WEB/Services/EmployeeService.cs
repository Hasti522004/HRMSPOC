using HRMSPOC.API.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace HRMSPOC.WEB.Services
{
    public class EmployeeService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        public EmployeeService(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<ActionResult<IEnumerable<Employee>>> GetEmployeeAsync()
        {
            var client = _httpClientFactory.CreateClient("HRMSAPI");
            var response = await client.GetAsync("api/Employee");

            if (response.IsSuccessStatusCode)
            {
                var employees = await response.Content.ReadFromJsonAsync<IEnumerable<Employee>>();
                return employees.ToList() ?? new List<Employee>();
            }
            throw new Exception($"An error occurred while fetching employees: {response.ReasonPhrase}");
        }

        public async Task<Employee> GetEmployeeById(int id)
        {
            var client = _httpClientFactory.CreateClient("HRMSAPI");
            var response = await client.GetAsync("api/Employee/{id}");
            if (response.IsSuccessStatusCode)
            {
                var employee = await response.Content.ReadFromJsonAsync<Employee>();    
                return employee ?? new Employee();
            }
            throw new Exception($"An error occurred while fetching employee with ID {id}: {response.ReasonPhrase}");
        }

        public async Task CreateEmployee(Employee employee)
        {
            var client = _httpClientFactory.CreateClient("HRMSAPI");
            var response = await client.PostAsJsonAsync("api/Employee",employee);
            if (!response.IsSuccessStatusCode)
            {
                throw new HttpRequestException($"An error occurred while creating employee: {response.ReasonPhrase}");
            }
        }

        public async Task UpdateEmployee(Employee employee)
        {
            var client = _httpClientFactory.CreateClient("HRMSAPI");
            
        }
    }
}
