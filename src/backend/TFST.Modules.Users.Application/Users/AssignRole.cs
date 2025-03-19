using MediatR;
using Microsoft.EntityFrameworkCore;
using TFST.Modules.Users.Persistence;
using TFST.Modules.Users.Domain.Entities;

namespace TFST.Modules.Users.Application.Users;

public record AssignRoleCommand(Guid UserId, Guid RoleId) : IRequest<bool>;

public class AssignRoleHandler : IRequestHandler<AssignRoleCommand, bool>
{
    private readonly UsersDbContext _dbContext;

    public AssignRoleHandler(UsersDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<bool> Handle(AssignRoleCommand request, CancellationToken cancellationToken)
    {
        var user = await _dbContext.Users
            .Include(u => u.UserRoles)
            .FirstOrDefaultAsync(u => u.Id == request.UserId, cancellationToken);

        if (user is null)
            throw new KeyNotFoundException($"User with ID {request.UserId} not found.");

        var role = await _dbContext.Roles
            .FirstOrDefaultAsync(r => r.Id == request.RoleId, cancellationToken);

        if (role is null)
            throw new KeyNotFoundException($"Role with ID {request.RoleId} not found.");

        if (user.UserRoles.Any(ur => ur.RoleId == request.RoleId))
            throw new InvalidOperationException($"User {user.Email} already has role {role.Name}.");

        var userRole = new UserRole
        {
            UserId = user.Id,
            RoleId = role.Id
        };

        _dbContext.UserRoles.Add(userRole);
        await _dbContext.SaveChangesAsync(cancellationToken);

        return true;
    }
}
