using Postgrest.Attributes;
using Postgrest.Models;

namespace ntcc_admin_blazor.Domain.Entities
{
    [Table("custom_nav_items")]
    public class CustomNavItemEntity : DomainBase
    {
        [PrimaryKey("id", false)]
        public long Id { get; set; }

        [Column("role")]
        public string Role { get; set; } = string.Empty;

        [Column("label")]
        public string Label { get; set; } = string.Empty;

        [Column("route")]
        public string Route { get; set; } = string.Empty;

        [Column("icon")]
        public string Icon { get; set; } = string.Empty;

        [Column("order_index")]
        public int OrderIndex { get; set; }
    }
}
