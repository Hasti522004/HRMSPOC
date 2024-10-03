using HRMSPOC.API.DTOs;
using Microsoft.AspNetCore.Identity;

namespace HRMSPOC.API.Repositories.Interfaces
{
    public interface IUserRepository
    {
        Task<IEnumerable<ApplicationUserDto>> GetUsersAsync();
        Task<ApplicationUserDto> GetUserByIdAsync(string id);
        Task<ApplicationUserDto> CreateUserAsync(CreateUserDto userDto);
        Task UpdateUserAsync(ApplicationUserDto userDto);
        Task DeleteUserAsync(string id);
        Task<IEnumerable<ApplicationUserDto>> GetUsersByCreatedByIdAsync(Guid createdById);
        Task<IEnumerable<UserWithRoleDto>> GetUsersByOrganizationIdAsync(Guid organizationId);
        Task AssignRoleAsync(string userId, string role);
        Task<string> GeneratePasswordResetTokenAsync(string userId);
        Task<IdentityResult> ResetPasswordAsync(string userId, string token, string newPassword);
    }
}
