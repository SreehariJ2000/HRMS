using System.ComponentModel.DataAnnotations;

namespace HRMS.ViewModels
{
    public class LeaveApprovalVM
    {
        public int LeaveRequestId { get; set; }

        [StringLength(500)]
        [Display(Name = "Admin Remarks")]
        public string? AdminRemarks { get; set; }
    }
}
