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
        Task<List<LeaveRequestDetailVM>> GetLeaveHistoryAsync();
        Task<List<LeaveRequestDetailVM>> GetAdminLeaveHistoryAsync();
        Task<List<LeaveRequestDetailVM>> GetAllPendingRequestsAsync();
        Task<List<LeaveBalanceVM>> GetLeaveBalancesAsync();
    }
}
