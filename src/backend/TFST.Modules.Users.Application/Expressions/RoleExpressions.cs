using System.Linq.Expressions;

namespace TFST.Modules.Users.Application.Expressions;

public static class RoleExpressions
{
    public static readonly Expression<Func<Domain.Entities.Role, Application.Models.Role>> Projection
        = role => new Models.Role(
            role.Id,
            role.Name
        );
}
