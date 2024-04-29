namespace EasyDoubles.Test.Entities.Configurations;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class CatalogTypeEntityTypeConfiguration
    : IEntityTypeConfiguration<CatalogType>
{
    public void Configure(EntityTypeBuilder<CatalogType> builder)
    {
        _ = builder.ToTable("CatalogType");

        _ = builder.Property(cb => cb.Type)
            .HasMaxLength(100);
    }
}
