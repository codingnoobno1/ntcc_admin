using Postgrest.Attributes;
using Postgrest.Models;
using System;

namespace ntcc_admin_blazor.Domain.Entities
{
    [Table("companies")]
    public class CompanyEntity : BaseModel
    {
        [PrimaryKey("id", false)]
        public string Id { get; set; } = Guid.NewGuid().ToString();

        [Column("name")]
        public string Name { get; set; } = string.Empty;

        [Column("industry")]
        public string Industry { get; set; } = string.Empty;

        [Column("location")]
        public string Location { get; set; } = string.Empty;

        // E.g., 1 to 5 stars based on past student feedback
        [Column("rating")]
        public float Rating { get; set; } = 0f;

        [Column("created_at")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
