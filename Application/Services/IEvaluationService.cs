using ntcc_admin_blazor.Domain.Entities;

namespace ntcc_admin_blazor.Application.Services
{
    public interface IEvaluationService
    {
        Task<EvaluationRubricEntity?> GetRubricForStageTypeAsync(Guid stageTypeId, string examType);
        Task<List<RubricComponentEntity>> GetComponentsForRubricAsync(Guid rubricId);
        Task<bool> SubmitStudentMarksAsync(Guid studentId, Guid componentId, decimal marks, string remarks);
        Task<decimal> CalculateTotalScoreAsync(Guid studentId, Guid rubricId);
        Task<bool> IsEvaluationCompleteAsync(Guid studentId, Guid rubricId);
        
    }
}
