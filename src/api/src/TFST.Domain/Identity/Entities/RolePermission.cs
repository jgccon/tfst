using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace TFST.Domain.Identity.Entities;

public class RolePermission
{
    [ForeignKey("Role")]
    public Guid RoleId { get; set; }
    public Role Role { get; set; } = null!;

    [ForeignKey("Permission")]
    public Guid PermissionId { get; set; }
    public Permission Permission { get; set; } = null!;
}
