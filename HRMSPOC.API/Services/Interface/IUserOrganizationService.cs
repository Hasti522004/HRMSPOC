using HRMSPOC.API.DTOs;

namespace HRMSPOC.API.Services.Interface
{
    public interface IUserOrganizationService
    {
        Task<Guid?> GetOrganizationIdByUserIdAsync(string userId);
        Task<bool> AddUserOrganizationAsync(UserOrganizationDto userOrganizationDto);
    }
}
