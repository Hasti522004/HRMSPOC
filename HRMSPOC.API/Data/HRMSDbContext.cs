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
        public DbSet<UserOrganization> UserOrganizations { get; set; } // DbSet for the join entity
        public DbSet<UserWithRoleDto> UserWithRoleDtos { get; set; } // This is not a typical DbSet, but allows for projection from raw SQL.

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure Identity Tables, if necessary
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

            SeedRoles(modelBuilder);
        }

        private static void SeedRoles(ModelBuilder builder)
        {
            builder.Entity<IdentityRole>().HasData(
                new IdentityRole
                {
                    Id = Guid.NewGuid().ToString(),
                    Name = "SuperAdmin",
                    NormalizedName = "SUPERADMIN".ToUpper(),
                    ConcurrencyStamp = Guid.NewGuid().ToString()
                },
                new IdentityRole
                {
                    Id = Guid.NewGuid().ToString(),
                    Name = "Admin",
                    NormalizedName = "ADMIN".ToUpper(),
                    ConcurrencyStamp = Guid.NewGuid().ToString()
                },
                new IdentityRole
                {
                    Id = Guid.NewGuid().ToString(),
                    Name = "HR",
                    NormalizedName = "HR".ToUpper(),
                    ConcurrencyStamp = Guid.NewGuid().ToString()
                },
                new IdentityRole
                {
                    Id = Guid.NewGuid().ToString(),
                    Name = "Employee",
                    NormalizedName = "EMPLOYEE".ToUpper(),
                    ConcurrencyStamp = Guid.NewGuid().ToString()
                }
            );
        }
    }
}
