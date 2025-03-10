using MediatR;
using Microsoft.EntityFrameworkCore;
using TFST.Modules.Users.Application.Models;
using TFST.Modules.Users.Persistence;
using TFST.Persistence;

namespace TFST.Modules.Users.Application.Roles;

public record GetRolesQuery() : IRequest<List<RoleProjection>>;

public class GetRolesHandler : IRequestHandler<GetRolesQuery, List<RoleProjection>>
{
    private readonly UsersDbContext _dbContext;

    public GetRolesHandler(UsersDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<List<RoleProjection>> Handle(GetRolesQuery request, CancellationToken cancellationToken)
    {
        return await _dbContext.Roles
            .Select(r => new RoleProjection(r.Id, r.Name))
            .ToListAsync(cancellationToken);
    }
}
