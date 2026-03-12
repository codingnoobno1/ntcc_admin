using Postgrest.Attributes;
using Postgrest.Models;

namespace ntcc_admin_blazor.Domain.Entities
{
    [Table("submissions")]
    public class SubmissionEntity : DomainBase
    {
        [PrimaryKey("id", false)]
        public Guid Id { get; set; } = Guid.NewGuid();

        [Column("student_id")]
        public Guid StudentId { get; set; }

        [Column("stage_id")]
        public Guid StageId { get; set; }

        [Column("type")]
        public string Type { get; set; } = string.Empty; // proposal, draft_report, etc.

        [Column("status")]
        public string Status { get; set; } = "pending";

        [Column("file_url")]
        public string? FileUrl { get; set; }

        [Column("reviewed_by")]
        public Guid? ReviewedBy { get; set; }

        [Column("submitted_at")]
        public DateTime SubmittedAt { get; set; } = DateTime.UtcNow;
    }

    [Table("activity_logs")]
    public class ActivityLogEntity : DomainBase
    {
        [PrimaryKey("id", false)]
        public Guid Id { get; set; } = Guid.NewGuid();

        [Column("user_id")]
        public Guid? UserId { get; set; }

        [Column("action")]
        public string Action { get; set; } = string.Empty;

        [Column("entity")]
        public string Entity { get; set; } = string.Empty;

        [Column("entity_id")]
        public Guid? EntityId { get; set; }

        [Column("metadata")]
        public string? Metadata { get; set; } // JSON string
    }

    [Table("notifications")]
    public class NotificationEntity : DomainBase
    {
        [PrimaryKey("id", false)]
        public Guid Id { get; set; } = Guid.NewGuid();

        [Column("user_id")]
        public Guid UserId { get; set; }

        [Column("message")]
        public string Message { get; set; } = string.Empty;

        [Column("type")]
        public string Type { get; set; } = "info";

        [Column("is_read")]
        public bool IsRead { get; set; } = false;
    }


    [Table("supervisor_meetings")]
    public class SupervisorMeetingEntity : DomainBase
    {
        [PrimaryKey("id", false)]
        public Guid Id { get; set; } = Guid.NewGuid();

        [Column("student_id")]
        public Guid StudentId { get; set; }

        [Column("faculty_id")]
        public Guid FacultyId { get; set; }

        [Column("meeting_date")]
        public DateTime MeetingDate { get; set; } = DateTime.UtcNow;

        [Column("summary")]
        public string? Summary { get; set; }

        [Column("progress_score")]
        public int ProgressScore { get; set; } // weekly log score
    }

    [Table("meeting_reports")]
    public class MeetingReportEntity : DomainBase
    {
        [PrimaryKey("id", false)]
        public Guid Id { get; set; } = Guid.NewGuid();

        [Column("meeting_id")]
        public Guid MeetingId { get; set; }

        [Column("report_url")]
        public string? ReportUrl { get; set; }

        [Column("remarks")]
        public string? Remarks { get; set; }
    }

    [Table("workflow_submissions")]
    public class WorkflowSubmissionEntity : DomainBase
    {
        [PrimaryKey("id", false)]
        public Guid Id { get; set; } = Guid.NewGuid();

        [Column("student_id")]
        public Guid StudentId { get; set; }

        [Column("workflow_step_id")]
        public Guid WorkflowStepId { get; set; }

        [Column("file_url")]
        public string? FileUrl { get; set; }

        [Column("gdrive_link")]
        public string? GDriveLink { get; set; }

        [Column("submitted_at")]
        public DateTime SubmittedAt { get; set; } = DateTime.UtcNow;
    }
}
