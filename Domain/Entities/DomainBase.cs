using Postgrest.Models;
using Postgrest.Attributes;

namespace ntcc_admin_blazor.Domain.Entities
{
    public abstract class DomainBase : Postgrest.Models.BaseModel
    {
        [Column("created_at")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [Column("updated_at")]
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    }
}
