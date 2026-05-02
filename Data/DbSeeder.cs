using HRMS.Models;
using HRMS.Models.Enums;
using Microsoft.EntityFrameworkCore;

namespace HRMS.Data
{
    public static class DbSeeder
    {
        public static async Task SeedAsync(IServiceProvider serviceProvider)
        {
            using var scope = serviceProvider.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();

            await context.Database.MigrateAsync();

            if (!await context.Users.AnyAsync())
            {
                var admin = new User
                {
                    EmployeeCode = "ADMIN001",
                    FirstName = "System",
                    LastName = "Administrator",
                    Email = "admin@hrms.com",
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword("Admin@123"),
                    Department = "Administration",
                    DateOfJoining = new DateTime(2024, 1, 1),
                    Role = UserRole.Admin
                };

                context.Users.Add(admin);
                await context.SaveChangesAsync();

                var currentYear = DateTime.UtcNow.Year;
                var leaveBalances = new List<LeaveBalance>
                {
                    new LeaveBalance
                    {
                        UserId = admin.Id,
                        LeaveType = LeaveType.CasualLeave,
                        Year = currentYear,
                        TotalAllocated = 12,
                        Used = 0
                    },
                    new LeaveBalance
                    {
                        UserId = admin.Id,
                        LeaveType = LeaveType.SickLeave,
                        Year = currentYear,
                        TotalAllocated = 10,
                        Used = 0
                    },
                    new LeaveBalance
                    {
                        UserId = admin.Id,
                        LeaveType = LeaveType.EarnedLeave,
                        Year = currentYear,
                        TotalAllocated = 15,
                        Used = 0
                    }
                };

                context.LeaveBalances.AddRange(leaveBalances);
                await context.SaveChangesAsync();
            }
        }
    }
}
