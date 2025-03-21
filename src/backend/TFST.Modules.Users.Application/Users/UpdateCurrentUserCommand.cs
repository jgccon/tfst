﻿using MediatR;
using Microsoft.EntityFrameworkCore;
using TFST.Modules.Users.Application.Models;
using TFST.Modules.Users.Application.Expressions;
using TFST.Modules.Users.Persistence;

namespace TFST.Modules.Users.Application.Users;

public record UpdateCurrentUserCommand(Guid Id, UpdateUserModel User) : IRequest<Application.Models.User>;

public class UpdateCurrentUserHandler : IRequestHandler<UpdateCurrentUserCommand, User>
{
    private readonly UsersDbContext _dbContext;

    public UpdateCurrentUserHandler(UsersDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<User> Handle(UpdateCurrentUserCommand request, CancellationToken cancellationToken)
    {
        var user = await _dbContext.Users
            .FirstOrDefaultAsync(u => u.Id == request.Id, cancellationToken);

        if (user is null)
            throw new KeyNotFoundException("User not found.");

        user.FirstName = request.User.FirstName;
        user.LastName = request.User.LastName;
        user.UpdatedAt = DateTime.UtcNow;

        await _dbContext.SaveChangesAsync(cancellationToken);

        return UserExpressions.Projection.Compile().Invoke(user);
    }
}
