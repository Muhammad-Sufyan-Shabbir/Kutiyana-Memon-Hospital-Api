using Kutiyana_Memon_Hospital_Api.API.Entities;
using Kutiyana_Memon_Hospital_Api.API.Services.GetCurrentUser;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace Kutiyana_Memon_Hospital_Api.API.Data
{
    public class ApplicationDbContext : DbContext
    {
        private readonly IUserContextService _userContextService;
        public ApplicationDbContext(
            DbContextOptions<ApplicationDbContext> options,
            IUserContextService? userContextService = null
        ) : base(options)
        {
            _userContextService = userContextService;
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            var entries = ChangeTracker
                .Entries<BaseEntity>();

            foreach (var entry in entries)
            {
                var now = DateTime.UtcNow;
                var userId = _userContextService.UserId;

                if (entry.State == EntityState.Added)
                {
                    entry.Entity.CreatedOn = now;
                    entry.Entity.CreatedBy = _userContextService.UserId ?? 0;
                }
                else if (entry.State == EntityState.Modified)
                {
                    entry.Entity.ModifiedOn = now;
                    entry.Entity.ModifiedBy = _userContextService.UserId ?? 0;
                }
            }

            return await base.SaveChangesAsync(cancellationToken);
        }

        public DbSet<Company> company { get; set; }
        public DbSet<Role> role { get; set; }
        public DbSet<RoleModuleAccess> roleModuleAccess { get; set; }
        public DbSet<User> ApplicationUser { get; set; }
        public DbSet<Entities.Module> Modules { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // 🚫 Ignore System.Reflection.CustomAttributeData conflict
            modelBuilder.Ignore<CustomAttributeData>();

            // User -> Role (Restrict delete behavior)
            modelBuilder.Entity<User>()
                .HasOne(u => u.Role)
                .WithMany(r => r.Users)
                .HasForeignKey(u => u.RoleId)
                .OnDelete(DeleteBehavior.Restrict);

            // User -> Company (Cascade allowed)
            modelBuilder.Entity<User>()
                .HasOne(u => u.company)
                .WithMany(c => c.Users)
                .HasForeignKey(u => u.CompanyId)
                .OnDelete(DeleteBehavior.Cascade);

            // Role -> Company (Cascade allowed)
            modelBuilder.Entity<Role>()
                .HasOne(r => r.Company)
                .WithMany(c => c.Roles)
                .HasForeignKey(r => r.CompanyId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Role>()
               .HasMany(r => r.ModuleAccesses)
               .WithOne(ma => ma.Role)
               .HasForeignKey(ma => ma.RoleId)
               .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
