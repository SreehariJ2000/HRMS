using HRMS.Data;
using HRMS.Models;
using HRMS.Models.Enums;
using HRMS.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace HRMS.Repositories.Implementations
{
    public class LeaveBalanceRepository : ILeaveBalanceRepository
    {
        private readonly AppDbContext _context;

        public LeaveBalanceRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<LeaveBalance?> GetAsync(int userId, LeaveType leaveType, int year)
        {
            return await _context.LeaveBalances
                .FirstOrDefaultAsync(lb => lb.UserId == userId && lb.LeaveType == leaveType && lb.Year == year);
        }

        public async Task<List<LeaveBalance>> GetByUserAsync(int userId, int year)
        {
            return await _context.LeaveBalances
                .AsNoTracking()
                .Where(lb => lb.UserId == userId && lb.Year == year)
                .ToListAsync();
        }

        public async Task AddAsync(LeaveBalance leaveBalance)
        {
            _context.LeaveBalances.Add(leaveBalance);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(LeaveBalance leaveBalance)
        {
            _context.LeaveBalances.Update(leaveBalance);
            await _context.SaveChangesAsync();
        }

        public async Task InitializeBalancesForUserAsync(int userId, int year)
        {
            var existingBalances = await _context.LeaveBalances
                .AnyAsync(lb => lb.UserId == userId && lb.Year == year);

            if (!existingBalances)
            {
                var balances = new List<LeaveBalance>
                {
                    new LeaveBalance { UserId = userId, LeaveType = LeaveType.CasualLeave, Year = year, TotalAllocated = 12, Used = 0 },
                    new LeaveBalance { UserId = userId, LeaveType = LeaveType.SickLeave, Year = year, TotalAllocated = 10, Used = 0 },
                    new LeaveBalance { UserId = userId, LeaveType = LeaveType.EarnedLeave, Year = year, TotalAllocated = 15, Used = 0 }
                };

                _context.LeaveBalances.AddRange(balances);
                await _context.SaveChangesAsync();
            }
        }
    }
}
