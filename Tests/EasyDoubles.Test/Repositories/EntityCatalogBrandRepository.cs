namespace EasyDoubles.Test.Repositories;

using EasyDoubles.Test.Entities;
using EasyDoubles.Test.Entities.Configurations;
using TIKSN.Data.EntityFrameworkCore;

public class EntityCatalogBrandRepository :
    EntityQueryRepository<CatalogContext, CatalogBrand, int>,
    ICatalogBrandRepository,
    ICatalogBrandQueryRepository
{
    public EntityCatalogBrandRepository(CatalogContext dbContext)
        : base(dbContext)
    {
    }
}
