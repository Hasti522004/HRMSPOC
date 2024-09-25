using HRMSPOC.API.Models;

namespace HRMSPOC.API.Repositories.Interfaces
{
    public interface IOrganizationRepository
    {
        Task<IEnumerable<Organization>> GetOrganizationsAsync();
        Task<Organization> GetOrganizationByIdAsync(Guid id);
        Task<Organization> CreateOrganizationAsync(Organization organization);
        Task UpdateOrganizationAsync(Organization organization);
        Task DeleteOrganizationAsync(Guid id);
        Task CreateAdminUserAsync(ApplicationUser adminUser, Guid organizationId);
        Task<bool> IsOrganizationExists(Guid id);
    }
}
