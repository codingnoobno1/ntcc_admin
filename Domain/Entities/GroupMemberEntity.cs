using Postgrest.Attributes;
using Postgrest.Models;
using System;

namespace ntcc_admin_blazor.Domain.Entities
{
    [Table("group_members")]
    public class GroupMemberEntity : BaseModel
    {
        [PrimaryKey("id", false)]
        public string Id { get; set; } = Guid.NewGuid().ToString();

        [Column("group_id")]
        public string GroupId { get; set; } = string.Empty;

        [Column("student_id")]
        public string StudentId { get; set; } = string.Empty;

        [Column("project_id")]
        public string ProjectId { get; set; } = string.Empty;

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
