using Postgrest.Attributes;
using Postgrest.Models;
using System;

namespace ntcc_admin_blazor.Domain.Entities
{
    [Table("student_activity_logs")]
    public class StudentActivityLogEntity : BaseModel
    {
        [PrimaryKey("id", false)]
        public string Id { get; set; } = Guid.NewGuid().ToString();

        [Column("student_id")]
        public string StudentId { get; set; } = string.Empty;

        [Column("action")]
        public string Action { get; set; } = string.Empty;

        [Column("metadata")]
        public string Metadata { get; set; } = "{}";

        [Column("created_at")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
