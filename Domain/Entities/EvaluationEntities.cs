using Postgrest.Attributes;
using Postgrest.Models;

namespace ntcc_admin_blazor.Domain.Entities
{
    [Table("evaluation_rubrics")]
    public class EvaluationRubricEntity : DomainBase
    {
        [PrimaryKey("id", false)]
        public Guid Id { get; set; } = Guid.NewGuid();

        [Column("stage_type_id")]
        public Guid StageTypeId { get; set; }

        [Column("exam_type")]
        public string ExamType { get; set; } = string.Empty; // midterm, endterm

        [Column("total_marks")]
        public int TotalMarks { get; set; }
    }

    [Table("rubric_components")]
    public class RubricComponentEntity : DomainBase
    {
        [PrimaryKey("id", false)]
        public Guid Id { get; set; } = Guid.NewGuid();

        [Column("rubric_id")]
        public Guid RubricId { get; set; }

        [Column("component_name")]
        public string ComponentName { get; set; } = string.Empty;

        [Column("max_marks")]
        public int MaxMarks { get; set; }
    }

    [Table("student_evaluations")]
    public class StudentEvaluationEntity : DomainBase
    {
        [PrimaryKey("id", false)]
        public Guid Id { get; set; } = Guid.NewGuid();

        [Column("student_id")]
        public Guid StudentId { get; set; }

        [Column("rubric_component_id")]
        public Guid RubricComponentId { get; set; }

        [Column("evaluator_id")]
        public Guid? EvaluatorId { get; set; }

        [Column("marks")]
        public decimal Marks { get; set; }

        [Column("remarks")]
        public string? Remarks { get; set; }
    }
}
