using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using HRMS.Models.Enums;

namespace HRMS.Models
{
    [Table("LeaveBalance", Schema = "Master")]
    public class LeaveBalance : AuditedEntity
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int UserId { get; set; }

        [Required]
        public LeaveType LeaveType { get; set; }

        [Required]
        public int Year { get; set; }

        [Required]
        public int TotalAllocated { get; set; }

        [Required]
        public int Used { get; set; }

        [NotMapped]
        public int Balance => TotalAllocated - Used;

        [ForeignKey("UserId")]
        public User User { get; set; } = null!;
    }
}
