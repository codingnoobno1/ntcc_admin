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

    [Table("projects")]
    public class ProjectEntity : DomainBase
    {
        [PrimaryKey("id", false)]
        public string Id { get; set; } = Guid.NewGuid().ToString();

        [Column("title")]
        public string Title { get; set; } = string.Empty;

        [Column("description")]
        public string Description { get; set; } = string.Empty;

        [Column("project_type")]
        public string ProjectType { get; set; } = "minor";

        [Column("stage_id")]
        public string? StageId { get; set; }

        [Column("faculty_mentor_id")]
        public string? FacultyMentorId { get; set; }

        [Column("proposal_status")]
        public string ProposalStatus { get; set; } = "draft";
    }

    [Table("project_members")]
    public class ProjectMemberEntity : DomainBase
    {
        [PrimaryKey("id", false)]
        public string Id { get; set; } = Guid.NewGuid().ToString();

        [Column("project_id")]
        public string ProjectId { get; set; } = string.Empty;

        [Column("student_id")]
        public string StudentId { get; set; } = string.Empty;

        [Column("role")]
        public string Role { get; set; } = "member";
    }

}
