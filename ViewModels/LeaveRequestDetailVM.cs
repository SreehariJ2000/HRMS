using HRMS.Models.Enums;

namespace HRMS.ViewModels
{
    public class LeaveRequestDetailVM
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string EmployeeName { get; set; } = string.Empty;
        public string EmployeeCode { get; set; } = string.Empty;
        public string Department { get; set; } = string.Empty;
        public LeaveType LeaveType { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public int RequestedDays { get; set; }
        public decimal RequestedDaysDecimal { get; set; }
        public bool IsHalfDay { get; set; }
        public string Reason { get; set; } = string.Empty;
        public LeaveStatus Status { get; set; }
        public string? AdminRemarks { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
