using System.ComponentModel.DataAnnotations;
using TFST.SharedKernel.Domain.Entities;

namespace TFST.Modules.Users.Domain.Entities;

public class Permission : BaseEntity
{
    [Required]
    public string Name { get; set; } = string.Empty;
}
