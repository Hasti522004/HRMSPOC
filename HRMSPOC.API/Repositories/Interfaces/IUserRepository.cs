using HRMSPOC.API.DTOs;
using HRMSPOC.API.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HRMSPOC.API.Repositories.Interfaces
{
    public interface IUserRepository
    {
        Task<IEnumerable<ApplicationUserDto>> GetUsersAsync();
        Task<ApplicationUserDto> GetUserByIdAsync(string id);
        Task<ApplicationUserDto> CreateUserAsync(CreateUserDto user);
        Task UpdateUserAsync(ApplicationUserDto user);
        Task DeleteUserAsync(string id);
        Task<IEnumerable<ApplicationUserDto>> GetUsersByCreatedByIdAsync(Guid createdbyId);
        Task<IEnumerable<UserWithRoleDto>> GetUsersByOrganizationIdAsync(Guid organizationId);
        Task AssignRoleAsync(string userId, string role);
    }
}
