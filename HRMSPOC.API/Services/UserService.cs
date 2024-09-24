using HRMSPOC.API.Models;
using HRMSPOC.API.Repositories.Interfaces;
using HRMSPOC.API.Services.Interface;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HRMSPOC.API.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<IEnumerable<ApplicationUser>> GetUsersAsync()
        {
            return await _userRepository.GetUsersAsync();
        }

        public async Task<ApplicationUser> GetUserByIdAsync(string id)
        {
            return await _userRepository.GetUserByIdAsync(id);
        }

        public async Task<ApplicationUser> CreateUserAsync(ApplicationUser user)
        {
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
        public async Task<IEnumerable<ApplicationUser>> GetUsersByOrganizationIdAsync(Guid organizationId)
        {
            return await _userRepository.GetUsersByOrganizationIdAsync(organizationId);
        }

    }
}
