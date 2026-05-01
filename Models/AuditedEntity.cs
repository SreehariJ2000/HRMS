using System;

namespace HRMS.Models
{
    public abstract class AuditedEntity
    {
        public int? CreatedUserId { get; set; }
        
        public DateTime CreationTime { get; set; } = DateTime.UtcNow;

        public bool IsDeleted { get; set; }

        public int? ModifiedUserId { get; set; }

        public DateTime? ModifiedDate { get; set; }
    }
}
