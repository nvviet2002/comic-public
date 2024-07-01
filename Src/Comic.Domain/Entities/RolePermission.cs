using Comic.Domain.Common;
using System.ComponentModel.DataAnnotations.Schema;
namespace Comic.Domain.Entities
{
    [Table("RolePermissions")]
    public class RolePermission:BaseEntity
    {
        public string RoleId { get; set; } = string.Empty;
        public string PermissionId { get; set; } = string.Empty;
        [ForeignKey("RoleId")]
        public virtual Role? Role { get; set; }
        [ForeignKey("PermissionId")]
        public virtual Permission? Permission { get; set; }
    }
}
