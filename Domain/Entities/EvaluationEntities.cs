using Postgrest.Attributes;
using Postgrest.Models;

namespace ntcc_admin_blazor.Domain.Entities
{
    [Table("evaluation_rubrics")]
    public class EvaluationRubricEntity : DomainBase
    {
        [PrimaryKey("id", false)]
        public string Id { get; set; } = Guid.NewGuid().ToString();

        [Column("stage_type_id")]
        public string StageTypeId { get; set; } = string.Empty;

        [Column("exam_type")]
        public string ExamType { get; set; } = string.Empty; // midterm, endterm

        [Column("total_marks")]
        public int TotalMarks { get; set; }
    }

    [Table("rubric_components")]
    public class RubricComponentEntity : DomainBase
    {
        [PrimaryKey("id", false)]
        public string Id { get; set; } = Guid.NewGuid().ToString();

        [Column("rubric_id")]
        public string RubricId { get; set; } = string.Empty;

        [Column("component_name")]
        public string ComponentName { get; set; } = string.Empty;

        [Column("max_marks")]
        public int MaxMarks { get; set; }
    }

    [Table("student_evaluations")]
    public class StudentEvaluationEntity : DomainBase
    {
        [PrimaryKey("id", false)]
        public string Id { get; set; } = Guid.NewGuid().ToString();

        [Column("student_id")]
        public string StudentId { get; set; } = string.Empty;

        [Column("rubric_component_id")]
        public string RubricComponentId { get; set; } = string.Empty;

        [Column("evaluator_id")]
        public string? EvaluatorId { get; set; }

        [Column("marks")]
        public decimal Marks { get; set; }

        [Column("remarks")]
        public string? Remarks { get; set; }
    }
}
