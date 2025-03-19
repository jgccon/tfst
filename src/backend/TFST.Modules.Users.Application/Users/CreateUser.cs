using MediatR;
using Microsoft.EntityFrameworkCore;
using TFST.Modules.Users.Application.Models;
using TFST.Modules.Users.Application.Expressions;
using TFST.Modules.Users.Persistence;

namespace TFST.Modules.Users.Application.Users;

public record CreateUserCommand(CreateUserModel User) : IRequest<User>;

public class CreateUserHandler : IRequestHandler<CreateUserCommand, User>
{
    private readonly UsersDbContext _dbContext;

    public CreateUserHandler(UsersDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<User> Handle(CreateUserCommand request, CancellationToken cancellationToken)
    {
        var existingUser = await _dbContext.Users
            .AnyAsync(u => u.Email == request.User.Email, cancellationToken);
        if (existingUser)
        {
            throw new InvalidOperationException("User with this email already exists.");
        }

        var user = new Domain.Entities.User
        {
            Id = Guid.NewGuid(),
            Email = request.User.Email,
            FirstName = request.User.FirstName,
            LastName = request.User.LastName
        };

        _dbContext.Users.Add(user);
        await _dbContext.SaveChangesAsync(cancellationToken);

        // 🔥 Aplicamos la proyección definida en UserExpressions
        return UserExpressions.Projection.Compile().Invoke(user);
    }
}
