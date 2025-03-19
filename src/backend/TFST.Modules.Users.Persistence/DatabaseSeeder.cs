using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using TFST.Modules.Users.Domain.Entities;
using TFST.Modules.Users.Domain.Enums;
using TFST.SharedKernel.Configuration;

namespace TFST.Modules.Users.Persistence;

public class DatabaseSeeder
{
    private readonly UsersDbContext _dbContext;
    private readonly ILogger<DatabaseSeeder> _logger;
    private readonly AdminSettings _adminSettings;

    public DatabaseSeeder(UsersDbContext dbContext, ILogger<DatabaseSeeder> logger, AdminSettings adminSettings)
    {
        _dbContext = dbContext;
        _logger = logger;
        _adminSettings = adminSettings;
    }

    public async Task SeedAsync()
    {
        _logger.LogInformation("Checking and seeding database...");

        await SeedRolesAsync();
        await SeedPermissionsAsync();
        await SeedAdminUserAsync();

        _logger.LogInformation("Database seeding completed.");
    }

    private async Task SeedRolesAsync()
    {
        var existingRoles = await _dbContext.Roles.Select(r => r.Name).ToListAsync();
        var missingRoles = Enum.GetNames<RoleType>().Except(existingRoles).ToList();

        if (!missingRoles.Any()) return;

        _logger.LogInformation($"Adding missing roles: {string.Join(", ", missingRoles)}");

        foreach (var roleName in missingRoles)
        {
            _dbContext.Roles.Add(new Role { Id = Guid.NewGuid(), Name = roleName });
        }

        await _dbContext.SaveChangesAsync();
    }

    private async Task SeedPermissionsAsync()
    {
        var existingPermissions = await _dbContext.Permissions.Select(p => p.Name).ToListAsync();
        var missingPermissions = Enum.GetNames<PermissionType>().Except(existingPermissions).ToList();

        if (!missingPermissions.Any()) return;

        _logger.LogInformation($"Adding missing permissions: {string.Join(", ", missingPermissions)}");

        foreach (var permissionName in missingPermissions)
        {
            _dbContext.Permissions.Add(new Permission { Id = Guid.NewGuid(), Name = permissionName });
        }

        await _dbContext.SaveChangesAsync();
    }

    private async Task SeedAdminUserAsync()
    {
        if (string.IsNullOrWhiteSpace(_adminSettings.Email)) return;

        var admin = await _dbContext.Users
            .Include(u => u.UserRoles)
            .FirstOrDefaultAsync(u => u.Email == _adminSettings.Email);

        if (admin != null)
        {
            _logger.LogInformation("Admin user already exists.");
            return;
        }

        _logger.LogInformation("Creating default admin user...");

        var adminRole = await _dbContext.Roles.FirstOrDefaultAsync(r => r.Name == RoleType.Admin.ToString());
        if (adminRole == null)
        {
            _logger.LogWarning("Admin role does not exist. Skipping admin creation.");
            return;
        }

        var newAdmin = new User
        {
            Id = Guid.NewGuid(),
            Email = _adminSettings.Email,
            FirstName = _adminSettings.FirstName ?? "Admin",
            LastName = _adminSettings.LastName ?? "User"
        };

        _dbContext.Users.Add(newAdmin);
        _dbContext.UserRoles.Add(new UserRole { UserId = newAdmin.Id, RoleId = adminRole.Id });

        await _dbContext.SaveChangesAsync();
        _logger.LogInformation("Admin user created.");
    }
}
