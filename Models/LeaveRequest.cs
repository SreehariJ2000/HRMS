using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using HRMS.Models.Enums;

namespace HRMS.Models
{
    [Table("LeaveRequest", Schema = "Transaction")]
    public class LeaveRequest : AuditedEntity
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int UserId { get; set; }

        [Required]
        public LeaveType LeaveType { get; set; }

        [Required]
        [Column(TypeName = "date")]
        public DateTime FromDate { get; set; }

        [Required]
        [Column(TypeName = "date")]
        public DateTime ToDate { get; set; }

        [Required]
        [Column(TypeName = "decimal(5,1)")]
        public decimal RequestedDays { get; set; }

        public bool IsHalfDay { get; set; } = false;

        public string Reason { get; set; } = string.Empty;

        [Required]
        public LeaveStatus Status { get; set; } = LeaveStatus.Pending;

        public string? AdminRemarks { get; set; }

        [ForeignKey("UserId")]
        public User User { get; set; } = null!;
    }
}
