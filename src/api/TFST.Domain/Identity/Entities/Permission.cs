using System;
using System.ComponentModel.DataAnnotations;
using TFST.Domain.Base;

namespace TFST.Domain.Identity.Entities;

public class Permission : BaseEntity
{
    [Required]
    public string Name { get; set; } = string.Empty;
}
