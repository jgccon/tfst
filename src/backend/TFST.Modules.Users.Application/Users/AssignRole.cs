using MediatR;
using Microsoft.EntityFrameworkCore;
using TFST.Modules.Users.Persistence;
using TFST.Persistence;

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
        var user = await _dbContext.Users.Include(u => u.Roles).FirstOrDefaultAsync(u => u.Id == request.UserId, cancellationToken);
        if (user is null) throw new KeyNotFoundException($"User with ID {request.UserId} not found.");

        var role = await _dbContext.Roles.FirstOrDefaultAsync(r => r.Id == request.RoleId, cancellationToken);
        if (role is null) throw new KeyNotFoundException($"Role with ID {request.RoleId} not found.");

        user.AssignRole(role);
        await _dbContext.SaveChangesAsync(cancellationToken);

        return true;
    }
}
