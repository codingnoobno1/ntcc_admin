using Postgrest.Attributes;
using Postgrest.Models;
using System.Text.Json;

namespace ntcc_admin_blazor.Domain.Entities
{
    [Table("dashboards")]
    public class DashboardEntity : DomainBase
    {
        [PrimaryKey("id", false)]
        public long Id { get; set; }

        [Column("role")]
        public string Role { get; set; } = string.Empty;

        [Column("name")]
        public string Name { get; set; } = string.Empty;
    }

    [Table("dashboard_widgets")]
    public class DashboardWidgetEntity : DomainBase
    {
        [PrimaryKey("id", false)]
        public long Id { get; set; }

        [Column("dashboard_id")]
        public long DashboardId { get; set; }

        [Column("component_key")]
        public string ComponentKey { get; set; } = string.Empty;

        [Column("display_order")]
        public int DisplayOrder { get; set; }

        [Column("width")]
        public int Width { get; set; } = 12;

        [Column("row_group")]
        public string RowGroup { get; set; } = string.Empty;

        [Column("config_json")]
        public string ConfigJson { get; set; } = "{}";
    }

    [Table("step_components")]
    public class StepComponentEntity : DomainBase
    {
        [PrimaryKey("id", false)]
        public string Id { get; set; } = Guid.NewGuid().ToString();

        [Column("step_id")]
        public string StepId { get; set; } = string.Empty;

        [Column("component_id")]
        public string ComponentId { get; set; } = string.Empty;
    }

    [Table("component_registry")]
    public class ComponentRegistryEntity : DomainBase
    {
        [PrimaryKey("id", false)]
        public string Id { get; set; } = Guid.NewGuid().ToString();

        [Column("component_key")]
        public string ComponentKey { get; set; } = string.Empty;

        [Column("component_path")]
        public string ComponentPath { get; set; } = string.Empty;

        [Column("type")]
        public string Type { get; set; } = "Metric";
    }
}
