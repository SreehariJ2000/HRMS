using HRMS.ViewModels;

namespace HRMS.Services.Interfaces
{
    public interface ILeaveService
    {
        Task<EmployeeDashboardVM> GetEmployeeDashboardAsync(int userId);
        Task<AdminDashboardVM> GetAdminDashboardAsync();
        Task<(bool Success, string Message)> ApplyLeaveAsync(int userId, LeaveRequestVM model);
        Task<(bool Success, string Message)> ApproveLeaveAsync(int leaveRequestId, string? adminRemarks);
        Task<(bool Success, string Message)> RejectLeaveAsync(int leaveRequestId, string? adminRemarks);
        Task<List<LeaveRequestDetailVM>> GetLeaveHistoryAsync(int userId);
        Task<List<LeaveRequestDetailVM>> GetAllPendingRequestsAsync();
        Task<List<LeaveBalanceVM>> GetLeaveBalancesAsync(int userId);
    }
}
