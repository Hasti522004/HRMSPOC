using HRMSPOC.API.DTOs;
using HRMSPOC.API.Models;

namespace HRMSPOC.API.Services.Interface
{
    public interface IOrganizationService
    {
        Task<IEnumerable<OrganizationDto>> GetOrganizationsAsync();
        Task<OrganizationDto> GetOrganizationByIdAsync(Guid id);
        Task<OrganizationDto> CreateOrganizationAsync(CreateOrganizationDto organization);
        Task UpdateOrganizationAsync(OrganizationDto organization);
        Task DeleteOrganizationAsync(Guid id);
        Task<OrganizationDto> CreateOrganizationWithAdminAsync(CreateOrganizationDto organization, Guid superAdminId);
        Task<bool> IsOrganizationExists(Guid id);

    }
}
