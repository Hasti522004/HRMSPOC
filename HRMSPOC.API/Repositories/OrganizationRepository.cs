using HRMSPOC.API.Data;
using HRMSPOC.API.Models;
using HRMSPOC.API.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace HRMSPOC.API.Repositories
{
    public class OrganizationRepository : IOrganizationRepository
    {
        private readonly HRMSDbContext _context;
        public OrganizationRepository(HRMSDbContext context)
        {
            _context = context;
        }
        public async Task AddAsync(Organization organization)
        {
            _context.Organization.Add(organization);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var org = await _context.Organization.FindAsync(id);
            if(org != null)
            {
                _context.Organization.Remove(org);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<Organization>> GetAllAsync()
        {
            return await _context.Organization.ToListAsync();
        }

        public async Task<Organization> GetByIdAsync(int id)
        {
            return await _context.Organization.FindAsync(id);
        }

        public async Task UpdateAsync(Organization organization)
        {
            _context.Organization.Update(organization);
            await _context.SaveChangesAsync();
        }
    }
}
