using Postgrest.Attributes;
using Postgrest.Models;
using System;

namespace ntcc_admin_blazor.Domain.Entities
{
    [Table("group_members")]
    public class GroupMemberEntity : BaseModel
    {
        [PrimaryKey("id", false)]
        public Guid Id { get; set; } = Guid.NewGuid();

        [Column("group_id")]
        public Guid GroupId { get; set; }

        [Column("student_id")]
        public Guid StudentId { get; set; }

        [Column("project_id")]
        public Guid ProjectId { get; set; }

        // leader, member
        [Column("role")]
        public string Role { get; set; } = "member";

        // invited, accepted, rejected, removed
        [Column("status")]
        public string Status { get; set; } = "invited";

        [Column("joined_at")]
        public DateTime? JoinedAt { get; set; }
    }
}
