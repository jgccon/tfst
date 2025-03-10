using System.ComponentModel.DataAnnotations.Schema;
using TFST.Modules.Users.Domain.Entities;

namespace TFST.Modules.Users.Domain.Entities;

public class UserRole
{
    [ForeignKey("User")]
    public Guid UserId { get; set; }
    public User User { get; set; } = null!;

    [ForeignKey("Role")]
    public Guid RoleId { get; set; }
    public Role Role { get; set; } = null!;
}
