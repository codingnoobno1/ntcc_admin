using ntcc_admin_blazor.Domain.Entities;

namespace ntcc_admin_blazor.Application.Services
{
    public interface IEvaluationService
    {
        Task<EvaluationSchemeEntity?> GetSchemeForStageAsync(long stageId);
        Task<List<EvaluationCategoryEntity>> GetCategoriesForSchemeAsync(long schemeId);
        Task<List<EvaluationComponentEntity>> GetComponentsForCategoryAsync(long categoryId);
        Task<bool> SubmitScoreAsync(string studentId, long componentId, int marks);
        Task<int> CalculateTotalScoreAsync(string studentId, long stageId);
        Task<bool> LockEvaluationAsync(string studentId, long stageId);
    }
}
