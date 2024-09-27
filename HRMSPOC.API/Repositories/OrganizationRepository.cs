using HRMSPOC.API.Data;
using HRMSPOC.API.Models;
using HRMSPOC.API.Repositories.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HRMSPOC.API.Repositories
{
    public class OrganizationRepository : IOrganizationRepository
    {
        private readonly HRMSDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public OrganizationRepository(HRMSDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<IEnumerable<Organization>> GetOrganizationsAsync()
        {
            return await _context.Organization.Where(u=>!u.isdelete).ToListAsync();
        }

        public async Task<Organization> GetOrganizationByIdAsync(Guid id)
        {
            return await _context.Organization.Where(u => u.Id == id && !u.isdelete).FirstOrDefaultAsync();
        }

        public async Task<Organization> CreateOrganizationAsync(Organization organization)
        {
            _context.Organization.Add(organization);
            await _context.SaveChangesAsync();
            return organization;
        }

        public async Task UpdateOrganizationAsync(Organization organization)
        {
            _context.Entry(organization).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteOrganizationAsync(Guid id)
        {
            var organization = await GetOrganizationByIdAsync(id);
            if (organization != null)
            {
                organization.isdelete = true;
                _context.Organization.Update(organization);
                await _context.SaveChangesAsync();
            }
        }

        public async Task CreateAdminUserAsync(ApplicationUser adminUser, Guid organizationId)
        {
            string adminPassword = $"Admin@{adminUser.LastName.Replace(" ", "").ToLower()}123";

            var result = await _userManager.CreateAsync(adminUser,adminPassword);
            if (result.Succeeded)
            {
                var userOrg = new UserOrganization
                {
                    UserId = adminUser.Id,
                    OrganizationId = organizationId
                };
                await _context.UserOrganizations.AddAsync(userOrg);
                await _context.SaveChangesAsync();
            }
            else
            {
                throw new Exception("Failed to create admin user.");
            }
        }

        public async Task<bool> IsOrganizationExists(Guid id)
        {
            return await _context.Organization.AnyAsync(o => o.Id == id && !o.isdelete);
        }
    }
}
