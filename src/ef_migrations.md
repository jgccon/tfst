# Managing Migrations and Schemas in TFST

This document provides guidelines for managing Entity Framework Core (EF Core) migrations in the TFST solution, ensuring that each module has its own schema while sharing the same database.

## 1. Database Structure with Schemas

Each module in TFST has its own **DbContext** and stores data in a separate **schema** within the shared database. This approach helps:

- Organize data cleanly.
- Avoid table name collisions across modules.
- Improve security and permission management.

### **Schemas per Module**
| Module                    | Schema    | Example Tables         |
|---------------------------|----------|------------------------|
| **AuthServer**            | `auth`    | `auth.Users`, `auth.Roles` |
| **Users Module**          | `users`   | `users.Users`, `users.Roles` |
| **Professional Profiles** | `profiles`| `profiles.Profiles` |

---

## 2. Configuring `DbContext` for Each Module

### **AuthServer (`AuthDbContext`)**
```csharp
public class AuthDbContext : IdentityDbContext<IdentityUser, IdentityRole, string>
{
    public AuthDbContext(DbContextOptions<AuthDbContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.HasDefaultSchema("auth");
        modelBuilder.Entity<IdentityUser>().ToTable("Users", "auth");
        modelBuilder.Entity<IdentityRole>().ToTable("Roles", "auth");
    }
}
```

### **Users Module (`UsersDbContext`)**
```csharp
public class UsersDbContext : DbContext
{
    public UsersDbContext(DbContextOptions<UsersDbContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.HasDefaultSchema("users");
        // Etc...
    }
}
```

### **Professional Profiles Module (`ProfessionalProfilesDbContext`)**
```csharp
public class ProfessionalProfilesDbContext : DbContext
{
    public ProfessionalProfilesDbContext(DbContextOptions<ProfessionalProfilesDbContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.HasDefaultSchema("profiles");
        // Etc...
    }
}
```

---

## 3. Running Migrations for Each Module
Each module has its own migrations stored in its respective project. **Use the `--context` flag to generate and apply migrations correctly.**

### **Create and Apply Migrations for AuthServer**
```sh
dotnet ef migrations add InitialAuthSetup --context AuthDbContext --project src/backend/TFST.AuthServer
dotnet ef database update --context AuthDbContext --project src/backend/TFST.AuthServer
```

### **Create and Apply Migrations for Users Module**
```sh
dotnet ef migrations add InitialUsersSetup --context UsersDbContext --project src/backend/TFST.Modules.Users.Persistence --startup-project src/backend/TFST.API
dotnet ef database update --context UsersDbContext --project src/backend/TFST.API
```

### **Create and Apply Migrations for Professional Profiles Module**
```sh
dotnet ef migrations add InitialProfilesSetup --context ProfessionalProfilesDbContext --project src/backend/TFST.Modules.ProfessionalProfiles.Persistence --startup-project src/backend/TFST.API
dotnet ef database update --context ProfessionalProfilesDbContext --project src/backend/TFST.API
```

---

## 4. Ensuring Migrations Run at Startup
To prevent issues in production, migrations should only be applied if there are pending migrations. Add this logic in `Program.cs` for each service:

### **`TFST.AuthServer/Program.cs`**
```csharp
if (builder.Configuration.GetValue<bool>("FeatureFlags:MigrateAtStartup"))
{
    using var scope = app.Services.CreateScope();
    var dbContext = scope.ServiceProvider.GetRequiredService<AuthDbContext>();
    if (dbContext.Database.GetPendingMigrations().Any())
    {
        dbContext.Database.Migrate();
    }
}
```

### **`TFST.API/Program.cs`**
```csharp
if (builder.Configuration.GetValue<bool>("FeatureFlags:MigrateAtStartup"))
{
    using var scope = app.Services.CreateScope();
    var usersDbContext = scope.ServiceProvider.GetRequiredService<UsersDbContext>();
    if (usersDbContext.Database.GetPendingMigrations().Any())
    {
        usersDbContext.Database.Migrate();
    }
}
```

---

## 5. Checking Schema in SQL Server
To verify that schemas are correctly applied, run this query in SQL Server:
```sql
SELECT SCHEMA_NAME(schema_id) AS SchemaName, name
FROM sys.tables
ORDER BY SchemaName;
```

Expected output:

| Schema   | Table      |
|----------|-----------|
| auth     | Users     |
| auth     | Roles     |
| users    | Users     |
| users    | Roles     |
| profiles | Profiles  |

---

## 6. Troubleshooting

### **If migration conflicts occur:**
1. Remove the last migration:
   ```sh
   dotnet ef migrations remove --context AuthDbContext
   ```
2. Ensure the correct `MigrationsAssembly` is set in `Program.cs`.
3. Re-run migrations as per the steps above.

### **If the database already exists and is causing issues:**
Run the following SQL command to manually drop and recreate:
```sql
DROP DATABASE TFST;
CREATE DATABASE TFST;
```
Then re-run:
```sh
dotnet ef database update --context AuthDbContext
```

---

## Summary
✅ **Each module has its own schema (`auth`, `users`, `profiles`).**  
✅ **Migrations are generated and applied separately for each `DbContext`.**  
✅ **Migrations are only applied at startup if necessary.**  
✅ **Database structure is easily verifiable via SQL queries.**  

🚀 **This setup ensures a well-structured and scalable monolithic architecture!**
