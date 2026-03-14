using ntcc_admin_blazor.Domain.Entities;
using ntcc_admin_blazor.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ntcc_admin_blazor.Application.Services
{
    public class ActivityLogService : IActivityLogService
    {
        private readonly SupabaseService _supabase;

        public ActivityLogService(SupabaseService supabase)
        {
            _supabase = supabase;
        }

        public async Task LogActivityAsync(Guid studentId, string action, string metadata = "{}")
        {
            try
            {
                var log = new StudentActivityLogEntity
                {
                    StudentId = studentId.ToString(),
                    Action = action,
                    Metadata = metadata
                };
                await _supabase.Insert(log);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ACTIVITY LOG] Failed to log: {ex.Message}");
            }
        }

        public async Task<List<StudentActivityLogEntity>> GetStudentTimelineAsync(Guid studentId)
        {
            try
            {
                var logs = await _supabase.GetWhere<StudentActivityLogEntity>("student_id", studentId.ToString());
                return logs.OrderByDescending(l => l.CreatedAt).ToList();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ACTIVITY LOG] Failed to fetch timeline: {ex.Message}");
                return new List<StudentActivityLogEntity>();
            }
        }

        public async Task<List<StudentActivityLogEntity>> GetRecentGlobalActivitiesAsync(int limit = 10)
        {
            try
            {
                // Note: Postgrest doesn't have a direct global OrderBy via Get() without From(), 
                // so we fetch all and sort in memory for now, or use Supabase's direct client if available.
                // Assuming GetAll fetches all and we order it. 
                var logs = await _supabase.GetAll<StudentActivityLogEntity>();
                return logs.OrderByDescending(l => l.CreatedAt).Take(limit).ToList();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ACTIVITY LOG] Failed to fetch recent activities: {ex.Message}");
                return new List<StudentActivityLogEntity>();
            }
        }
    }
}
