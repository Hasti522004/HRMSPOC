using AutoMapper;
using HRMSPOC.API.Data;
using HRMSPOC.API.DTOs;
using HRMSPOC.API.Models;
using HRMSPOC.API.Repositories.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace HRMSPOC.API.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly HRMSDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IMapper _mapper;

        public UserRepository(HRMSDbContext context, UserManager<ApplicationUser> userManager, IMapper mapper)
        {
            _context = context;
            _userManager = userManager;
            _mapper = mapper;
        }

        public async Task<IEnumerable<ApplicationUserDto>> GetUsersAsync()
        {
            var users = await _context.Users.Where(u => !u.IsDeleted).ToListAsync();
            var userDtos = _mapper.Map<IEnumerable<ApplicationUserDto>>(users);
            return userDtos;
        }

        public async Task<ApplicationUserDto> GetUserByIdAsync(string id)
        {
            var user = await _userManager.Users.FirstOrDefaultAsync(u => u.Id == id && !u.IsDeleted);
            return _mapper.Map<ApplicationUserDto>(user);
        }

        public async Task<ApplicationUserDto> CreateUserAsync(CreateUserDto userDto)
        {
            var user = _mapper.Map<ApplicationUser>(userDto);
            user.UserName = user.Email;

            var result = await _userManager.CreateAsync(user, userDto.Password);
            if (!result.Succeeded)
            {
                throw new Exception("User creation failed: " + string.Join(", ", result.Errors));
            }

            return _mapper.Map<ApplicationUserDto>(user);
        }

        public async Task UpdateUserAsync(ApplicationUserDto userDto)
        {
            var existingUser = await _userManager.FindByIdAsync(userDto.Id);

            // Map the updated fields from DTO to the existing user
            _mapper.Map(userDto, existingUser);

            // Update user fields other than password
            await _userManager.UpdateAsync(existingUser);
        }

        public async Task<string> GeneratePasswordResetTokenAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                throw new Exception("User not found.");
            }

            return await _userManager.GeneratePasswordResetTokenAsync(user);
        }

        public async Task<IdentityResult> ResetPasswordAsync(string userId, string token, string newPassword)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                throw new Exception("User not found.");
            }

            return await _userManager.ResetPasswordAsync(user, token, newPassword);
        }

        public async Task DeleteUserAsync(string id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user != null)
            {
                user.IsDeleted = true;
                await _context.SaveChangesAsync();
            }
            else
            {
                throw new Exception("User not found.");
            }
        }

        public async Task<IEnumerable<ApplicationUserDto>> GetUsersByCreatedByIdAsync(Guid createdById)
        {
            var users = await _userManager.Users
                .Where(u => u.CreatedBy == createdById && !u.IsDeleted)
                .ToListAsync();

            return _mapper.Map<IEnumerable<ApplicationUserDto>>(users);
        }

        public async Task<IEnumerable<UserWithRoleDto>> GetUsersByOrganizationIdAsync(Guid organizationId)
        {
            var organizationIdParam = new SqlParameter("@OrganizationId", organizationId);
            return await _context.Set<UserWithRoleDto>()
                .FromSqlRaw("EXEC sp_GetUsersByOrganizationId @OrganizationId", organizationIdParam)
                .ToListAsync();
        }

        public async Task AssignRoleAsync(string userId, string role)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user != null)
            {
                await _userManager.AddToRoleAsync(user, role);
            }
        }
    }
}
