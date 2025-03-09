using System.ComponentModel.DataAnnotations;
using TFST.Domain.Base;

namespace TFST.Domain.Identity.Entities;

public class Role : BaseEntity
{
    [Required]
    public string Name { get; set; } = string.Empty;

    public ICollection<UserRole> UserRoles { get; set; } = new List<UserRole>();
    public ICollection<RolePermission> RolePermissions { get; set; } = new List<RolePermission>();
}
