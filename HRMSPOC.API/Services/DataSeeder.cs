using HRMSPOC.API.Models;
using Microsoft.AspNetCore.Identity;

namespace HRMSPOC.API.Services;

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
        if(! await _roleManager.RoleExistsAsync("Admin"))
        {
            await _roleManager.CreateAsync(new IdentityRole("Admin"));
        }
        var adminUser = await _userManager.FindByEmailAsync("admin@admin.com");
        if (adminUser == null)
        {
            var admin = new ApplicationUser
            {
                UserName = "admin@admin.com",
                Email = "admin@admin.com",
                EmailConfirmed = true,
                Role = "Admin"
            };
            var result = await _userManager.CreateAsync(admin,"Admin@123");
            if(result.Succeeded)
            {
                await _userManager.AddToRoleAsync(admin, "Admin");
            }
        }
    }
}
