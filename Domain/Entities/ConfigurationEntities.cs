using Postgrest.Attributes;
using Postgrest.Models;

namespace ntcc_admin_blazor.Domain.Entities
{
    [Table("system_settings")]
    public class SystemSettingEntity : DomainBase
    {
        [PrimaryKey("key", false)]
        public string Key { get; set; } = string.Empty;

        [Column("value")]
        public string Value { get; set; } = string.Empty;

        [Column("description")]
        public string? Description { get; set; }

    }

    [Table("faculty_workload")]
    public class FacultyWorkloadEntity : DomainBase
    {
        [PrimaryKey("id", false)]
        public Guid Id { get; set; } = Guid.NewGuid();

        [Column("faculty_id")]
        public Guid FacultyId { get; set; }

        [Column("stage_id")]
        public Guid StageId { get; set; }

        [Column("students_assigned")]
        public int StudentsAssigned { get; set; } = 0;

        [Column("projects_assigned")]
        public int ProjectsAssigned { get; set; } = 0;

        [Column("evaluations_pending")]
        public int EvaluationsPending { get; set; } = 0;
    }
}
