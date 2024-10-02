using HRMSPOC.API.DTOs;
using HRMSPOC.API.Models;

namespace HRMSPOC.API.Repositories.Interfaces
{
    public interface IOrganizationRepository
    {
        Task<IEnumerable<OrganizationDto>> GetOrganizationsAsync();
        Task<OrganizationDto> GetOrganizationByIdAsync(Guid id);
        Task<OrganizationDto> CreateOrganizationAsync(OrganizationDto organization);
        Task UpdateOrganizationAsync(OrganizationDto organization);
        Task DeleteOrganizationAsync(Guid id);
        Task CreateAdminUserAsync(ApplicationUserDto adminUser, Guid organizationId);
        Task<bool> IsOrganizationExists(Guid id);
        Task CreateRoleIfNotExistsAsync(string roleName);
        Task AssignRoleToUserAsync(ApplicationUserDto adminUser, string roleName);
    }
}
