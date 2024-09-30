using HRMSPOC.API.DTOs;
using HRMSPOC.API.Models;
using HRMSPOC.API.Repositories.Interfaces;
using HRMSPOC.API.Services.Interface;
using Microsoft.AspNetCore.Identity;

namespace HRMSPOC.API.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IOrganizationRepository _organizationRepository;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IUserOrganizationRepository _userOrganizationRepository;
        public UserService(IUserRepository userRepository, IOrganizationRepository organizationRepository,UserManager<ApplicationUser> userManager, IUserOrganizationRepository userOrganizationRepository)
        {
            _userRepository = userRepository;
            _organizationRepository = organizationRepository;
            _userManager = userManager;
            _userOrganizationRepository = userOrganizationRepository;
        }

        public async Task<IEnumerable<ApplicationUser>> GetUsersAsync()
        {
            return await _userRepository.GetUsersAsync();
        }

        public async Task<ApplicationUser> GetUserByIdAsync(string id)
        {
            return await _userRepository.GetUserByIdAsync(id);
        }

        public async Task<ApplicationUser> CreateUserAsync(ApplicationUser user, string role)
        {
            // Validate the CreatedBy field
            if (user.CreatedBy != Guid.Empty)
            {
                var createdById = user.CreatedBy;

                bool isOrganization = await _organizationRepository.IsOrganizationExists(createdById);

                // Create the user
                var result = await _userRepository.CreateUserAsync(user);

                if (result != null)
                {
                    if (!string.IsNullOrEmpty(role))
                    {
                        await _userManager.AddToRoleAsync(result, role);
                        var organizationId = await _userOrganizationRepository.GetOrganizationIdByUserIdAsync(createdById.ToString());
                        if (organizationId.HasValue)
                        {
                            await _userOrganizationRepository.AddUserOrganizationAsync(result.Id, organizationId.Value);
                        }

                    }
                    else if (isOrganization)
                    {
                        await _userManager.AddToRoleAsync(result, "HR");
                        await _userOrganizationRepository.AddUserOrganizationAsync(result.Id, createdById);
                    }
                    else
                    {
                        await _userManager.AddToRoleAsync(result, "Employee");
                        var organizationId = await _userOrganizationRepository.GetOrganizationIdByUserIdAsync(createdById.ToString());
                        if (organizationId.HasValue)
                        {
                            await _userOrganizationRepository.AddUserOrganizationAsync(result.Id, organizationId.Value);
                        }
                    }
                }

                return result;
            }
            return await _userRepository.CreateUserAsync(user);
        }


        public async Task UpdateUserAsync(ApplicationUser user)
        {
            await _userRepository.UpdateUserAsync(user);
        }

        public async Task DeleteUserAsync(string id)
        {
            await _userRepository.DeleteUserAsync(id);
        }
        public async Task<IEnumerable<ApplicationUser>> GetUsersByCreatedByIdAsync(Guid createdbyId)
        {
            return await _userRepository.GetUsersByCreatedByIdAsync(createdbyId);
        }
        public async Task<IEnumerable<UserWithRoleDto>> GetUsersByOrganizationIdAsync(Guid organizationId)
        {
            return await _userRepository.GetUsersByOrganizationIdAsync(organizationId);
        }
    }
}
