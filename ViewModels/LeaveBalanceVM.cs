using HRMS.Models.Enums;

namespace HRMS.ViewModels
{
    public class LeaveBalanceVM
    {
        public LeaveType LeaveType { get; set; }
        public string LeaveTypeName { get; set; } = string.Empty;
        public decimal TotalAllocated { get; set; }
        public decimal Used { get; set; }
        public decimal Balance { get; set; }
    }
}
