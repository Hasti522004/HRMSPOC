using AutoMapper;
using HRMSPOC.API.Data;
using HRMSPOC.API.DTOs;
using HRMSPOC.API.Models;
using HRMSPOC.API.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace HRMSPOC.API.Repositories
{
    public class UserOrganizationRepository : IUserOrganizationRepository
    {
        private readonly HRMSDbContext _context;
        private readonly IMapper _mapper; // Inject AutoMapper for mapping

        public UserOrganizationRepository(HRMSDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<Guid?> GetOrganizationIdByUserIdAsync(string userId)
        {
            var userOrganization = await _context.UserOrganizations.FirstOrDefaultAsync(u => u.UserId == userId);
            return userOrganization?.OrganizationId;
        }

        public async Task<bool> AddUserOrganizationAsync(UserOrganizationDto userOrganizationDTO)
        {
            // Map DTO to entity
            var userOrganization = _mapper.Map<UserOrganization>(userOrganizationDTO);

            await _context.UserOrganizations.AddAsync(userOrganization);

            // Save changes and return true if successful
            return await _context.SaveChangesAsync() > 0;
        }
    }
}
