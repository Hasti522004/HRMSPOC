using HRMSPOC.API.Models;

namespace HRMSPOC.API.Services.Interface
{
    public interface IOrganizationService
    {
        Task<IEnumerable<Organization>> GetEmployeesAsync();
        Task<Organization> GetEmployeeByIdAsync(int id);
        Task CreateEmployeeAsync(Organization organization);
        Task UpdateEmployeeAsync(Organization organization);
        Task DeleteEmployeeAsync(int id);
    }
}
