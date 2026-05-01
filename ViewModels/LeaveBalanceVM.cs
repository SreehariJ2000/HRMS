using HRMS.Models.Enums;

namespace HRMS.ViewModels
{
    public class LeaveBalanceVM
    {
        public LeaveType LeaveType { get; set; }
        public string LeaveTypeName { get; set; } = string.Empty;
        public int TotalAllocated { get; set; }
        public int Used { get; set; }
        public int Balance { get; set; }
    }
}
