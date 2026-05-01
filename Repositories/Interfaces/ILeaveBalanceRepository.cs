using HRMS.Models;
using HRMS.Models.Enums;

namespace HRMS.Repositories.Interfaces
{
    public interface ILeaveBalanceRepository
    {
        Task<LeaveBalance?> GetAsync(int userId, LeaveType leaveType, int year);
        Task<List<LeaveBalance>> GetByUserAsync(int userId, int year);
        Task AddAsync(LeaveBalance leaveBalance);
        Task UpdateAsync(LeaveBalance leaveBalance);
        Task InitializeBalancesForUserAsync(int userId, int year);
    }
}
