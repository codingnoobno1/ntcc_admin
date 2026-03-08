using ntcc_admin_blazor.Application.DTOs;
using ntcc_admin_blazor.Domain.Enums;

namespace ntcc_admin_blazor.Application.Services
{
    public interface IWorkflowService
    {
        Task<StageDashboardDto> GetStageDashboardAsync(Guid studentId, long stageId);
        Task<List<WorkflowStepDto>> GetStepsForStageAsync(long stageId);
        Task<bool> SubmitStepAsync(Guid studentId, long stepId, string payloadJson);
        Task<bool> ApproveStepAsync(Guid studentId, long stepId, string feedback);
        Task<bool> RejectStepAsync(Guid studentId, long stepId, string feedback);
    }
}
