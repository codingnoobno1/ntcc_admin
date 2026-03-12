using Postgrest.Attributes;
using Postgrest.Models;
using System;
using System.Collections.Generic;

namespace ntcc_admin_blazor.Domain.Entities
{
    [Table("project_groups")]
    public class ProjectGroupEntity : BaseModel
    {
        [PrimaryKey("id", false)]
        public string Id { get; set; } = Guid.NewGuid().ToString();

        [Column("stage_id")]
        public string StageId { get; set; } = string.Empty;

        [Column("project_title")]
        public string ProjectTitle { get; set; } = string.Empty;

        // abstract, outline, etc.
        [Column("project_description")]
        public string ProjectDescription { get; set; } = string.Empty;

        [Column("project_type")]
        public string ProjectType { get; set; } = "Minor";

        [Column("description")]
        public string Description { get; set; } = string.Empty;

        [Column("semester")]
        public int Semester { get; set; }

        [Column("batch_id")]
        public string BatchId { get; set; } = string.Empty;

        [Column("submission_status")]
        public string SubmissionStatus { get; set; } = "pending";

        // pending, approved, rejected, draft
        [Column("status")]
        public string Status { get; set; } = "draft";

        [Column("mentor_id")]
        public string? MentorId { get; set; }

        [Column("created_at")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [Column("updated_at")]
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        [Column("gdrive_link")]
        public string? GDriveLink { get; set; }
    }
}
