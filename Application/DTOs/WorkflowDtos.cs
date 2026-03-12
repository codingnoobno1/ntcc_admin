using ntcc_admin_blazor.Domain.Enums;

namespace ntcc_admin_blazor.Application.DTOs
{
    public class WorkflowStepDto
    {
        public string Id { get; set; }
        public string StepKey { get; set; }
        public string Name { get; set; }
        public int OrderIndex { get; set; }
        public StepStatus Status { get; set; }
        public DateTime? SubmittedAt { get; set; }
        public bool RequiresSubmission { get; set; }
        public bool RequiresMeeting { get; set; }
    }

    public class StageDashboardDto
    {
        public string StageId { get; set; }
        public string StageName { get; set; }
        public int RequiredMeetings { get; set; }
        public int LoggedMeetings { get; set; }
        public int RequiredReports { get; set; }
        public int SubmittedReports { get; set; }
        public List<WorkflowStepDto> Steps { get; set; } = new();
        public List<WidgetDto> Widgets { get; set; } = new();
    }

    public class SupervisorMeetingDto
    {
        public string Id { get; set; }
        public string StudentId { get; set; }
        public string FacultyId { get; set; }
        public DateTime? MeetingDate { get; set; }
        public string Summary { get; set; }
        public int ProgressScore { get; set; }
        public string ReportUrl { get; set; }
    }

    public class EvaluationRubricDto
    {
        public string Id { get; set; }
        public string ExamType { get; set; }
        public int TotalMarks { get; set; }
        public List<RubricComponentDto> Components { get; set; } = new();
    }

    public class RubricComponentDto
    {
        public string Id { get; set; }
        public string ComponentName { get; set; }
        public int MaxMarks { get; set; }
        public decimal MarksObtained { get; set; }
        public string Remarks { get; set; }
    }

    public class WidgetDto
    {
        public string ComponentKey { get; set; }
        public ComponentType Type { get; set; }
        public string ConfigJson { get; set; }
        public int Order { get; set; }
        public int Width { get; set; } = 6;
        public string RowGroup { get; set; } = "operations";
    }
}
