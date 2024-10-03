using HRMSPOC.API.DTOs;

namespace HRMSPOC.API.Services.Interface
{
    public interface IUserService
    {
        Task<IEnumerable<ApplicationUserDto>> GetUsersAsync();
        Task<ApplicationUserDto> GetUserByIdAsync(string id);
        Task<ApplicationUserDto> CreateUserAsync(CreateUserDto user, string role);
        Task UpdateUserAsync(ApplicationUserDto user);
        Task DeleteUserAsync(string id);
        Task<IEnumerable<ApplicationUserDto>> GetUsersByCreatedByIdAsync(Guid createdById);
        Task<IEnumerable<UserWithRoleDto>> GetUsersByOrganizationIdAsync(Guid organizationId);
    }
}
