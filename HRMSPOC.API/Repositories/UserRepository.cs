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
        
        public UserRepository(HRMSDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<IEnumerable<ApplicationUser>> GetUsersAsync()
        {
            return await _context.Users.Where(u=>!u.isdelete).ToListAsync();
        }

        public async Task<ApplicationUser> GetUserByIdAsync(string id)
        {
            return await _userManager.Users
                .FirstOrDefaultAsync(u => u.Id == id && !u.isdelete);
        }

        public async Task<ApplicationUser> CreateUserAsync(ApplicationUser user)
        {
            user.UserName = user.Email;
            var result = await _userManager.CreateAsync(user,user.PasswordHash);
            if(user.CreatedBy != Guid.Empty)
            {
                var createdById = user.CreatedBy.ToString();
            }
            if (result.Succeeded)
            {
                return user;
            }
            throw new Exception("User creation failed: " + string.Join(", ", result.Errors));

        }

        public async Task UpdateUserAsync(ApplicationUser user)
        {
            var existingUser = await _userManager.FindByIdAsync(user.Id);
            if (existingUser == null)
            {
                throw new Exception("User not found.");
            }
            if (existingUser.isdelete)
            {
                throw new Exception("User was Deleted");
            }
            existingUser.FirstName = user.FirstName;
            existingUser.LastName = user.LastName;
            existingUser.Address = user.Address;
            existingUser.PhoneNumber = user.PhoneNumber;  
            existingUser.Email = user.Email;

            if (!string.IsNullOrWhiteSpace(user.PasswordHash))
            {
                var token = await _userManager.GeneratePasswordResetTokenAsync(existingUser);
                var result = await _userManager.ResetPasswordAsync(existingUser, token, user.PasswordHash);

                if (!result.Succeeded)
                {
                    throw new Exception("Password update failed: " + string.Join(", ", result.Errors));
                }
            }

            await _userManager.UpdateAsync(existingUser);
        }


        public async Task DeleteUserAsync(string id)
        {
            var user = await GetUserByIdAsync(id);
            if (user != null)
            {
                user.isdelete = true;
                await _userManager.UpdateAsync(user);
            }
        }
        public async Task<IEnumerable<ApplicationUser>> GetUsersByCreatedByIdAsync(Guid createdbyId)
        {
            return await _userManager.Users
                .Where(u => u.CreatedBy == createdbyId && !u.isdelete)
                .ToListAsync();
        }

        public async Task<IEnumerable<UserWithRoleDto>> GetUsersByOrganizationIdAsync(Guid organizationId)
        {
            var organizationIdParam = new SqlParameter("@OrganizationId", organizationId);

            var users = await _context.Set<UserWithRoleDto>()
             .FromSqlRaw("EXEC sp_GetUsersByOrganizationId @OrganizationId", organizationIdParam)
             .ToListAsync();

            return users;
        }


    }
}
