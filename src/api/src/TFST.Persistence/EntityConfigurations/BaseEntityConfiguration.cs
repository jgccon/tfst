using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TFST.Domain.Base;


namespace TFST.Persistence.EntityConfigurations;

public class BaseEntityConfiguration<T> : IEntityTypeConfiguration<T> where T : BaseEntity
{
    public virtual void Configure(EntityTypeBuilder<T> builder)
    {
        builder.HasKey(e => e.Id);

        // Configure Guid as primary key
        builder.Property(e => e.Id)
            .HasConversion(
                Guid => Guid.ToString(), // From Guid to string
                GuidString => Guid.Parse(GuidString) // From string to Guid
            );

        // Filter out deleted entities
        builder.HasQueryFilter(e => !e.IsDeleted);

        builder.Property(e => e.CreatedAt)
            .IsRequired()
            .HasDefaultValueSql("GETUTCDATE()");
    }
}
