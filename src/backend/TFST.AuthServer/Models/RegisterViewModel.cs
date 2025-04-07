using System.ComponentModel.DataAnnotations;

namespace TFST.AuthServer.Models;

public class RegisterViewModel
{
    [Required(ErrorMessage = "{0} is required")]
    [StringLength(50, ErrorMessage = "{0} must be between {2} and {1} characters.", MinimumLength = 10)]
    [EmailAddress(ErrorMessage = "{0} is not a valid email address")]
    [DataType(DataType.EmailAddress)]
    public string? Email { get; set; }

    [Required(ErrorMessage = "{0} is required")]
    [StringLength(30, ErrorMessage = "{0} must be between {2} and {1} characters.", MinimumLength = 8)]
    [DataType(DataType.Password)]
    public string? Password { get; set; }

    [Compare(nameof(Password), ErrorMessage = "{0} and {1} do not match")]
    [DataType(DataType.Password)]
    public string? ConfirmPassword { get; set; }

    /* [Required(ErrorMessage = "{0} is required")]
    [StringLength(50, ErrorMessage = "{0} must be between {2} and {1} characters.", MinimumLength = 3)]
    [RegularExpression(@"^[a-zA-ZáéíóúÁÉÍÓÚñÑüÜ][a-zA-ZáéíóúÁÉÍÓÚñÑüÜ\s.]+$", ErrorMessage = "{0} must start with a letter and only contain letters, spaces and dots.")]
    public string? Name { get; set; }

    [Required(ErrorMessage = "{0} is required")]
    [StringLength(50, ErrorMessage = "{0} must be between {2} and {1} characters.", MinimumLength = 3)]
    [RegularExpression(@"^[a-zA-ZáéíóúÁÉÍÓÚñÑüÜ][a-zA-ZáéíóúÁÉÍÓÚñÑüÜ\s.]+$", ErrorMessage = "{0} must start with a letter and only contain letters, spaces and dots.")]
    public string? LastName { get; set; } */
}