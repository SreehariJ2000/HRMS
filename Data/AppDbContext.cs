using Microsoft.EntityFrameworkCore;
using HRMS.Models;
using HRMS.Models.Enums;

using HRMS.Services.Interfaces;

namespace HRMS.Data
{
    public class AppDbContext : DbContext
    {
        private readonly IApplicationUserService _applicationUserService;

        public AppDbContext(
            DbContextOptions<AppDbContext> options,
            IApplicationUserService applicationUserService) : base(options)
        {
            _applicationUserService = applicationUserService;
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            var userId = _applicationUserService.GetUserId();
            var now = DateTime.UtcNow;

            foreach (var entry in ChangeTracker.Entries<AuditedEntity>())
            {
                if (entry.State == EntityState.Added)
                {
                    entry.Entity.CreatedUserId = userId;
                    entry.Entity.CreationTime = now;
                }
                else if (entry.State == EntityState.Modified)
                {
                    entry.Entity.ModifiedUserId = userId;
                    entry.Entity.ModifiedDate = now;
                }
            }

            return base.SaveChangesAsync(cancellationToken);
        }

        public DbSet<User> Users { get; set; }
        public DbSet<LeaveBalance> LeaveBalances { get; set; }
        public DbSet<LeaveRequest> LeaveRequests { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // User configuration
            modelBuilder.Entity<User>(entity =>
            {
                entity.HasIndex(u => u.EmployeeCode).IsUnique();
                entity.HasIndex(u => u.Email).IsUnique();

                entity.Property(u => u.Role)
                      .HasConversion<string>()
                      .HasMaxLength(20);
            });

            // LeaveBalance configuration
            modelBuilder.Entity<LeaveBalance>(entity =>
            {
                entity.Property(lb => lb.LeaveType)
                      .HasConversion<string>()
                      .HasMaxLength(20);

                // Composite unique index: one record per user per leave type per year
                entity.HasIndex(lb => new { lb.UserId, lb.LeaveType, lb.Year }).IsUnique();

                entity.HasOne(lb => lb.User)
                      .WithMany(u => u.LeaveBalances)
                      .HasForeignKey(lb => lb.UserId)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            // LeaveRequest configuration
            modelBuilder.Entity<LeaveRequest>(entity =>
            {
                entity.Property(lr => lr.LeaveType)
                      .HasConversion<string>()
                      .HasMaxLength(20);

                entity.Property(lr => lr.Status)
                      .HasConversion<string>()
                      .HasMaxLength(20);

                entity.HasOne(lr => lr.User)
                      .WithMany(u => u.LeaveRequests)
                      .HasForeignKey(lr => lr.UserId)
                      .OnDelete(DeleteBehavior.Cascade);
            });
        }
    }
}
