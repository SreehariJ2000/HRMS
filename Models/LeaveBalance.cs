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
        [Column(TypeName = "decimal(5,1)")]
        public decimal TotalAllocated { get; set; }

        [Required]
        [Column(TypeName = "decimal(5,1)")]
        public decimal Used { get; set; }

        [NotMapped]
        public decimal Balance => TotalAllocated - Used;

        [ForeignKey("UserId")]
        public User User { get; set; } = null!;
    }
}
