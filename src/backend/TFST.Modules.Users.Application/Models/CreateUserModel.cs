namespace TFST.Modules.Users.Application.Models;

public record CreateUserModel(string Email, string FirstName, string LastName, List<Guid> RoleIds);
