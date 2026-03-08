using Postgrest.Attributes;
using Postgrest.Models;
using System;

namespace ntcc_admin_blazor.Domain.Entities
{
    [Table("internships")]
    public class InternshipEntity : BaseModel
    {
        [PrimaryKey("id", false)]
        public string Id { get; set; } = Guid.NewGuid().ToString();

        [Column("student_id")]
        public string StudentId { get; set; } = string.Empty;

        [Column("company_id")]
        public string CompanyId { get; set; } = string.Empty;

        [Column("supervisor_name")]
        public string SupervisorName { get; set; } = string.Empty;

        [Column("supervisor_email")]
        public string SupervisorEmail { get; set; } = string.Empty;

        [Column("start_date")]
        public DateTime StartDate { get; set; }

        [Column("end_date")]
        public DateTime EndDate { get; set; }

        // Remote, Hybrid, In-Person
        [Column("mode")]
        public string Mode { get; set; } = "In-Person";

        // pending_noc, approved, completed, rejected
        [Column("status")]
        public string Status { get; set; } = "pending_noc";

        [Column("created_at")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
