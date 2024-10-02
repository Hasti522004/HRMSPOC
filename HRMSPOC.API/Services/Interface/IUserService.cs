using HRMSPOC.API.DTOs;
using HRMSPOC.API.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HRMSPOC.API.Services.Interface
{
    public interface IUserService
    {
        Task<IEnumerable<ApplicationUserDto>> GetUsersAsync();
        Task<ApplicationUserDto> GetUserByIdAsync(string id);
        Task<ApplicationUserDto> CreateUserAsync(CreateUserDto user,string role);
        Task UpdateUserAsync(ApplicationUserDto user);
        Task DeleteUserAsync(string id);
        Task<IEnumerable<ApplicationUserDto>> GetUsersByCreatedByIdAsync(Guid createdbyId);
        Task<IEnumerable<UserWithRoleDto>> GetUsersByOrganizationIdAsync(Guid organizationId);
    }
}
