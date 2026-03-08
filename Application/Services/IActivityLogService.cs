using ntcc_admin_blazor.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ntcc_admin_blazor.Application.Services
{
    public interface IActivityLogService
    {
        Task LogActivityAsync(string studentId, string action, string metadata = "{}");
        Task<List<StudentActivityLogEntity>> GetStudentTimelineAsync(string studentId);
        Task<List<StudentActivityLogEntity>> GetRecentGlobalActivitiesAsync(int limit = 10);
    }
}
