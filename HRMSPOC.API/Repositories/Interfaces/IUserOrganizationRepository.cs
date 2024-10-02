using HRMSPOC.API.DTOs;

namespace HRMSPOC.API.Repositories.Interfaces
{
    public interface IUserOrganizationRepository
    {
        Task<Guid?> GetOrganizationIdByUserIdAsync(string userId);
        Task<bool> AddUserOrganizationAsync(UserOrganizationDto userOrganizationDto);
    }
}
