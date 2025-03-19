using MediatR;
using Microsoft.EntityFrameworkCore;
using TFST.Modules.Users.Application.Models;
using TFST.Modules.Users.Application.Expressions;
using TFST.Modules.Users.Persistence;

namespace TFST.Modules.Users.Application.Users;

public record UpdateUserCommand(Guid UserId, string FirstName, string LastName) : IRequest<User>;

public class UpdateUserHandler : IRequestHandler<UpdateUserCommand, User>
{
    private readonly UsersDbContext _dbContext;

    public UpdateUserHandler(UsersDbContext dbContext) => _dbContext = dbContext;

    public async Task<User> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
    {
        var user = await _dbContext.Users
            .FirstOrDefaultAsync(u => u.Id == request.UserId, cancellationToken);

        if (user is null)
            throw new KeyNotFoundException($"User with ID {request.UserId} not found.");

        user.FirstName = request.FirstName;
        user.LastName = request.LastName;
        user.UpdatedAt = DateTime.UtcNow;

        await _dbContext.SaveChangesAsync(cancellationToken);

        return UserExpressions.Projection.Compile().Invoke(user);
    }
}
