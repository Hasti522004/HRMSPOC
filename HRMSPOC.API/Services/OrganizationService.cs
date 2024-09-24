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

        public async Task<IEnumerable<Organization>> GetOrganizationsAsync()
        {
            return await _organizationRepository.GetOrganizationsAsync();
        }

        public async Task<Organization> GetOrganizationByIdAsync(Guid id)
        {
            return await _organizationRepository.GetOrganizationByIdAsync(id);
        }

        public async Task<Organization> CreateOrganizationAsync(Organization organization)
        {
            return await _organizationRepository.CreateOrganizationAsync(organization);
        }

        public async Task UpdateOrganizationAsync(Organization organization)
        {
            await _organizationRepository.UpdateOrganizationAsync(organization);
        }

        public async Task DeleteOrganizationAsync(Guid id)
        {
            await _organizationRepository.DeleteOrganizationAsync(id);
        }

        public async Task<Organization> CreateOrganizationWithAdminAsync(Organization organization, Guid superAdminId)
        {
            var createdOrganization = await _organizationRepository.CreateOrganizationAsync(organization);

            var adminUser = new ApplicationUser
            {
                UserName = "admin@" + createdOrganization.Name.Replace(" ", "").ToLower() + ".com",
                Email = "admin@" + createdOrganization.Name.Replace(" ", "").ToLower() + ".com",
                FirstName = "Admin",
                LastName = createdOrganization.Name,
                EmailConfirmed = true,
                CreatedBy = superAdminId
            };

            // Create the Admin role if it doesn't exist
            if (!await _roleManager.RoleExistsAsync("Admin"))
            {
                await _roleManager.CreateAsync(new IdentityRole("Admin"));
            }

            await _organizationRepository.CreateAdminUserAsync(adminUser, createdOrganization.Id);

            // Assign the Admin role to the created user
            var result = await _userManager.AddToRoleAsync(adminUser, "Admin");
            if (!result.Succeeded)
            {
                throw new Exception("Failed to assign admin role to user.");
            }

            return createdOrganization;
        }
    }
}
