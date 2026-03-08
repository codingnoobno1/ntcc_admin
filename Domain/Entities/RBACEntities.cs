using Postgrest.Attributes;
using Postgrest.Models;

namespace ntcc_admin_blazor.Domain.Entities
{
    [Table("roles")]
    public class RoleEntity : BaseModel
    {
        [PrimaryKey("id", false)]
        public long Id { get; set; }

        [Column("role_name")]
        public string RoleName { get; set; }
    }

    [Table("permissions")]
    public class PermissionEntity : BaseModel
    {
        [PrimaryKey("id", false)]
        public long Id { get; set; }

        [Column("permission_key")]
        public string PermissionKey { get; set; }
    }

    [Table("role_permissions")]
    public class RolePermissionEntity : BaseModel
    {
        [Column("role_id")]
        public long RoleId { get; set; }

        [Column("permission_id")]
        public long PermissionId { get; set; }

        [Column("enabled")]
        public bool Enabled { get; set; }
    }

    [Table("user_roles")]
    public class UserRoleEntity : BaseModel
    {
        [Column("user_id")]
        public Guid UserId { get; set; }

        [Column("role_id")]
        public long RoleId { get; set; }
    }

    [Table("user_permission_override")]
    public class UserPermissionOverrideEntity : BaseModel
    {
        [Column("user_id")]
        public Guid UserId { get; set; }

        [Column("permission_id")]
        public long PermissionId { get; set; }

        [Column("enabled")]
        public bool Enabled { get; set; }
    }

    [Table("workflow_permissions")]
    public class WorkflowPermissionEntity : BaseModel
    {
        [Column("role_id")]
        public long RoleId { get; set; }

        [Column("stage_id")]
        public long StageId { get; set; }

        [Column("action")]
        public string Action { get; set; }

        [Column("allowed")]
        public bool Allowed { get; set; }
    }
}
