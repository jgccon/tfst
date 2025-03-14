using Microsoft.EntityFrameworkCore;
using TFST.Modules.ProfessionalProfiles.Domain.Entities;

namespace TFST.Modules.ProfessionalProfiles.Persistence;

public class ProfessionalProfilesDbContext : DbContext
{
    public ProfessionalProfilesDbContext(DbContextOptions<ProfessionalProfilesDbContext> options)
    : base(options) { }

    public DbSet<ProfessionalProfile> ProfessionalProfiles { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.HasDefaultSchema("profiles");

        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ProfessionalProfilesDbContext).Assembly);
    }

    // TODO: add EntityTypeConfigurations (/Configurations)
}
