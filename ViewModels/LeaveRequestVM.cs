using System.ComponentModel.DataAnnotations;
using HRMS.Models.Enums;

namespace HRMS.ViewModels
{
    public class LeaveRequestVM
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Leave Type is required")]
        [Display(Name = "Leave Type")]
        public LeaveType LeaveType { get; set; }

        [Required(ErrorMessage = "From Date is required")]
        [DataType(DataType.Date)]
        [Display(Name = "From Date")]
        public DateTime FromDate { get; set; } = DateTime.Today;

        [Required(ErrorMessage = "To Date is required")]
        [DataType(DataType.Date)]
        [Display(Name = "To Date")]
        public DateTime ToDate { get; set; } = DateTime.Today;

        [Required(ErrorMessage = "Reason is required")]
        [StringLength(500, MinimumLength = 10, ErrorMessage = "Reason must be between 10 and 500 characters")]
        [Display(Name = "Reason")]
        public string Reason { get; set; } = string.Empty;
    }
}
