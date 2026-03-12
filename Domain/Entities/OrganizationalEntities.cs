using Postgrest.Attributes;
using Postgrest.Models;

namespace ntcc_admin_blazor.Domain.Entities
{
    [Table("programs")]
    public class ProgramEntity : DomainBase
    {
        [PrimaryKey("id", false)]
        public string Id { get; set; } = Guid.NewGuid().ToString();

        [Column("name")]
        public string Name { get; set; } = string.Empty;

        [Column("department_id")]
        public string? DepartmentId { get; set; }

        [Column("duration_years")]
        public int DurationYears { get; set; } = 4;
    }

    [Table("batches")]
    public class BatchEntity : DomainBase
    {
        [PrimaryKey("id", false)]
        public string Id { get; set; } = Guid.NewGuid().ToString();

        [Column("program_id")]
        public string? ProgramId { get; set; }

        [Column("name")]
        public string Name { get; set; } = string.Empty;

        [Column("start_year")]
        public int StartYear { get; set; }

        [Column("end_year")]
        public int EndYear { get; set; }
    }
    [Table("batch_semesters")]
    public class BatchSemesterEntity : DomainBase
    {
        [PrimaryKey("id", false)]
        public string Id { get; set; } = Guid.NewGuid().ToString();

        [Column("batch_id")]
        public string BatchId { get; set; } = string.Empty;

        [Column("semester_number")]
        public int SemesterNumber { get; set; }

        [Column("start_date")]
        public DateTime StartDate { get; set; }

        [Column("end_date")]
        public DateTime EndDate { get; set; }

        [Column("is_active")]
        public bool IsActive { get; set; }
    }

    [Table("academic_stage_rules")]
    public class AcademicStageRuleEntity : DomainBase
    {
        [PrimaryKey("id", false)]
        public string Id { get; set; } = Guid.NewGuid().ToString();

        [Column("program_id")]
        public string? ProgramId { get; set; }

        [Column("semester_number")]
        public int SemesterNumber { get; set; }

        [Column("stage_type_id")]
        public string StageTypeId { get; set; } = string.Empty;

        [Column("stage_type")]
        public string StageType { get; set; } = "Minor";

        [Column("stage_name")]
        public string StageName { get; set; } = "Minor Project";

        [Column("is_visible")]
        public bool IsVisible { get; set; } = true;

        [Column("status")]
        public string Status { get; set; } = "active"; // active, closed
    }
}
