using ntcc_admin_blazor.Domain.Entities;

namespace ntcc_admin_blazor.Application.Services
{
    public interface IEvaluationService
    {
        Task<EvaluationSchemeEntity> GetSchemeForStageAsync(long stageId);
        Task<List<EvaluationCategoryEntity>> GetCategoriesForSchemeAsync(long schemeId);
        Task<List<EvaluationComponentEntity>> GetComponentsForCategoryAsync(long categoryId);
        Task<bool> SubmitScoreAsync(Guid studentId, long componentId, int marks);
        Task<int> CalculateTotalScoreAsync(Guid studentId, long stageId);
        Task<bool> LockEvaluationAsync(Guid studentId, long stageId);
    }
}
