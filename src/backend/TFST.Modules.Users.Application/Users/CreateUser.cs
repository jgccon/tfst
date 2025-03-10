using MediatR;
using Microsoft.EntityFrameworkCore;
using TFST.Modules.Users.Application.Models;
using TFST.Modules.Users.Domain.Entities;
using TFST.Persistence;

namespace TFST.Modules.Users.Application.Users;

public record CreateUserCommand(string Email, string FirstName, string LastName) : IRequest<UserProjection>;

public class CreateUserHandler : IRequestHandler<CreateUserCommand, UserProjection>
{
    private readonly UsersDbContext _dbContext;

    public CreateUserHandler(UsersDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<UserProjection> Handle(CreateUserCommand request, CancellationToken cancellationToken)
    {
        var existingUser = await _dbContext.Users.FirstOrDefaultAsync(u => u.Email == request.Email, cancellationToken);
        if (existingUser != null)
        {
            throw new InvalidOperationException("User with this email already exists.");
        }

        var user = new User(Guid.NewGuid().ToString(), request.Email, request.FirstName, request.LastName);

        _dbContext.Users.Add(user);
        await _dbContext.SaveChangesAsync(cancellationToken);

        return new UserProjection(user.Id, user.Email, user.FirstName, user.LastName, new List<string>());
    }
}
