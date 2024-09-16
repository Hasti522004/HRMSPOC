using HRMSPOC.API.Models;
using Microsoft.EntityFrameworkCore;

namespace HRMSPOC.API.Data
{
    public class HRMSDbContext : DbContext
    {
        public HRMSDbContext(DbContextOptions<HRMSDbContext> options) : base(options)
        {
            
        }

        public DbSet<Organization> Organization { get; set; }
        public DbSet<Employee> Employees { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Employee>()
                .HasOne(e => e.Organization)
                .WithMany(o => o.Employees)
                .HasForeignKey(e => e.OrganizationId);

        }
    }
}
