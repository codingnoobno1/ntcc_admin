using ntcc_admin_blazor.Domain.Enums;

namespace ntcc_admin_blazor.Application.DTOs
{
    public class WorkflowStepDto
    {
        public long Id { get; set; }
        public string StepKey { get; set; }
        public string Name { get; set; }
        public int OrderIndex { get; set; }
        public StepStatus Status { get; set; }
        public DateTime? SubmittedAt { get; set; }
    }

    public class StageDashboardDto
    {
        public long StageId { get; set; }
        public string StageName { get; set; }
        public List<WorkflowStepDto> Steps { get; set; } = new();
        public List<WidgetDto> Widgets { get; set; } = new();
    }

    public class WidgetDto
    {
        public string ComponentKey { get; set; }
        public ComponentType Type { get; set; }
        public string ConfigJson { get; set; }
        public int Order { get; set; }
    }
}
