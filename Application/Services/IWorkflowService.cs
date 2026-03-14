using ntcc_admin_blazor.Application.DTOs;
using ntcc_admin_blazor.Domain.Enums;

namespace ntcc_admin_blazor.Application.Services
{
    public interface IWorkflowService
    {
        Task<StageDashboardDto?> GetStageDashboardAsync(Guid studentId, Guid programId, int semesterNumber);
        
        // Meetings
        Task<List<SupervisorMeetingDto>> GetStudentMeetingsAsync(Guid studentId);
        Task<bool> LogMeetingAsync(SupervisorMeetingDto meeting);
        
        // Evaluations
        Task<EvaluationRubricDto?> GetEvaluationRubricAsync(Guid stageTypeId, string examType);
        Task<bool> SubmitEvaluationAsync(Guid studentId, Guid rubricId, List<RubricComponentDto> components, Guid evaluatorId);
        
        // Verification & Submissions
        Task<bool> SubmitStepArtefactAsync(Guid studentId, Guid stepId, string fileUrl, string gdriveLink);
        Task<bool> UpdateStepStatusAsync(Guid studentId, Guid stepId, StepStatus status);
    }
}
