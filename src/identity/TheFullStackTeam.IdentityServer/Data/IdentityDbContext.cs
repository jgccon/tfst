using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TheFullStackTeam.IdentityServer.Domain.Entities;

namespace TheFullStackTeam.IdentityServer.Data
{
    public class IdentityDbContext : IdentityDbContext<Account>
    {
        public IdentityDbContext(DbContextOptions<IdentityDbContext> options) : base(options) { }

        public DbSet<Token> Tokens { get; set; }
        public DbSet<ExternalProviderCredential> ExternalProviders { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Account>()
                .HasIndex(a => a.Email)
                .IsUnique();
        }
    }
}
