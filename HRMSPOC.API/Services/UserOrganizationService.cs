using HRMSPOC.API.DTOs;
using HRMSPOC.API.Models;
using HRMSPOC.API.Repositories.Interfaces;
using HRMSPOC.API.Services.Interface;
using System;

namespace HRMSPOC.API.Services
{
    public class UserOrganizationService : IUserOrganizationService
    {
        private readonly IUserOrganizationRepository _userOrganizationRepository;

        public UserOrganizationService(IUserOrganizationRepository userOrganizationRepository)
        {
            _userOrganizationRepository = userOrganizationRepository;
        }

        public async Task<Guid?> GetOrganizationIdByUserIdAsync(string userId)
        {
            if (string.IsNullOrWhiteSpace(userId))
            {
                throw new ArgumentException("User ID cannot be null or empty.", nameof(userId));
            }

            return await _userOrganizationRepository.GetOrganizationIdByUserIdAsync(userId);
        }

        public async Task<bool> AddUserOrganizationAsync(UserOrganizationDto userOrganizationDto)
        {
            if (userOrganizationDto == null)
            {
                throw new ArgumentNullException(nameof(userOrganizationDto), "UserOrganizationDto cannot be null.");
            }

            if (string.IsNullOrEmpty(userOrganizationDto.UserId))
            {
                throw new ArgumentException("User ID cannot be empty.", nameof(userOrganizationDto.UserId));
            }

            if (userOrganizationDto.OrganizationId == Guid.Empty)
            {
                throw new ArgumentException("Organization ID cannot be empty.", nameof(userOrganizationDto.OrganizationId));
            }

            var organizationId = await _userOrganizationRepository.GetOrganizationIdByUserIdAsync(userOrganizationDto.UserId.ToString());
            if (organizationId == null)
            {
                throw new KeyNotFoundException($"No organization found for user ID: {userOrganizationDto.UserId}.");
            }

            return await _userOrganizationRepository.AddUserOrganizationAsync(userOrganizationDto);
        }
    }
}
