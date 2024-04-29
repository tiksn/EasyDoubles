namespace EasyDoubles.Test.Entities.Configurations;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class CatalogItemEntityTypeConfiguration
    : IEntityTypeConfiguration<CatalogItem>
{
    public void Configure(EntityTypeBuilder<CatalogItem> builder)
    {
        _ = builder.ToTable("Catalog");

        _ = builder.Property(ci => ci.Name)
            .HasMaxLength(50);

        _ = builder.HasOne(ci => ci.CatalogBrand)
            .WithMany();

        _ = builder.HasOne(ci => ci.CatalogType)
            .WithMany();

        _ = builder.HasIndex(ci => ci.Name);
    }
}
