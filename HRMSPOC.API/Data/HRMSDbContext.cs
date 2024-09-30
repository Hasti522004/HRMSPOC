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
    }
}
