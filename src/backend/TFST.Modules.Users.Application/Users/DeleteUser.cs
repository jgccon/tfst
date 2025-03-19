using MediatR;
using Microsoft.EntityFrameworkCore;
using TFST.Modules.Users.Persistence;

namespace TFST.Modules.Users.Application.Users;

public record DeleteUserCommand(Guid UserId) : IRequest<bool>;

public class DeleteUserHandler : IRequestHandler<DeleteUserCommand, bool>
{
    private readonly UsersDbContext _dbContext;

    public DeleteUserHandler(UsersDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<bool> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
    {
        var user = await _dbContext.Users
            .FirstOrDefaultAsync(u => u.Id == request.UserId, cancellationToken);

        if (user is null)
            throw new KeyNotFoundException($"User with ID {request.UserId} not found.");

        user.MarkAsDeleted();
        await _dbContext.SaveChangesAsync(cancellationToken);

        return true;
    }
}
