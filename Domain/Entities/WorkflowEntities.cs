using Postgrest.Attributes;
using Postgrest.Models;

namespace ntcc_admin_blazor.Domain.Entities
{
    [Table("stage_types")]
    public class StageTypeEntity : DomainBase
    {
        [PrimaryKey("id", false)]
        public string Id { get; set; } = Guid.NewGuid().ToString();

        [Column("stage_key")]
        public string StageKey { get; set; } = string.Empty; // e.g., minor_project

        [Column("name")]
        public string Name { get; set; } = string.Empty;

        [Column("description")]
        public string? Description { get; set; }

        [Column("program_id")]
        public string ProgramId { get; set; } = string.Empty;
    }

    [Table("workflow_stages")]
    public class WorkflowStageEntity : DomainBase
    {
        [PrimaryKey("id", false)]
        public string Id { get; set; } = Guid.NewGuid().ToString();

        [Column("stage_type_id")]
        public string StageTypeId { get; set; } = string.Empty;

        [Column("name")]
        public string Name { get; set; } = string.Empty;

        [Column("description")]
        public string? Description { get; set; }

        [Column("stage_key")]
        public string StageKey { get; set; } = string.Empty;
    }

    [Table("workflow_steps")]
    public class WorkflowStepEntity : DomainBase
    {
        [PrimaryKey("id", false)]
        public string Id { get; set; } = Guid.NewGuid().ToString();

        [Column("workflow_stage_id")]
        public string WorkflowStageId { get; set; } = string.Empty;

        [Column("step_key")]
        public string StepKey { get; set; } = string.Empty;

        [Column("step_name")]
        public string StepName { get; set; } = string.Empty;

        [Column("step_order")]
        public int OrderIndex { get; set; }

        [Column("requires_submission")]
        public bool RequiresSubmission { get; set; } = false;

        [Column("requires_meeting")]
        public bool RequiresMeeting { get; set; } = false;

        [Column("requires_report")]
        public bool RequiresReport { get; set; } = false;

        [Column("requires_ppt")]
        public bool RequiresPpt { get; set; } = false;
    }

    [Table("stage_requirements")]
    public class StageRequirementEntity : DomainBase
    {
        [PrimaryKey("id", false)]
        public string Id { get; set; } = Guid.NewGuid().ToString();

        [Column("stage_type_id")]
        public string StageTypeId { get; set; } = string.Empty;

        [Column("required_meetings")]
        public int RequiredMeetings { get; set; } = 0;

        [Column("required_reports")]
        public int RequiredReports { get; set; } = 0;

        [Column("required_presentations")]
        public int RequiredPresentations { get; set; } = 0;

        [Column("required_publications")]
        public int RequiredPublications { get; set; } = 0;
    }

    [Table("student_workflow_steps")]
    public class StudentWorkflowStepEntity : DomainBase
    {
        [PrimaryKey("id", false)]
        public string Id { get; set; } = Guid.NewGuid().ToString();

        [Column("student_id")]
        public string StudentId { get; set; } = string.Empty;

        [Column("workflow_step_id")]
        public string WorkflowStepId { get; set; } = string.Empty;

        [Column("status")]
        public string Status { get; set; } = "pending"; // pending, submitted, approved, rejected

        [Column("submitted_at")]
        public DateTime? SubmittedAt { get; set; }
    }
}
