using HRMS.Models;
using HRMS.Models.Enums;

namespace HRMS.Repositories.Interfaces
{
    public interface ILeaveRequestRepository
    {
        Task<LeaveRequest?> GetByIdAsync(int id);
        Task<List<LeaveRequest>> GetByUserIdAsync(int userId);
        Task<List<LeaveRequest>> GetPendingRequestsAsync();
        IQueryable<LeaveRequest> GetPendingRequestsQueryable();
        Task<List<LeaveRequest>> GetAllRequestsAsync();
        IQueryable<LeaveRequest> GetAllRequestsQueryable();
        IQueryable<LeaveRequest> GetByUserIdQueryable(int userId);
        Task<int> GetCountByStatusAsync(LeaveStatus status);
        Task<int> GetCountByStatusAndDateAsync(LeaveStatus status, DateTime date);
        Task AddAsync(LeaveRequest leaveRequest);
        Task UpdateAsync(LeaveRequest leaveRequest);
        Task DeleteAsync(LeaveRequest leaveRequest);
        Task<bool> HasOverlappingRequestAsync(int userId, DateTime fromDate, DateTime toDate, int? excludeRequestId = null);
    }
}
