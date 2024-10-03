using AutoMapper;
using HRMSPOC.API.Data;
using HRMSPOC.API.DTOs;
using HRMSPOC.API.Models;
using HRMSPOC.API.Repositories.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace HRMSPOC.API.Repositories
{
    public class OrganizationRepository : IOrganizationRepository
    {
        private readonly HRMSDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IMapper _mapper;

        public OrganizationRepository(HRMSDbContext context, UserManager<ApplicationUser> userManager, IMapper mapper, RoleManager<IdentityRole> roleManager)
        {
            _context = context;
            _userManager = userManager;
            _mapper = mapper;
            _roleManager = roleManager;
        }

        // Retrieve all organizations
        public async Task<IEnumerable<OrganizationDto>> GetOrganizationsAsync()
        {
            var organizations = await _context.Organization.Where(u => !u.isdelete).ToListAsync();
            return _mapper.Map<IEnumerable<OrganizationDto>>(organizations);
        }

        // Retrieve a single organization by Id
        public async Task<OrganizationDto> GetOrganizationByIdAsync(Guid id)
        {
            var organization = await _context.Organization
                .Where(o => o.Id == id && !o.isdelete)
                .FirstOrDefaultAsync();

            if (organization == null)
                throw new KeyNotFoundException("Organization not found.");

            return _mapper.Map<OrganizationDto>(organization);
        }

        // Create a new organization
        public async Task<OrganizationDto> CreateOrganizationAsync(CreateOrganizationDto organizationDto)
        {
            var organization = _mapper.Map<Organization>(organizationDto);
            await _context.Organization.AddAsync(organization);
            await _context.SaveChangesAsync();

            return _mapper.Map<OrganizationDto>(organization);
        }

        // Update an existing organization
        public async Task UpdateOrganizationAsync(OrganizationDto organizationDto)
        {
            var organization = await _context.Organization.FindAsync(organizationDto.Id);
            if (organization == null || organization.isdelete)
                throw new KeyNotFoundException("Organization not found or deleted.");

            // Map updates from DTO to the entity
            _mapper.Map(organizationDto, organization);

            _context.Organization.Update(organization);
            await _context.SaveChangesAsync();
        }

        // Soft delete an organization
        public async Task DeleteOrganizationAsync(Guid id)
        {
            var organization = await _context.Organization.FindAsync(id);
            if (organization == null)
                throw new KeyNotFoundException("Organization not found.");

            // Soft delete by setting isdelete flag
            organization.isdelete = true;
            _context.Organization.Update(organization);
            await _context.SaveChangesAsync();
        }

        // Create an admin user for the organization
        public async Task CreateAdminUserAsync(CreateUserDto adminUserDto, Guid organizationId)
        {
            var adminUser = _mapper.Map<ApplicationUser>(adminUserDto);
            adminUser.UserName = adminUser.Email; // Set username to email

            string adminPassword = $"Admin@{adminUserDto.LastName.Replace(" ", "").ToLower()}123";

            var result = await _userManager.CreateAsync(adminUser, adminPassword);
            if (!result.Succeeded)
                throw new Exception("Failed to create admin user: " + string.Join(", ", result.Errors.Select(e => e.Description)));

            var userOrg = new UserOrganization
            {
                UserId = adminUser.Id,
                OrganizationId = organizationId
            };

            await _context.UserOrganizations.AddAsync(userOrg);
            await _context.SaveChangesAsync();
        }

        // Check if an organization exists by its Id
        public async Task<bool> IsOrganizationExists(Guid id)
        {
            return await _context.Organization.AnyAsync(o => o.Id == id && !o.isdelete);
        }
        public async Task CreateRoleIfNotExistsAsync(string roleName)
        {
            if (!await _roleManager.RoleExistsAsync(roleName))
            {
                await _roleManager.CreateAsync(new IdentityRole(roleName));
            }
        }

        // Assign the Admin role to the created user
        public async Task AssignRoleToUserAsync(CreateUserDto adminUserDto, string roleName)
        {
            // Map ApplicationUserDto to ApplicationUser
            var adminUser = await _userManager.FindByEmailAsync(adminUserDto.Email);
            if (adminUser == null)
            {
                throw new KeyNotFoundException("User not found.");
            }

            var result = await _userManager.AddToRoleAsync(adminUser, roleName);
            if (!result.Succeeded)
            {
                throw new Exception("Failed to assign role to user: " + string.Join(", ", result.Errors.Select(e => e.Description)));
            }
        }
    }
}
