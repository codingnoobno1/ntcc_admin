using ntcc_admin_blazor.Application.DTOs;
using ntcc_admin_blazor.Domain.Enums;

namespace ntcc_admin_blazor.Application.Services
{
    public interface IWorkflowService
    {
        Task<StageDashboardDto> GetStageDashboardAsync(string studentId, long stageId);
        Task<List<WorkflowStepDto>> GetStepsForStageAsync(long stageId);
        Task<bool> SubmitStepAsync(string studentId, long stepId, string payloadJson);
        Task<bool> ApproveStepAsync(string studentId, long stepId, string feedback);
        Task<bool> RejectStepAsync(string studentId, long stepId, string feedback);
    }
}
