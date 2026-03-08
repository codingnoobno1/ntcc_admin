using ntcc_admin_blazor.Application.DTOs;

namespace ntcc_admin_blazor.Application.Services
{
    public interface IDashboardService
    {
        Task<List<WidgetDto>> GetWidgetsForStepAsync(long stepId, string role);
        Task<List<WidgetDto>> GetGlobalWidgetsAsync(string role);
    }
}
