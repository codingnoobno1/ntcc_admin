using ntcc_admin_blazor.Domain.Entities;
using ntcc_admin_blazor.Services;

namespace ntcc_admin_blazor.Application.Services
{
    public class EvaluationService : IEvaluationService
    {
        private readonly SupabaseService _supabase;

        public EvaluationService(SupabaseService supabase)
        {
            _supabase = supabase;
        }

        public async Task<EvaluationSchemeEntity> GetSchemeForStageAsync(long stageId)
        {
            var schemes = await _supabase.GetWhere<EvaluationSchemeEntity>("stage_id", stageId);
            return schemes.FirstOrDefault();
        }

        public async Task<List<EvaluationCategoryEntity>> GetCategoriesForSchemeAsync(long schemeId)
        {
            return await _supabase.GetWhere<EvaluationCategoryEntity>("scheme_id", schemeId);
        }

        public async Task<List<EvaluationComponentEntity>> GetComponentsForCategoryAsync(long categoryId)
        {
            return await _supabase.GetWhere<EvaluationComponentEntity>("category_id", categoryId);
        }

        public async Task<bool> SubmitScoreAsync(Guid studentId, long componentId, int marks)
        {
            var score = new EvaluationScoreEntity
            {
                StudentId = studentId,
                ComponentId = componentId,
                Marks = marks,
                IsLocked = false,
                UpdatedAt = DateTime.UtcNow
            };

            await _supabase.Upsert(score);
            return true;
        }

        public async Task<int> CalculateTotalScoreAsync(Guid studentId, long stageId)
        {
            var scheme = await GetSchemeForStageAsync(stageId);
            if (scheme == null) return 0;

            var categories = await GetCategoriesForSchemeAsync(scheme.Id);
            int total = 0;

            foreach (var cat in categories)
            {
                var components = await GetComponentsForCategoryAsync(cat.Id);
                int catScore = 0;

                foreach (var comp in components)
                {
                    var scores = await _supabase.GetWhere<EvaluationScoreEntity>("student_id", studentId);
                    var score = scores.FirstOrDefault(s => s.ComponentId == comp.Id);
                    if (score != null) catScore += score.Marks;
                }

                // Apply category weight logic if needed
                total += catScore;
            }

            return total;
        }

        public async Task<bool> LockEvaluationAsync(Guid studentId, long stageId)
        {
            // Logic to mark all scores for this student/stage as IsLocked = true
            return true;
        }
    }
}
