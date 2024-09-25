﻿using HRMSPOC.API.Data;
using HRMSPOC.API.Models;
using HRMSPOC.API.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace HRMSPOC.API.Repositories
{
    public class UserOrganizationRepository : IUserOrganizationRepository
    {
        private readonly HRMSDbContext _context;
        public UserOrganizationRepository(HRMSDbContext context)
        {
            _context = context;
        }
        public async Task<Guid?> GetOrganizationIdByUserIdAsync(string userId)
        {
            var userOrganization = await _context.UserOrganizations.FirstOrDefaultAsync(u => u.UserId == userId);
            return userOrganization?.OrganizationId;
        }
        public async Task<bool> AddUserOrganizationAsync(string userId, Guid organizationId)
        {
            var userOrganization = new UserOrganization
            {
                UserId = userId,
                OrganizationId = organizationId
            };

            await _context.UserOrganizations.AddAsync(userOrganization);

            // Save changes and return true if successful, otherwise false
            return await _context.SaveChangesAsync() > 0; // Returns true if any row was affected
        }

    }
}
