using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using TFST.Modules.Users.Domain.Entities;
using TFST.Modules.Users.Persistence;

namespace TFST.Modules.Users.Presentation.Middlewares;

public class UserSyncMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<UserSyncMiddleware> _logger;

    public UserSyncMiddleware(RequestDelegate next, ILogger<UserSyncMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task Invoke(HttpContext context, UsersDbContext dbContext)
    {
        if (context.User?.Identity is not { IsAuthenticated: true })
        {
            await _next(context);
            return;
        }

        var userIdClaim = context.User.FindFirst(ClaimTypes.NameIdentifier);
        var emailClaim = context.User.FindFirst(ClaimTypes.Email);
        var firstNameClaim = context.User.FindFirst(ClaimTypes.GivenName);
        var lastNameClaim = context.User.FindFirst(ClaimTypes.Surname);

        if (userIdClaim == null || string.IsNullOrWhiteSpace(userIdClaim.Value))
        {
            await _next(context);
            return;
        }

        var sub = Guid.Parse(userIdClaim.Value); 

        var user = await dbContext.Users.FirstOrDefaultAsync(u => u.Id == sub);
        if (user == null)
        {
            _logger.LogInformation($"User with sub {sub} not found, creating...");

            user = new User
            {
                Id = sub,
                Email = emailClaim?.Value ?? throw new InvalidOperationException("Email is required"),
                FirstName = firstNameClaim?.Value ?? string.Empty,
                LastName = lastNameClaim?.Value ?? string.Empty
            };

            dbContext.Users.Add(user);
            await dbContext.SaveChangesAsync();
        }

        await _next(context);
    }
}
