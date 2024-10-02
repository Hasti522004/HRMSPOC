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
            Console.WriteLine(users.Count);
            var userDtos = _mapper.Map<IEnumerable<ApplicationUserDto>>(users);
            Console.WriteLine(userDtos);

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
                Console.WriteLine("User creation failed:");
                foreach (var error in result.Errors)
                {
                    Console.WriteLine($"- {error.Code}: {error.Description}");
                }
                throw new Exception("User creation failed: " + string.Join(", ", result.Errors));
            }

            return _mapper.Map<ApplicationUserDto>(user);
        }

        public async Task UpdateUserAsync(ApplicationUserDto userDto)
        {
            var existingUser = await _userManager.FindByIdAsync(userDto.Id);
            if (existingUser == null || existingUser.IsDeleted)
            {
                throw new Exception("User not found or deleted.");
            }

            _mapper.Map(userDto, existingUser);

            if (!string.IsNullOrWhiteSpace(userDto.Password))
            {
                var token = await _userManager.GeneratePasswordResetTokenAsync(existingUser);
                var result = await _userManager.ResetPasswordAsync(existingUser, token, userDto.Password);

                if (!result.Succeeded)
                {
                    throw new Exception("Password update failed: " + string.Join(", ", result.Errors));
                }
            }

            await _userManager.UpdateAsync(existingUser);
        }

        public async Task DeleteUserAsync(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user != null)
            {
                user.IsDeleted = true;
                await _userManager.UpdateAsync(user);
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
