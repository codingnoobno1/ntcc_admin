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
        public string ProgramId { get; set; } = string.Empty;

        [Column("name")]
        public string Name { get; set; } = string.Empty;

        [Column("start_year")]
        public int StartYear { get; set; }

        [Column("end_year")]
        public int EndYear { get; set; }
    }
}
