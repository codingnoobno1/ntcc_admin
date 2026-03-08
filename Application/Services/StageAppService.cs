using ntcc_admin_blazor.Domain.Entities;
using ntcc_admin_blazor.Services;
using ntcc_admin_blazor.Models;

namespace ntcc_admin_blazor.Application.Services
{
    public interface IStageAppService
    {
        Task<List<NtccStage>> GetAllStages();
        Task<NtccStage?> GetStageById(string id);
        Task<bool> CreateStage(NtccStage stage);
        Task<bool> UpdateStageSettings(string stageId, Dictionary<string, string> settings);
        Task<List<StageDeadline>> GetDeadlines(string stageId);
        Task<List<EvaluationCategory>> GetEvaluationRubric(string stageId);
    }

    public class StageAppService : IStageAppService
    {
        private readonly SupabaseService _supabase;
        private readonly ILogger<StageAppService> _logger;

        public StageAppService(SupabaseService supabase, ILogger<StageAppService> logger)
        {
            _supabase = supabase;
            _logger = logger;
        }

        public async Task<List<NtccStage>> GetAllStages()
        {
            return await _supabase.GetAll<NtccStage>();
        }

        public async Task<NtccStage?> GetStageById(string id)
        {
            return await _supabase.GetById<NtccStage>(id);
        }

        public async Task<bool> CreateStage(NtccStage stage)
        {
            try
            {
                await _supabase.Insert(stage);
                
                await _supabase.Insert(new ActivityLogEntity
                {
                    Action = "stage_created",
                    Entity = "NtccStage",
                    EntityId = stage.Id,
                    Metadata = $"{{\"name\":\"{stage.Name}\"}}"
                });

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to create stage");
                return false;
            }
        }

        public async Task<bool> UpdateStageSettings(string stageId, Dictionary<string, string> settings)
        {
            try
            {
                foreach (var setting in settings)
                {
                    var entity = new SystemSettingEntity
                    {
                        Key = $"stage_{stageId}_{setting.Key}",
                        Value = setting.Value,
                        UpdatedAt = DateTime.UtcNow
                    };
                    await _supabase.Update(entity); // Assuming Upsert exists or Update handles it
                }
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to update stage settings");
                return false;
            }
        }

        public async Task<List<StageDeadline>> GetDeadlines(string stageId)
        {
            await _supabase.InitializeAsync();
            var result = await _supabase.Client.From<StageDeadline>()
                .Where(x => x.StageId == stageId)
                .Get();
            return result.Models;
        }

        public async Task<List<EvaluationCategory>> GetEvaluationRubric(string stageId)
        {
            await _supabase.InitializeAsync();
            var result = await _supabase.Client.From<EvaluationCategory>()
                .Where(x => x.StageId == stageId)
                .Get();
            return result.Models;
        }
    }
}
