using System.ComponentModel.DataAnnotations;
using TFST.SharedKernel.Domain.Entities;

namespace TFST.Modules.Users.Domain.Entities;

public class Role : BaseEntity
{
    [Required]
    public string Name { get; set; } = string.Empty;

    public ICollection<UserRole> UserRoles { get; set; } = new List<UserRole>();
    public ICollection<RolePermission> RolePermissions { get; set; } = new List<RolePermission>();
}
