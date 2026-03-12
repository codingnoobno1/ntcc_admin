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

        public async Task<EvaluationRubricEntity?> GetRubricForStageTypeAsync(Guid stageTypeId, string examType)
        {
            var rubrics = await _supabase.GetWhere<EvaluationRubricEntity>("stage_type_id", stageTypeId);
            return rubrics.FirstOrDefault(r => r.ExamType == examType);
        }

        public async Task<List<RubricComponentEntity>> GetComponentsForRubricAsync(Guid rubricId)
        {
            return await _supabase.GetWhere<RubricComponentEntity>("rubric_id", rubricId);
        }

        public async Task<bool> SubmitStudentMarksAsync(Guid studentId, Guid componentId, decimal marks, string remarks)
        {
            var evaluation = new StudentEvaluationEntity
            {
                StudentId = studentId,
                RubricComponentId = componentId,
                Marks = marks,
                Remarks = remarks,
                EvaluatorId = Guid.Empty // Mocked
            };

            await _supabase.Upsert(evaluation);
            return true;
        }

        public async Task<decimal> CalculateTotalScoreAsync(Guid studentId, Guid rubricId)
        {
            var components = await GetComponentsForRubricAsync(rubricId);
            var evaluations = await _supabase.GetWhere<StudentEvaluationEntity>("student_id", studentId);
            
            decimal total = 0;
            foreach (var comp in components)
            {
                var ev = evaluations.FirstOrDefault(e => e.RubricComponentId == comp.Id);
                if (ev != null) total += ev.Marks;
            }

            return total;
        }

        public async Task<bool> IsEvaluationCompleteAsync(Guid studentId, Guid rubricId)
        {
            var components = await GetComponentsForRubricAsync(rubricId);
            var evaluations = await _supabase.GetWhere<StudentEvaluationEntity>("student_id", studentId);
            
            return components.All(c => evaluations.Any(e => e.RubricComponentId == c.Id));
        }

    }
}
