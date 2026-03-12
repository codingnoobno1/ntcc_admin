using Postgrest.Attributes;
using Postgrest.Models;

namespace ntcc_admin_blazor.Domain.Entities
{
    [Table("students")]
    public class StudentEntity : DomainBase
    {
        [PrimaryKey("id", false)]
        public Guid Id { get; set; }

        [Column("full_name")]
        public string FullName { get; set; } = string.Empty;

        [Column("email")]
        public string Email { get; set; } = string.Empty;

        [Column("batch_id")]
        public Guid? BatchId { get; set; }

        [Column("university_id")]
        public string? UniversityId { get; set; }

        [Column("enrollment_number")]
        public string? EnrollmentNumber { get; set; }

        [Column("section")]
        public string? Section { get; set; }

        [Column("current_semester")]
        public int CurrentSemester { get; set; } = 1;

        [Column("roll_number")]
        public string? RollNumber { get; set; }

        [Column("enrollment_year")]
        public int EnrollmentYear { get; set; }

        [Column("status")]
        public string Status { get; set; } = "active";

        [Column("password_hash")]
        public string? PasswordHash { get; set; }
    }

    [Table("student_stage_progress")]
    public class StudentStageProgress : DomainBase
    {
        [PrimaryKey("id", false)]
        public Guid Id { get; set; } = Guid.NewGuid();

        [Column("student_id")]
        public Guid StudentId { get; set; }

        [Column("stage_id")]
        public Guid StageId { get; set; }

        [Column("status")]
        public string Status { get; set; } = "enrolled";

        [Column("mentor_approved")]
        public bool MentorApproved { get; set; }

        [Column("midterm_score")]
        public decimal? MidtermScore { get; set; }

        [Column("endterm_score")]
        public decimal? EndtermScore { get; set; }

        [Column("is_completed")]
        public bool IsCompleted { get; set; }
    }
}