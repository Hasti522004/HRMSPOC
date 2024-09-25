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
            return await _context.Users.ToListAsync();
        }

        public async Task<ApplicationUser> GetUserByIdAsync(string id)
        {
            return await _userManager.FindByIdAsync(id.ToString());
        }

        public async Task<ApplicationUser> CreateUserAsync(ApplicationUser user)
        {
            user.UserName = user.Email;
            var result = await _userManager.CreateAsync(user);
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

            // Update user fields
            existingUser.FirstName = user.FirstName;
            existingUser.LastName = user.LastName;
            existingUser.Address = user.Address;
            existingUser.PhoneNumber = user.PhoneNumber;  // Update phone number
            existingUser.Email = user.Email;  // Update email

            // Update password if provided
            if (!string.IsNullOrWhiteSpace(user.PasswordHash))
            {
                var token = await _userManager.GeneratePasswordResetTokenAsync(existingUser);
                var result = await _userManager.ResetPasswordAsync(existingUser, token, user.PasswordHash);

                if (!result.Succeeded)
                {
                    throw new Exception("Password update failed: " + string.Join(", ", result.Errors));
                }
            }

            // Update the user in the database
            await _userManager.UpdateAsync(existingUser);
        }


        public async Task DeleteUserAsync(string id)
        {
            var user = await GetUserByIdAsync(id);
            if (user != null)
            {
                await _userManager.DeleteAsync(user);
            }
        }
        public async Task<IEnumerable<ApplicationUser>> GetUsersByCreatedByIdAsync(Guid createdbyId)
        {
            return await _userManager.Users
                .Where(u => u.CreatedBy == createdbyId)
                .ToListAsync();
        }

    }
}
