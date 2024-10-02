using HRMSPOC.API.DTOs;
using HRMSPOC.API.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace HRMSPOC.API.Data
{
    public class HRMSDbContext : IdentityDbContext<ApplicationUser>
    {
        public HRMSDbContext(DbContextOptions<HRMSDbContext> options) : base(options)
        {
        }

        public DbSet<Organization> Organization { get; set; }
        public DbSet<UserOrganization> UserOrganizations { get; set; }
        public DbSet<UserWithRoleDto> UserWithRoleDtos { get; set; } // Allows for projection from raw SQL for running a stored procedure

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            SeedData(modelBuilder);

            modelBuilder.Entity<ApplicationUser>(entity =>
            {
                entity.ToTable(name: "Users");
            });

            // Configure the composite key for UserOrganization
            modelBuilder.Entity<UserOrganization>()
                .HasKey(uo => new { uo.UserId, uo.OrganizationId });

            // Define relationships
            modelBuilder.Entity<UserOrganization>()
                .HasOne(uo => uo.ApplicationUser)
                .WithMany(u => u.UserOrganizations)
                .HasForeignKey(uo => uo.UserId);

            modelBuilder.Entity<UserOrganization>()
                .HasOne(uo => uo.Organization)
                .WithMany(o => o.UserOrganizations)
                .HasForeignKey(uo => uo.OrganizationId);
        }
        private void SeedData(ModelBuilder modelBuilder)
        {
            // Seed roles
            var superAdminRoleId = Guid.NewGuid().ToString();
            var adminRoleId = Guid.NewGuid().ToString();
            var hrRoleId = Guid.NewGuid().ToString();
            var employeeRoleId = Guid.NewGuid().ToString();

            // Check if roles exist before seeding
            modelBuilder.Entity<IdentityRole>().HasData(
                new IdentityRole { Id = superAdminRoleId, Name = "SuperAdmin", NormalizedName = "SUPERADMIN" },
                new IdentityRole { Id = adminRoleId, Name = "Admin", NormalizedName = "ADMIN" },
                new IdentityRole { Id = hrRoleId, Name = "HR", NormalizedName = "HR" },
                new IdentityRole { Id = employeeRoleId, Name = "Employee", NormalizedName = "EMPLOYEE" }
            );
            var passwordHasher = new PasswordHasher<ApplicationUser>();

            // Seed the SuperAdmin user
            var superAdminId = Guid.NewGuid().ToString();
            modelBuilder.Entity<ApplicationUser>().HasData(
                new ApplicationUser
                {
                    Id = superAdminId,
                    UserName = "superadmin@admin.com",
                    Email = "superadmin@admin.com",
                    FirstName = "Super",
                    LastName = "Admin",
                    Address = "123 Admin St",
                    IsDeleted = false,
                    EmailConfirmed = true,
                    NormalizedEmail = "SUPERADMIN@ADMIN.COM",
                    NormalizedUserName = "SUPERADMIN@ADMIN.COM",
                    PasswordHash = passwordHasher.HashPassword(null, "SuperAdmin@123")
                }
            );

            // Assign SuperAdmin role to the user
            modelBuilder.Entity<IdentityUserRole<string>>().HasData(
                new IdentityUserRole<string>
                {
                    UserId = superAdminId,
                    RoleId = superAdminRoleId
                }
            );
        }

    }
}
