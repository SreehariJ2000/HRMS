namespace HRMS.ViewModels
{
    public class AdminDashboardVM
    {
        public int TotalEmployees { get; set; }
        public int PendingRequests { get; set; }
        public int ApprovedToday { get; set; }
        public int RejectedToday { get; set; }
        public List<LeaveRequestDetailVM> PendingLeaveRequests { get; set; } = new();
    }
}
