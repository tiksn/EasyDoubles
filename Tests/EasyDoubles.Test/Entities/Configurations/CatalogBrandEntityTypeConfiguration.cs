namespace EasyDoubles.Test.Entities.Configurations;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class CatalogBrandEntityTypeConfiguration
    : IEntityTypeConfiguration<CatalogBrand>
{
    public void Configure(EntityTypeBuilder<CatalogBrand> builder)
    {
        _ = builder.ToTable("CatalogBrand");

        _ = builder.Property(cb => cb.Brand)
            .HasMaxLength(100);
    }
}
