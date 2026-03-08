using Postgrest.Attributes;
using Postgrest.Models;

namespace ntcc_admin_blazor.Domain.Entities
{
    [Table("students")]
    public class StudentEntity : DomainBase
    {
        [PrimaryKey("id", false)]
        public string Id { get; set; } = string.Empty; // Maps to Profile ID / Auth ID

        [Column("full_name")]
        public string FullName { get; set; } = string.Empty;

        [Column("email")]
        public string Email { get; set; } = string.Empty;

        [Column("batch_id")]
        public string BatchId { get; set; } = string.Empty;

        [Column("current_semester")]
        public int CurrentSemester { get; set; } = 1;

        [Column("roll_number")]
        public string? RollNumber { get; set; }

        [Column("status")]
        public string Status { get; set; } = "active";
    }

    [Table("student_stage_progress")]
    public class StudentStageProgress : DomainBase
    {
        [PrimaryKey("id", false)]
        public string Id { get; set; } = Guid.NewGuid().ToString();

        [Column("student_id")]
        public string StudentId { get; set; } = string.Empty;

        [Column("stage_id")]
        public string StageId { get; set; } = string.Empty;

        [Column("status")]
        public string Status { get; set; } = "enrolled";

        [Column("mentor_approved")]
        public bool MentorApproved { get; set; } = false;

        [Column("midterm_score")]
        public decimal? MidtermScore { get; set; }

        [Column("endterm_score")]
        public decimal? EndtermScore { get; set; }

        [Column("is_completed")]
        public bool IsCompleted { get; set; } = false;
    }
}
