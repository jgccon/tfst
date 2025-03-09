using System.ComponentModel.DataAnnotations.Schema;

namespace TFST.Domain.Identity.Entities;

public class UserRole
{
    [ForeignKey("User")]
    public Guid UserId { get; set; }
    public User User { get; set; } = null!;

    [ForeignKey("Role")]
    public Guid RoleId { get; set; }
    public Role Role { get; set; } = null!;
}
