using Postgrest.Attributes;
using Postgrest.Models;

namespace ntcc_admin_blazor.Domain.Entities
{
    [Table("submissions")]
    public class SubmissionEntity : DomainBase
    {
        [PrimaryKey("id", false)]
        public string Id { get; set; } = Guid.NewGuid().ToString();

        [Column("student_id")]
        public string StudentId { get; set; } = string.Empty;

        [Column("stage_id")]
        public string StageId { get; set; } = string.Empty;

        [Column("type")]
        public string Type { get; set; } = string.Empty; // proposal, draft_report, etc.

        [Column("status")]
        public string Status { get; set; } = "pending";

        [Column("file_url")]
        public string? FileUrl { get; set; }

        [Column("reviewed_by")]
        public string? ReviewedBy { get; set; }

        [Column("submitted_at")]
        public DateTime SubmittedAt { get; set; } = DateTime.UtcNow;
    }

    [Table("activity_logs")]
    public class ActivityLogEntity : DomainBase
    {
        [PrimaryKey("id", false)]
        public string Id { get; set; } = Guid.NewGuid().ToString();

        [Column("user_id")]
        public string? UserId { get; set; }

        [Column("action")]
        public string Action { get; set; } = string.Empty;

        [Column("entity")]
        public string Entity { get; set; } = string.Empty;

        [Column("entity_id")]
        public string? EntityId { get; set; }

        [Column("metadata")]
        public string? Metadata { get; set; } // JSON string
    }

    [Table("notifications")]
    public class NotificationEntity : DomainBase
    {
        [PrimaryKey("id", false)]
        public string Id { get; set; } = Guid.NewGuid().ToString();

        [Column("user_id")]
        public string UserId { get; set; } = string.Empty;

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
        public string Id { get; set; } = Guid.NewGuid().ToString();

        [Column("student_id")]
        public string StudentId { get; set; } = string.Empty;

        [Column("faculty_id")]
        public string FacultyId { get; set; } = string.Empty;

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
        public string Id { get; set; } = Guid.NewGuid().ToString();

        [Column("meeting_id")]
        public string MeetingId { get; set; } = string.Empty;

        [Column("report_url")]
        public string? ReportUrl { get; set; }

        [Column("remarks")]
        public string? Remarks { get; set; }
    }

    [Table("workflow_submissions")]
    public class WorkflowSubmissionEntity : DomainBase
    {
        [PrimaryKey("id", false)]
        public string Id { get; set; } = Guid.NewGuid().ToString();

        [Column("student_id")]
        public string StudentId { get; set; } = string.Empty;

        [Column("workflow_step_id")]
        public string WorkflowStepId { get; set; } = string.Empty;

        [Column("file_url")]
        public string? FileUrl { get; set; }

        [Column("gdrive_link")]
        public string? GDriveLink { get; set; }

        [Column("submitted_at")]
        public DateTime SubmittedAt { get; set; } = DateTime.UtcNow;
    }
}
