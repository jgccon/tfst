using System.ComponentModel.DataAnnotations;
using TFST.SharedKernel.Domain.Entities;

namespace TFST.Modules.Users.Domain.Entities;

public class User : IdentifiableEntity
{
    [Required]
    public string Email { get; set; } = string.Empty;

    public string? FirstName { get; set; }
    public string? LastName { get; set; }

    public bool IsProfessional { get; set; } = false;

    public ICollection<UserRole> UserRoles { get; set; } = new List<UserRole>();
}
