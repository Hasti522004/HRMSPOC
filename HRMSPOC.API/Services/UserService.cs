using HRMSPOC.API.DTOs;
using HRMSPOC.API.Repositories.Interfaces;
using HRMSPOC.API.Services.Interface;

namespace HRMSPOC.API.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IOrganizationRepository _organizationRepository;
        private readonly IUserOrganizationRepository _userOrganizationRepository;

        public UserService(IUserRepository userRepository, IOrganizationRepository organizationRepository, IUserOrganizationRepository userOrganizationRepository)
        {
            _userRepository = userRepository;
            _organizationRepository = organizationRepository;
            _userOrganizationRepository = userOrganizationRepository;
        }

        public async Task<IEnumerable<ApplicationUserDto>> GetUsersAsync()
        {
            return await _userRepository.GetUsersAsync();
        }

        public async Task<ApplicationUserDto> GetUserByIdAsync(string id)
        {
            return await _userRepository.GetUserByIdAsync(id);
        }

        public async Task<ApplicationUserDto> CreateUserAsync(CreateUserDto user, string role)
        {
            // Validate the CreatedBy field
            if (user.CreatedBy != Guid.Empty)
            {
                var createdById = user.CreatedBy;
                var result = await _userRepository.CreateUserAsync(user);

                if (result != null)
                {
                    // If role is provided, assign it
                    if (!string.IsNullOrEmpty(role))
                    {
                        await _userRepository.AssignRoleAsync(result.Id, role);

                        // Create a UserOrganizationDto to add the user to the organization
                        var organizationId = await _userOrganizationRepository.GetOrganizationIdByUserIdAsync(createdById.ToString());
                        if (organizationId.HasValue)
                        {
                            var userOrganizationDto = new UserOrganizationDto
                            {
                                UserId = result.Id,
                                OrganizationId = organizationId.Value
                            };
                            await _userOrganizationRepository.AddUserOrganizationAsync(userOrganizationDto);
                        }
                    }
                    else
                    {
                        // Determine if the user is an organization (HR) or employee
                        bool isOrganization = await _organizationRepository.IsOrganizationExists(createdById);
                        string defaultRole = isOrganization ? "HR" : "Employee";

                        await _userRepository.AssignRoleAsync(result.Id, defaultRole);

                        // Add the user to the appropriate organization
                        var organizationId = await _userOrganizationRepository.GetOrganizationIdByUserIdAsync(createdById.ToString());
                        if (organizationId.HasValue)
                        {
                            var userOrganizationDto = new UserOrganizationDto
                            {
                                UserId = result.Id,
                                OrganizationId = organizationId.Value
                            };
                            await _userOrganizationRepository.AddUserOrganizationAsync(userOrganizationDto);
                        }
                    }
                }

                return result;
            }

            return await _userRepository.CreateUserAsync(user);
        }

        public async Task UpdateUserAsync(ApplicationUserDto user)
        {
            await _userRepository.UpdateUserAsync(user);
        }

        public async Task DeleteUserAsync(string id)
        {
            await _userRepository.DeleteUserAsync(id);
        }

        public async Task<IEnumerable<ApplicationUserDto>> GetUsersByCreatedByIdAsync(Guid createdById)
        {
            return await _userRepository.GetUsersByCreatedByIdAsync(createdById);
        }

        public async Task<IEnumerable<UserWithRoleDto>> GetUsersByOrganizationIdAsync(Guid organizationId)
        {
            return await _userRepository.GetUsersByOrganizationIdAsync(organizationId);
        }
    }
}
