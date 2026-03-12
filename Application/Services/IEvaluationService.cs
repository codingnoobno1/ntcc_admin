using ntcc_admin_blazor.Domain.Entities;

namespace ntcc_admin_blazor.Application.Services
{
    public interface IEvaluationService
    {
        Task<EvaluationRubricEntity?> GetRubricForStageTypeAsync(string stageTypeId, string examType);
        Task<List<RubricComponentEntity>> GetComponentsForRubricAsync(string rubricId);
        Task<bool> SubmitStudentMarksAsync(string studentId, string componentId, decimal marks, string remarks);
        Task<decimal> CalculateTotalScoreAsync(string studentId, string rubricId);
        Task<bool> IsEvaluationCompleteAsync(string studentId, string rubricId);
        
    }
}
