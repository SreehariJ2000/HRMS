namespace HRMS.ViewModels
{
    public class EmployeeDashboardVM
    {
        public string EmployeeName { get; set; } = string.Empty;
        public string EmployeeCode { get; set; } = string.Empty;
        public string Department { get; set; } = string.Empty;
        public List<LeaveBalanceVM> LeaveBalances { get; set; } = new();
        public List<LeaveRequestDetailVM> RecentRequests { get; set; } = new();
    }
}
