namespace TFST.Modules.Users.Application.Models;

public record User(Guid Id, string Email, string FirstName, string LastName, List<string> Roles)
{
    public User() : this(Guid.Empty, string.Empty, string.Empty, string.Empty, new List<string>()) { }
}
