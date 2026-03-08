using Postgrest.Attributes;
using Postgrest.Models;

namespace ntcc_admin_blazor.Domain.Entities
{
    [Table("workflow_stages")]
    public class WorkflowStageEntity : DomainBase
    {
        [PrimaryKey("id", false)]
        public long Id { get; set; }

        [Column("stage_key")]
        public string StageKey { get; set; }

        [Column("description")]
        public string Description { get; set; }
    }

    [Table("workflow_steps")]
    public class WorkflowStepEntity : DomainBase
    {
        [PrimaryKey("id", false)]
        public long Id { get; set; }

        [Column("stage_id")]
        public long StageId { get; set; }

        [Column("step_key")]
        public string StepKey { get; set; }

        [Column("order_index")]
        public int OrderIndex { get; set; }
    }

    [Table("step_components")]
    public class StepComponentEntity : BaseModel
    {
        [Column("step_id")]
        public long StepId { get; set; }

        [Column("component_id")]
        public long ComponentId { get; set; }
    }

    [Table("components_registry")]
    public class ComponentRegistryEntity : DomainBase
    {
        [PrimaryKey("id", false)]
        public long Id { get; set; }

        [Column("component_key")]
        public string ComponentKey { get; set; }

        [Column("type")]
        public string Type { get; set; } // metric, visualization, academic, evaluation, productivity

        [Column("description")]
        public string Description { get; set; }
    }

    [Table("student_workflow_steps")]
    public class StudentWorkflowStepEntity : DomainBase
    {
        [PrimaryKey("id", false)]
        public string Id { get; set; } = Guid.NewGuid().ToString();

        [Column("student_id")]
        public string StudentId { get; set; } = string.Empty;

        [Column("step_id")]
        public long StepId { get; set; }

        [Column("status")]
        public string Status { get; set; } = "pending"; // pending, submitted, approved, rejected

        [Column("submitted_at")]
        public DateTime? SubmittedAt { get; set; }
    }
}
