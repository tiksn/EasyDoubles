namespace EasyDoubles.Test.Repositories;

using EasyDoubles.Test.Entities;
using EasyDoubles.Test.Entities.Configurations;
using TIKSN.Data.EntityFrameworkCore;

public class EntityCatalogItemRepository :
    EntityQueryRepository<CatalogContext, CatalogItem, int>,
    ICatalogItemRepository,
    ICatalogItemQueryRepository
{
    public EntityCatalogItemRepository(CatalogContext dbContext)
        : base(dbContext)
    {
    }
}
