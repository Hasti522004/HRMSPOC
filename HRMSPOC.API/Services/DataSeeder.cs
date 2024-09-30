﻿using HRMSPOC.API.Models;
using Microsoft.AspNetCore.Identity;

namespace HRMSPOC.API.Services
{
    public class DataSeeder
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<ApplicationUser> _userManager;

        public DataSeeder(RoleManager<IdentityRole> roleManager, UserManager<ApplicationUser> userManager)
        {
            _roleManager = roleManager;
            _userManager = userManager;
        }

        public async Task SeedAdminUserAsync()
        {
            // Seed roles
            await SeedRoleAsync("SuperAdmin");
            await SeedRoleAsync("Admin");
            await SeedRoleAsync("HR");
            await SeedRoleAsync("Employee");

            // Seed the SuperAdmin user
            var superAdminUser = await _userManager.FindByEmailAsync("superadmin@admin.com");
            if (superAdminUser == null)
            {
                var superAdmin = new ApplicationUser
                {
                    UserName = "superadmin@admin.com",
                    Email = "superadmin@admin.com",
                    FirstName = "Super",
                    LastName = "Admin", 
                    Address = "123 Admin St",
                    isdelete = false,
                    EmailConfirmed = true,
                };

                var result = await _userManager.CreateAsync(superAdmin, "SuperAdmin@123");
                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(superAdmin, "SuperAdmin");
                }
            }
        }


        private async Task SeedRoleAsync(string roleName)
        {
            if (!await _roleManager.RoleExistsAsync(roleName))
            {
                var role = new IdentityRole
                {
                    Name = roleName,
                    NormalizedName = roleName.ToUpper(),
                    ConcurrencyStamp = Guid.NewGuid().ToString()
                };

                await _roleManager.CreateAsync(role);

            }
        }
    }
}
