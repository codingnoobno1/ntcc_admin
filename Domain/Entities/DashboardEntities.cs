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
}
