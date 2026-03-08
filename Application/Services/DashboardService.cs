using ntcc_admin_blazor.Application.DTOs;
using ntcc_admin_blazor.Domain.Entities;
using ntcc_admin_blazor.Domain.Enums;
using ntcc_admin_blazor.Services;

namespace ntcc_admin_blazor.Application.Services
{
    public class DashboardService : IDashboardService
    {
        private readonly SupabaseService _supabase;

        public DashboardService(SupabaseService supabase)
        {
            _supabase = supabase;
        }

        public async Task<List<WidgetDto>> GetWidgetsForStepAsync(long stepId, string role)
        {
            var widgets = new List<WidgetDto>();

            // 1. Get components linked to this step
            var stepComponents = await _supabase.GetWhere<StepComponentEntity>("step_id", stepId);
            
            foreach (var sc in stepComponents)
            {
                // 2. Get component details from registry
                var registryEntries = await _supabase.GetWhere<ComponentRegistryEntity>("id", sc.ComponentId);
                var registryEntry = registryEntries.FirstOrDefault();
                if (registryEntry == null) continue;

                // 3. (Optional) Check role-specific permissions for this component
                
                widgets.Add(new WidgetDto
                {
                    ComponentKey = registryEntry.ComponentKey,
                    Type = Enum.TryParse<ComponentType>(registryEntry.Type, true, out var ct) ? ct : ComponentType.Metric,
                    ConfigJson = "{}", // To be loaded from component_config table in later phases
                    Order = 0 // Can be extended with order_index in step_components
                });
            }

            return widgets;
        }

        public async Task<List<WidgetDto>> GetGlobalWidgetsAsync(string role)
        {
            var widgets = new List<WidgetDto>();

            // Fetch Dashboard config matching the user role exactly
            var dashboards = await _supabase.GetWhere<DashboardEntity>("role", role);
            var dashboard = dashboards.FirstOrDefault();

            // Fallback default Admin layout if database is unseeded
            if (dashboard == null && (role.ToLower() == "admin" || role.ToLower() == "master"))
            {
                return new List<WidgetDto>
                {
                    new WidgetDto { ComponentKey = "faculty_accounts", Type = ComponentType.Admin, ConfigJson = "{}", Order = 1, Width = 12, RowGroup = "operations" },
                    new WidgetDto { ComponentKey = "student_metrics", Type = ComponentType.Metric, ConfigJson = "{}", Order = 2, Width = 4, RowGroup = "insights" },
                    new WidgetDto { ComponentKey = "department_metrics", Type = ComponentType.Metric, ConfigJson = "{}", Order = 3, Width = 4, RowGroup = "insights" },
                    new WidgetDto { ComponentKey = "deadline_timer", Type = ComponentType.Metric, ConfigJson = "{}", Order = 4, Width = 4, RowGroup = "insights" },
                    new WidgetDto { ComponentKey = "project_supervision", Type = ComponentType.Academic, ConfigJson = "{}", Order = 5, Width = 6, RowGroup = "pipeline" },
                    new WidgetDto { ComponentKey = "internship_pipeline", Type = ComponentType.Academic, ConfigJson = "{}", Order = 6, Width = 6, RowGroup = "pipeline" },
                    new WidgetDto { ComponentKey = "evaluation_queue", Type = ComponentType.Evaluation, ConfigJson = "{}", Order = 7, Width = 6, RowGroup = "admin" },
                    new WidgetDto { ComponentKey = "stage_engine", Type = ComponentType.Academic, ConfigJson = "{}", Order = 8, Width = 6, RowGroup = "admin" }
                };
            }

            // Custom roles or seeded Admin boards require DB configuration
            if (dashboard != null)
            {
                var dbWidgets = await _supabase.GetWhere<DashboardWidgetEntity>("dashboard_id", dashboard.Id);
                
                foreach (var w in dbWidgets.OrderBy(x => x.DisplayOrder))
                {
                    widgets.Add(new WidgetDto
                    {
                        ComponentKey = w.ComponentKey,
                        Type = ComponentType.Metric, // Can fetch from generic registry later
                        ConfigJson = string.IsNullOrWhiteSpace(w.ConfigJson) ? "{}" : w.ConfigJson,
                        Order = w.DisplayOrder,
                        Width = w.Width,
                        RowGroup = string.IsNullOrWhiteSpace(w.RowGroup) ? "operations" : w.RowGroup
                    });
                }
            }
            
            return widgets;
        }
    }
}
