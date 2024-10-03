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
            var users = await _userRepository.GetUsersAsync();
            if (users == null || !users.Any())
            {
                throw new Exception("No users found in the system.");
            }
            return users;
        }

        public async Task<ApplicationUserDto> GetUserByIdAsync(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                throw new ArgumentException("User ID cannot be null or empty.");
            }

            var user = await _userRepository.GetUserByIdAsync(id);

            if (user == null)
            {
                throw new Exception($"User with ID {id} not found.");
            }

            return user;
        }

        public async Task<ApplicationUserDto> CreateUserAsync(CreateUserDto user, string role)
        {
            if (user.CreatedBy != Guid.Empty)
            {
                var createdById = user.CreatedBy;
                var result = await _userRepository.CreateUserAsync(user);

                if (result != null)
                {
                    if (!string.IsNullOrEmpty(role))
                    {
                        await _userRepository.AssignRoleAsync(result.Id, role);

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
                        bool isOrganization = await _organizationRepository.IsOrganizationExists(createdById);
                        string defaultRole = isOrganization ? "HR" : "Employee";

                        await _userRepository.AssignRoleAsync(result.Id, defaultRole);

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
            var existingUser = await _userRepository.GetUserByIdAsync(user.Id);
            if (existingUser == null)
            {
                throw new Exception("User not found or deleted.");
            }
            if (!string.IsNullOrWhiteSpace(user.Password))
            {
                var token = await _userRepository.GeneratePasswordResetTokenAsync(user.Id);
                var result = await _userRepository.ResetPasswordAsync(user.Id, token, user.Password);

                if (!result.Succeeded)
                {
                    throw new Exception("Password update failed: " + string.Join(", ", result.Errors));
                }
            }

            await _userRepository.UpdateUserAsync(user);

        }

        public async Task DeleteUserAsync(string id)
        {
            var existingUser = await _userRepository.GetUserByIdAsync(id);
            if (existingUser == null || existingUser.IsDeleted)
            {
                throw new Exception("User not found or already deleted.");
            }

            await _userRepository.DeleteUserAsync(id);
        }

        public async Task<IEnumerable<ApplicationUserDto>> GetUsersByCreatedByIdAsync(Guid createdById)
        {
            if (createdById == Guid.Empty)
            {
                throw new ArgumentException("CreatedById cannot be an empty GUID.", nameof(createdById));
            }

            var users = await _userRepository.GetUsersByCreatedByIdAsync(createdById);

            if (users == null || !users.Any())
            {
                throw new Exception("No users found for the specified CreatedById.");
            }

            return users;
        }

        public async Task<IEnumerable<UserWithRoleDto>> GetUsersByOrganizationIdAsync(Guid organizationId)
        {
            if (organizationId == Guid.Empty)
            {
                throw new ArgumentException("OrganizationId cannot be an empty GUID.", nameof(organizationId));
            }

            var users = await _userRepository.GetUsersByOrganizationIdAsync(organizationId);

            if (users == null || !users.Any())
            {
                throw new Exception("No users found for the specified OrganizationId.");
            }

            return users;
        }
    }
}
