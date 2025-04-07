using System.ComponentModel.DataAnnotations;

namespace TFST.AuthServer.Models;

public class LoginViewModel
{
    [Required(ErrorMessage = "{{0} is required")]
    [EmailAddress(ErrorMessage = "{0} is not a valid email address")]
    [StringLength(50, ErrorMessage = "{0} must be between {2} and {1} characters.", MinimumLength = 10)]
    [DataType(DataType.EmailAddress)]
    public string? Email { get; set; }

    [Required(ErrorMessage = "{0} is required")]
    [StringLength(30, ErrorMessage = "{0} must be between {2} and {1} characters.", MinimumLength = 8)]
    [DataType(DataType.Password)]
    public string? Password { get; set; }

    [Display(Name = "¿Remember me?")]
    public bool RememberMe { get; set; }
}