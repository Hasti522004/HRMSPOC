using HRMSPOC.API.DTOs;
using HRMSPOC.API.Models;
using HRMSPOC.API.Repositories.Interfaces;
using HRMSPOC.API.Services.Interface;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HRMSPOC.API.Services
{
    public class OrganizationService : IOrganizationService
    {
        private readonly IOrganizationRepository _organizationRepository;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<ApplicationUser> _userManager;
        public OrganizationService(IOrganizationRepository organizationRepository, RoleManager<IdentityRole> roleManager, UserManager<ApplicationUser> userManager)
        {
            _organizationRepository = organizationRepository;
            _roleManager = roleManager;
            _userManager = userManager;
        }

        public async Task<IEnumerable<OrganizationDto>> GetOrganizationsAsync()
        {
            return await _organizationRepository.GetOrganizationsAsync();
        }

        public async Task<OrganizationDto> GetOrganizationByIdAsync(Guid id)
        {
            return await _organizationRepository.GetOrganizationByIdAsync(id);
        }

        public async Task<OrganizationDto> CreateOrganizationAsync(OrganizationDto organization)
        {
            return await _organizationRepository.CreateOrganizationAsync(organization);
        }

        public async Task UpdateOrganizationAsync(OrganizationDto organization)
        {
            await _organizationRepository.UpdateOrganizationAsync(organization);
        }

        public async Task DeleteOrganizationAsync(Guid id)
        {
            await _organizationRepository.DeleteOrganizationAsync(id);
        }

        public async Task<OrganizationDto> CreateOrganizationWithAdminAsync(OrganizationDto organization, Guid superAdminId)
        {
            var createdOrganization = await _organizationRepository.CreateOrganizationAsync(organization);

            var adminUser = new ApplicationUserDto
            {
                Email = "admin@" + createdOrganization.Name.Replace(" ", "").ToLower() + ".com",
                FirstName = "Admin",
                LastName = createdOrganization.Name,
                CreatedBy = superAdminId
            };

            await _organizationRepository.CreateRoleIfNotExistsAsync("Admin");

            await _organizationRepository.CreateAdminUserAsync(adminUser, createdOrganization.Id);

            // Assign the Admin role to the created user in the repository
            await _organizationRepository.AssignRoleToUserAsync(adminUser, "Admin");
            return createdOrganization;
        }

        public async Task<bool> IsOrganizationExists(Guid id)
        {
            return await _organizationRepository.IsOrganizationExists(id);
        }
    }
}
