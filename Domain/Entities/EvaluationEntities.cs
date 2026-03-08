using Postgrest.Attributes;
using Postgrest.Models;

namespace ntcc_admin_blazor.Domain.Entities
{
    [Table("evaluation_schemes")]
    public class EvaluationSchemeEntity : DomainBase
    {
        [PrimaryKey("id", false)]
        public long Id { get; set; }

        [Column("stage_id")]
        public long StageId { get; set; }

        [Column("name")]
        public string Name { get; set; }
    }

    [Table("evaluation_categories")]
    public class EvaluationCategoryEntity : DomainBase
    {
        [PrimaryKey("id", false)]
        public long Id { get; set; }

        [Column("scheme_id")]
        public long SchemeId { get; set; }

        [Column("category")]
        public string Category { get; set; }

        [Column("weight")]
        public int Weight { get; set; }
    }

    [Table("evaluation_components")]
    public class EvaluationComponentEntity : DomainBase
    {
        [PrimaryKey("id", false)]
        public long Id { get; set; }

        [Column("category_id")]
        public long CategoryId { get; set; }

        [Column("component")]
        public string Component { get; set; }

        [Column("marks")]
        public int Marks { get; set; }
    }

    [Table("evaluation_scores")]
    public class EvaluationScoreEntity : DomainBase
    {
        [PrimaryKey("id", false)]
        public Guid Id { get; set; }

        [Column("student_id")]
        public Guid StudentId { get; set; }

        [Column("component_id")]
        public long ComponentId { get; set; }

        [Column("marks")]
        public int Marks { get; set; }

        [Column("faculty_id")]
        public Guid FacultyId { get; set; }

        [Column("is_locked")]
        public bool IsLocked { get; set; }
    }
}
