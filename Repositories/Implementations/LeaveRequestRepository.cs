using HRMS.Data;
using HRMS.Models;
using HRMS.Models.Enums;
using HRMS.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace HRMS.Repositories.Implementations
{
    public class LeaveRequestRepository : ILeaveRequestRepository
    {
        private readonly AppDbContext _context;

        public LeaveRequestRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<LeaveRequest?> GetByIdAsync(int id)
        {
            return await _context.LeaveRequests
                .Include(lr => lr.User)
                .FirstOrDefaultAsync(lr => lr.Id == id);
        }

        public async Task<List<LeaveRequest>> GetByUserIdAsync(int userId)
        {
            return await _context.LeaveRequests
                .AsNoTracking()
                .Where(lr => lr.UserId == userId)
                .OrderByDescending(lr => lr.CreatedAt)
                .ToListAsync();
        }

        public async Task<List<LeaveRequest>> GetPendingRequestsAsync()
        {
            return await _context.LeaveRequests
                .AsNoTracking()
                .Include(lr => lr.User)
                .Where(lr => lr.Status == LeaveStatus.Pending)
                .OrderBy(lr => lr.CreatedAt)
                .ToListAsync();
        }

        public async Task<List<LeaveRequest>> GetAllRequestsAsync()
        {
            return await _context.LeaveRequests
                .AsNoTracking()
                .Include(lr => lr.User)
                .OrderByDescending(lr => lr.CreatedAt)
                .ToListAsync();
        }

        public async Task<int> GetCountByStatusAsync(LeaveStatus status)
        {
            return await _context.LeaveRequests
                .CountAsync(lr => lr.Status == status);
        }

        public async Task<int> GetCountByStatusAndDateAsync(LeaveStatus status, DateTime date)
        {
            return await _context.LeaveRequests
                .CountAsync(lr => lr.Status == status && lr.CreatedAt.Date == date.Date);
        }

        public async Task AddAsync(LeaveRequest leaveRequest)
        {
            _context.LeaveRequests.Add(leaveRequest);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(LeaveRequest leaveRequest)
        {
            _context.LeaveRequests.Update(leaveRequest);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> HasOverlappingRequestAsync(int userId, DateTime fromDate, DateTime toDate, int? excludeRequestId = null)
        {
            return await _context.LeaveRequests
                .AnyAsync(lr => lr.UserId == userId
                    && lr.Status != LeaveStatus.Rejected
                    && lr.FromDate <= toDate
                    && lr.ToDate >= fromDate
                    && (!excludeRequestId.HasValue || lr.Id != excludeRequestId.Value));
        }
    }
}
