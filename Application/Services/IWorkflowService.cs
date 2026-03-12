using ntcc_admin_blazor.Application.DTOs;
using ntcc_admin_blazor.Domain.Enums;

namespace ntcc_admin_blazor.Application.Services
{
    public interface IWorkflowService
    {
        Task<StageDashboardDto?> GetStageDashboardAsync(string studentId, string programId, int semesterNumber);
        
        // Meetings
        Task<List<SupervisorMeetingDto>> GetStudentMeetingsAsync(string studentId);
        Task<bool> LogMeetingAsync(SupervisorMeetingDto meeting);
        
        // Evaluations
        Task<EvaluationRubricDto?> GetEvaluationRubricAsync(string stageTypeId, string examType);
        Task<bool> SubmitEvaluationAsync(string studentId, string rubricId, List<RubricComponentDto> components, string evaluatorId);
        
        // Verification & Submissions
        Task<bool> SubmitStepArtefactAsync(string studentId, string stepId, string fileUrl, string gdriveLink);
        Task<bool> UpdateStepStatusAsync(string studentId, string stepId, StepStatus status);
    }
}
