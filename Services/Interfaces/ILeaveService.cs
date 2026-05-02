using HRMS.ViewModels;

namespace HRMS.Services.Interfaces
{
    public interface ILeaveService
    {
        Task<EmployeeDashboardVM> GetEmployeeDashboardAsync();
        Task<AdminDashboardVM> GetAdminDashboardAsync();
        Task<(bool Success, string Message)> ApplyLeaveAsync(LeaveRequestVM model);
        Task<(bool Success, string Message)> ApproveLeaveAsync(int leaveRequestId, string? adminRemarks);
        Task<(bool Success, string Message)> RejectLeaveAsync(int leaveRequestId, string? adminRemarks);
        Task<(bool Success, string Message)> CancelLeaveAsync(int leaveRequestId);
        Task<List<LeaveRequestDetailVM>> GetLeaveHistoryAsync();
        Task<HRMS.Helpers.PaginatedList<LeaveRequestDetailVM>> GetPaginatedLeaveHistoryAsync(string? searchString, int pageNumber, int pageSize);
        Task<List<LeaveRequestDetailVM>> GetAdminLeaveHistoryAsync();
        Task<HRMS.Helpers.PaginatedList<LeaveRequestDetailVM>> GetPaginatedAdminLeaveHistoryAsync(string? searchString, int pageNumber, int pageSize);
        Task<List<LeaveRequestDetailVM>> GetAllPendingRequestsAsync();
        Task<HRMS.Helpers.PaginatedList<LeaveRequestDetailVM>> GetPaginatedPendingRequestsAsync(string? searchString, int pageNumber, int pageSize);
        Task<List<LeaveBalanceVM>> GetLeaveBalancesAsync();
    }
}
