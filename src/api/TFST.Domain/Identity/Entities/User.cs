using System.ComponentModel.DataAnnotations;
using TFST.Domain.Base;

namespace TFST.Domain.Identity.Entities;

public class User : BaseEntity
{
    [Required]
    public string Email { get; set; } = string.Empty;

    public string? FirstName { get; set; }
    public string? LastName { get; set; }

    public bool IsProfessional { get; set; } = false;

    // Relación con roles
    public ICollection<UserRole> UserRoles { get; set; } = new List<UserRole>();
}
