using Postgrest.Attributes;
using Postgrest.Models;

namespace ntcc_admin_blazor.Models
{
    // ─── Stage Engine Core Tables ────────────────────────

    [Table("ntcc_stages")]
    public class NtccStage : BaseModel
    {
        [PrimaryKey("id", false)]
        public Guid Id { get; set; } = Guid.NewGuid();

        [Column("name")]
        public string Name { get; set; } = string.Empty;

        [Column("program")]
        public string Program { get; set; } = string.Empty;

        [Column("semester")]
        public int Semester { get; set; }

        [Column("batch")]
        public int Batch { get; set; }

        [Column("stage_type")]
        public string StageType { get; set; } = string.Empty;

        [Column("credits")]
        public int Credits { get; set; }

        [Column("status")]
        public string Status { get; set; } = "active";

        [Column("created_at")]
        public DateTime CreatedAt { get; set; }
    }

    [Table("stage_deadlines")]
    public class StageDeadline : BaseModel
    {
        [PrimaryKey("id", false)]
        public Guid Id { get; set; } = Guid.NewGuid();

        [Column("stage_id")]
        public Guid StageId { get; set; }

        [Column("milestone_name")]
        public string MilestoneName { get; set; } = string.Empty;

        [Column("deadline")]
        public DateTime Deadline { get; set; }

        [Column("is_submission_required")]
        public bool IsSubmissionRequired { get; set; }

        [Column("order_index")]
        public int OrderIndex { get; set; }
    }

    [Table("stage_submission_rules")]
    public class StageSubmissionRule : BaseModel
    {
        [PrimaryKey("id", false)]
        public Guid Id { get; set; } = Guid.NewGuid();

        [Column("stage_id")]
        public Guid StageId { get; set; }

        [Column("milestone_id")]
        public Guid MilestoneId { get; set; }

        [Column("file_type")]
        public string FileType { get; set; } = "PDF";

        [Column("max_files")]
        public int MaxFiles { get; set; } = 1;

        [Column("required")]
        public bool Required { get; set; } = true;
    }

    [Table("evaluation_categories")]
    public class EvaluationCategory : BaseModel
    {
        [PrimaryKey("id", false)]
        public Guid Id { get; set; } = Guid.NewGuid();

        [Column("stage_id")]
        public Guid StageId { get; set; }

        [Column("name")]
        public string Name { get; set; } = string.Empty;

        [Column("max_marks")]
        public decimal MaxMarks { get; set; }

        [Column("evaluator_role")]
        public string EvaluatorRole { get; set; } = "evaluator";
    }

    [Table("evaluation_components")]
    public class EvaluationComponent : BaseModel
    {
        [PrimaryKey("id", false)]
        public Guid Id { get; set; } = Guid.NewGuid();

        [Column("category_id")]
        public Guid CategoryId { get; set; }

        [Column("name")]
        public string Name { get; set; } = string.Empty;

        [Column("marks")]
        public decimal Marks { get; set; }
    }

    [Table("stage_requirements")]
    public class StageRequirement : BaseModel
    {
        [PrimaryKey("id", false)]
        public Guid Id { get; set; } = Guid.NewGuid();

        [Column("stage_id")]
        public Guid StageId { get; set; }

        [Column("requirement_type")]
        public string RequirementType { get; set; } = string.Empty;

        [Column("value")]
        public int Value { get; set; }
    }

    // ─── Core Entity Tables ──────────────────────────────

    [Table("profiles")]
    public class Profile : BaseModel
    {
        [PrimaryKey("id", false)]
        public Guid Id { get; set; }

        [Column("full_name")]
        public string FullName { get; set; } = string.Empty;

        [Column("email")]
        public string Email { get; set; } = string.Empty;

        [Column("role")]
        public string Role { get; set; } = string.Empty;

        [Column("department")]
        public string Department { get; set; } = string.Empty;

        [Column("is_verified")]
        public bool IsVerified { get; set; } = false;
    }

    [Table("batches")]
    public class Batch : BaseModel
    {
        [PrimaryKey("id", false)]
        public Guid Id { get; set; } = Guid.NewGuid();

        [Column("name")]
        public string Name { get; set; } = string.Empty;

        [Column("year_start")]
        public int YearStart { get; set; }

        [Column("year_end")]
        public int YearEnd { get; set; }

        [Column("created_at")]
        public DateTime CreatedAt { get; set; }
    }

    [Table("students")]
    public class Student : BaseModel
    {
        [PrimaryKey("id", false)]
        public Guid Id { get; set; } = Guid.NewGuid();

        [Column("full_name")]
        public string FullName { get; set; } = string.Empty;

        [Column("roll_number")]
        public string RollNumber { get; set; } = string.Empty;

        [Column("email")]
        public string Email { get; set; } = string.Empty;

        [Column("batch_id")]
        public Guid BatchId { get; set; }

        [Column("program")]
        public string Program { get; set; } = "B.Tech CSE";

        [Column("current_semester")]
        public int CurrentSemester { get; set; } = 1;

        [Column("enrollment_year")]
        public int EnrollmentYear { get; set; }

        [Column("status")]
        public string Status { get; set; } = "active";

        [Column("created_at")]
        public DateTime CreatedAt { get; set; }
    }

    [Table("internships")]
    public class Internship : BaseModel
    {
        [PrimaryKey("id", false)]
        public Guid Id { get; set; } = Guid.NewGuid();

        [Column("student_id")]
        public Guid StudentId { get; set; }

        [Column("stage_id")]
        public Guid StageId { get; set; }

        [Column("company_name")]
        public string CompanyName { get; set; } = string.Empty;

        [Column("company_location")]
        public string CompanyLocation { get; set; } = string.Empty;

        [Column("duration_days")]
        public int DurationDays { get; set; } = 45;

        [Column("start_date")]
        public DateTime? StartDate { get; set; }

        [Column("end_date")]
        public DateTime? EndDate { get; set; }

        [Column("mentor_name")]
        public string MentorName { get; set; } = string.Empty;

        [Column("mentor_contact")]
        public string MentorContact { get; set; } = string.Empty;

        [Column("report_uploaded")]
        public bool ReportUploaded { get; set; } = false;

        [Column("report_url")]
        public string ReportUrl { get; set; } = string.Empty;

        [Column("faculty_mentor_id")]
        public Guid FacultyMentorId { get; set; }

        [Column("evaluation_score")]
        public decimal? EvaluationScore { get; set; }

        [Column("evaluation_remarks")]
        public string EvaluationRemarks { get; set; } = string.Empty;

        [Column("status")]
        public string Status { get; set; } = "ongoing";

        [Column("created_at")]
        public DateTime CreatedAt { get; set; }
    }

    [Table("projects")]
    public class Project : BaseModel
    {
        [PrimaryKey("id", false)]
        public Guid Id { get; set; } = Guid.NewGuid();

        [Column("title")]
        public string Title { get; set; } = string.Empty;

        [Column("description")]
        public string Description { get; set; } = string.Empty;

        [Column("project_type")]
        public string ProjectType { get; set; } = "minor";

        [Column("stage_id")]
        public Guid StageId { get; set; }

        [Column("batch_id")]
        public Guid BatchId { get; set; }

        [Column("faculty_mentor_id")]
        public Guid FacultyMentorId { get; set; }

        [Column("proposal_status")]
        public string ProposalStatus { get; set; } = "draft";

        [Column("submission_status")]
        public string SubmissionStatus { get; set; } = "pending";

        [Column("submission_url")]
        public string SubmissionUrl { get; set; } = string.Empty;

        [Column("semester")]
        public int Semester { get; set; }

        [Column("created_at")]
        public DateTime CreatedAt { get; set; }
    }

    [Table("project_members")]
    public class ProjectMember : BaseModel
    {
        [PrimaryKey("id", false)]
        public Guid Id { get; set; } = Guid.NewGuid();

        [Column("project_id")]
        public Guid ProjectId { get; set; }

        [Column("student_id")]
        public Guid StudentId { get; set; }

        [Column("role")]
        public string Role { get; set; } = "member";
    }

    [Table("evaluation_scores")]
    public class EvaluationScore : BaseModel
    {
        [PrimaryKey("id", false)]
        public Guid Id { get; set; } = Guid.NewGuid();

        [Column("student_id")]
        public Guid StudentId { get; set; }

        [Column("stage_id")]
        public Guid StageId { get; set; }

        [Column("category_id")]
        public Guid CategoryId { get; set; }

        [Column("evaluator_id")]
        public Guid EvaluatorId { get; set; }

        [Column("score")]
        public decimal Score { get; set; }

        [Column("max_score")]
        public decimal MaxScore { get; set; }

        [Column("remarks")]
        public string Remarks { get; set; } = string.Empty;

        [Column("evaluated_at")]
        public DateTime EvaluatedAt { get; set; }
    }

    [Table("stage_faculty")]
    public class StageFaculty : BaseModel
    {
        [PrimaryKey("id", false)]
        public Guid Id { get; set; } = Guid.NewGuid();

        [Column("stage_id")]
        public Guid StageId { get; set; }

        [Column("faculty_id")]
        public Guid FacultyId { get; set; }

        [Column("role")]
        public string Role { get; set; } = "evaluator";
    }
    [Table("faculty_roles")]
    public class FacultyRole : BaseModel
    {
        [PrimaryKey("id", false)]
        public Guid Id { get; set; } = Guid.NewGuid();

        [Column("faculty_id")]
        public Guid FacultyId { get; set; }

        [Column("role")]
        public string Role { get; set; } = string.Empty; // 'host', 'mentor', 'evaluator'

        [Column("created_at")]
        public DateTime CreatedAt { get; set; }
    }

    [Table("faculty_stage_assignments")]
    public class FacultyStageAssignment : BaseModel
    {
        [PrimaryKey("id", false)]
        public Guid Id { get; set; } = Guid.NewGuid();

        [Column("faculty_id")]
        public Guid FacultyId { get; set; }

        [Column("stage_id")]
        public Guid StageId { get; set; }

        [Column("role")]
        public string Role { get; set; } = string.Empty; // 'host', 'mentor', 'evaluator'
    }
}
