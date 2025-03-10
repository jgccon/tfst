using MediatR;
using Microsoft.EntityFrameworkCore;
using TFST.Modules.Users.Persistence;

namespace TFST.Modules.Users.Application.Users;

public record DeleteCurrentUserCommand(Guid UserId) : IRequest<bool>;

public class DeleteCurrentUserHandler : IRequestHandler<DeleteCurrentUserCommand, bool>
{
    private readonly UsersDbContext _dbContext;

    public DeleteCurrentUserHandler(UsersDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<bool> Handle(DeleteCurrentUserCommand request, CancellationToken cancellationToken)
    {
        var user = await _dbContext.Users
            .FirstOrDefaultAsync(u => u.Id == request.UserId, cancellationToken);

        if (user is null)
            throw new KeyNotFoundException("User not found.");

        user.MarkAsDeleted();
        await _dbContext.SaveChangesAsync(cancellationToken);

        return true;
    }
}
