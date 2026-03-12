using ntcc_admin_blazor.Domain.Entities;
using ntcc_admin_blazor.Services;
using ntcc_admin_blazor.Models;

namespace ntcc_admin_blazor.Application.Services
{
    public interface IStudentAppService
    {
        Task<List<StudentEntity>> GetStudentsByBatch(Guid batchId);
        Task<StudentEntity?> GetStudentById(string id);
        Task<bool> EnrollStudentInStage(string studentId, string stageId);
        Task<bool> UpdateProgressStatus(string progressId, string status);
        Task<List<StudentStageProgress>> GetStudentProgress(string studentId);
    }

    public class StudentAppService : IStudentAppService
    {
        private readonly SupabaseService _supabase;
        private readonly ILogger<StudentAppService> _logger;

        public StudentAppService(SupabaseService supabase, ILogger<StudentAppService> logger)
        {
            _supabase = supabase;
            _logger = logger;
        }

        public async Task<List<StudentEntity>> GetStudentsByBatch(Guid batchId)
        {
            await _supabase.InitializeAsync();
            var result = await _supabase.Client.From<StudentEntity>()
                .Where(x => x.BatchId == batchId)
                .Get();
            return result.Models;
        }

        public async Task<StudentEntity?> GetStudentById(string id)
        {
            return await _supabase.GetById<StudentEntity>(id);
        }

        public async Task<bool> EnrollStudentInStage(string studentId, string stageId)
        {
            try
            {
                var progress = new StudentStageProgress
                {
                    StudentId = studentId,
                    StageId = stageId,
                    Status = "enrolled"
                };
                await _supabase.Insert(progress);
                
                // Track activity
                await _supabase.Insert(new ActivityLogEntity 
                { 
                    Action = "student_enrolled_in_stage",
                    Entity = "StudentStageProgress",
                    EntityId = progress.Id,
                    UserId = studentId // or current admin ID if available
                });

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to enroll student in stage");
                return false;
            }
        }

        public async Task<bool> UpdateProgressStatus(string progressId, string status)
        {
            try
            {
                var progress = await _supabase.GetById<StudentStageProgress>(progressId);
                if (progress == null) return false;

                progress.Status = status;
                progress.UpdatedAt = DateTime.UtcNow;
                await _supabase.Update(progress);

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to update progress status");
                return false;
            }
        }

        public async Task<List<StudentStageProgress>> GetStudentProgress(string studentId)
        {
            await _supabase.InitializeAsync();
            var result = await _supabase.Client.From<StudentStageProgress>()
                .Where(x => x.StudentId == studentId)
                .Get();
            return result.Models;
        }
    }
}
