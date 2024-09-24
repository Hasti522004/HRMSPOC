using HRMSPOC.API.Models;

namespace HRMSPOC.API.Services.Interface
{
    public interface IOrganizationService
    {
        Task<IEnumerable<Organization>> GetOrganizationsAsync();
        Task<Organization> GetOrganizationByIdAsync(Guid id);
        Task<Organization> CreateOrganizationAsync(Organization organization);
        Task UpdateOrganizationAsync(Organization organization);
        Task DeleteOrganizationAsync(Guid id);
        Task<Organization> CreateOrganizationWithAdminAsync(Organization organization, Guid superAdminId);
    }
}
