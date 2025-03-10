namespace TFST.Modules.Users.Application.Models;
public record UserProjection(Guid Id, string Email, string FirstName, string LastName, List<string> Roles);
