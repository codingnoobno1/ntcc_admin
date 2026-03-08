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
                var registryEntry = await _supabase.GetById<ComponentRegistryEntity>(sc.ComponentId);
                if (registryEntry == null) continue;

                // 3. (Optional) Check role-specific permissions for this component
                // For now, we'll assume all linked components are allowed if they reach this step
                
                widgets.Add(new WidgetDto
                {
                    ComponentKey = registryEntry.ComponentKey,
                    Type = Enum.Parse<ComponentType>(registryEntry.Type, true),
                    ConfigJson = "{}", // To be loaded from component_config table in later phases
                    Order = 0 // Can be extended with order_index in step_components
                });
            }

            return widgets;
        }

        public async Task<List<WidgetDto>> GetGlobalWidgetsAsync(string role)
        {
            // Implementation for role-specific global widgets (like Overview stats)
            return new List<WidgetDto>();
        }
    }
}
