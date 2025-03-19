using MediatR;
using Microsoft.EntityFrameworkCore;
using TFST.Modules.Users.Application.Models;
using TFST.Modules.Users.Application.Expressions;
using TFST.Modules.Users.Persistence;

namespace TFST.Modules.Users.Application.Users;

public record GetUserByIdQuery(Guid Id) : IRequest<User>;

public class GetUserByIdHandler : IRequestHandler<GetUserByIdQuery, User>
{
    private readonly UsersDbContext _dbContext;

    public GetUserByIdHandler(UsersDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<User> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
    {
        var user = await _dbContext.Users
            .Where(u => u.Id == request.Id)
            .Select(UserExpressions.Projection)
            .FirstOrDefaultAsync(cancellationToken);

        if (user is null)
            throw new KeyNotFoundException($"User with ID {request.Id} not found.");

        return user;
    }
}
