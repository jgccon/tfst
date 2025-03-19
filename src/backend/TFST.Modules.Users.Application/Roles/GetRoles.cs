using MediatR;
using Microsoft.EntityFrameworkCore;
using TFST.Modules.Users.Application.Expressions;
using TFST.Modules.Users.Application.Models;
using TFST.Modules.Users.Persistence;

namespace TFST.Modules.Users.Application.Roles;

public record GetRolesQuery() : IRequest<List<Role>>;

public class GetRolesHandler : IRequestHandler<GetRolesQuery, List<Role>>
{
    private readonly UsersDbContext _dbContext;

    public GetRolesHandler(UsersDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<List<Role>> Handle(GetRolesQuery request, CancellationToken cancellationToken)
    {
        return await _dbContext.Roles
            .Select(RoleExpressions.Projection)
            .ToListAsync(cancellationToken);
    }
}
