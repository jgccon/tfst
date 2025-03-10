using System.Security.Claims;
using TFST.Domain.Identity.Entities;
using TFST.Persistence;

namespace TFST.API.Middlewares
{
    public class UserSyncMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<UserSyncMiddleware> _logger;

        public UserSyncMiddleware(RequestDelegate next, ILogger<UserSyncMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task Invoke(HttpContext context, ApplicationDbContext dbContext)
        {
            var userIdClaim = context.User.FindFirst(ClaimTypes.NameIdentifier);
            var emailClaim = context.User.FindFirst(ClaimTypes.Email);

            // Si no hay usuario autenticado, continuar sin hacer nada
            if (userIdClaim == null || !Guid.TryParse(userIdClaim.Value, out var userId))
            {
                await _next(context);
                return;
            }

            // Buscar el usuario en la base de datos
            var user = await dbContext.Users.FindAsync(userId);
            if (user == null)
            {
                _logger.LogInformation($"User {userId} not found, creating...");

                user = new User
                {
                    Id = userId,
                    Email = emailClaim?.Value ?? throw new InvalidOperationException("Email is required"),
                };

                dbContext.Users.Add(user);
                await dbContext.SaveChangesAsync();
            }

            await _next(context);
        }
    }

}
