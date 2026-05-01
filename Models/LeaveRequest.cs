using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using HRMS.Models.Enums;

namespace HRMS.Models
{
    public class LeaveRequest
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
        public int RequestedDays { get; set; }

        [MaxLength(500)]
        public string Reason { get; set; } = string.Empty;

        [Required]
        public LeaveStatus Status { get; set; } = LeaveStatus.Pending;

        [MaxLength(500)]
        public string? AdminRemarks { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // Navigation property
        [ForeignKey("UserId")]
        public User User { get; set; } = null!;
    }
}
