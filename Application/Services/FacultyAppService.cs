using ntcc_admin_blazor.Domain.Entities;
using ntcc_admin_blazor.Services;
using ntcc_admin_blazor.Models;

namespace ntcc_admin_blazor.Application.Services
{
    public interface IFacultyAppService
    {
        Task<List<Profile>> GetAllFaculty();
        Task<bool> CreateFaculty(string email, string fullName, string department, List<string> roles);
        Task<List<FacultyRole>> GetFacultyRoles(string facultyId);
        Task<FacultyWorkloadEntity?> GetWorkload(string facultyId, string stageId);
    }

    public class FacultyAppService : IFacultyAppService
    {
        private readonly SupabaseService _supabase;
        private readonly ILogger<FacultyAppService> _logger;

        public FacultyAppService(SupabaseService supabase, ILogger<FacultyAppService> logger)
        {
            _supabase = supabase;
            _logger = logger;
        }

        public async Task<List<Profile>> GetAllFaculty()
        {
            await _supabase.InitializeAsync();
            var result = await _supabase.Client.From<Profile>()
                .Where(x => x.Role == "faculty")
                .Get();
            return result.Models;
        }

        public async Task<bool> CreateFaculty(string email, string fullName, string department, List<string> roles)
        {
            try
            {
                var password = SupabaseService.GenerateStrongPassword();
                var userId = await _supabase.CreateFacultyAccount(email, password, fullName, department, roles);
                
                if (string.IsNullOrEmpty(userId)) return false;

                // Log activity
                await _supabase.Insert(new ActivityLogEntity
                {
                    Action = "faculty_invited",
                    Entity = "Profile",
                    EntityId = userId,
                    Metadata = $"{{\"email\":\"{email}\", \"roles\":\"{string.Join(",", roles)}\"}}"
                });

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to create faculty account");
                return false;
            }
        }

        public async Task<List<FacultyRole>> GetFacultyRoles(string facultyId)
        {
            await _supabase.InitializeAsync();
            var result = await _supabase.Client.From<FacultyRole>()
                .Where(x => x.FacultyId == facultyId)
                .Get();
            return result.Models;
        }

        public async Task<FacultyWorkloadEntity?> GetWorkload(string facultyId, string stageId)
        {
            await _supabase.InitializeAsync();
            var result = await _supabase.Client.From<FacultyWorkloadEntity>()
                .Where(x => x.FacultyId == facultyId)
                .Where(x => x.StageId == stageId)
                .Get();
            return result.Models.FirstOrDefault();
        }
    }
}
