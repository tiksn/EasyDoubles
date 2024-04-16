namespace EasyDoubles.Test.Entities.Configurations;

using Microsoft.EntityFrameworkCore;

public class CatalogContext : DbContext
{
    public CatalogContext(DbContextOptions<CatalogContext> options)
        : base(options)
    {
    }

    public DbSet<CatalogBrand> CatalogBrands { get; set; }

    public DbSet<CatalogItem> CatalogItems { get; set; }

    public DbSet<CatalogType> CatalogTypes { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        _ = modelBuilder.ApplyConfiguration(new CatalogBrandEntityTypeConfiguration());
        _ = modelBuilder.ApplyConfiguration(new CatalogTypeEntityTypeConfiguration());
        _ = modelBuilder.ApplyConfiguration(new CatalogItemEntityTypeConfiguration());
    }
}
