using HRMSPOC.API.Models;

namespace HRMSPOC.API.Services.Interface
{
    public interface IOrganizationService
    {
        Task<IEnumerable<Organization>> GetOrganizationsAsync();
        Task<Organization> GetOrganizationByIdAsync(int id);
        Task CreateOrganizationAsync(Organization organization);
        Task UpdateOrganizationAsync(Organization organization);
        Task DeleteOrganizationAsync(int id);
    }
}
