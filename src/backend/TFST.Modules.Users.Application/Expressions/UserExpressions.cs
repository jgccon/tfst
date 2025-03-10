using System.Linq.Expressions;

namespace TFST.Modules.Users.Application.Expressions;

public static class UserExpressions
{
    public static readonly Expression<Func<Domain.Entities.User, Application.Models.User>> Projection
        = user => new Models.User(
            user.Id,
            user.Email,
            user.FirstName ?? string.Empty,
            user.LastName ?? string.Empty,
            user.UserRoles.Select(ur => ur.Role.Name).ToList()
        );
}
