namespace EasyDoubles.Test.Repositories;

using System.Linq;
using EasyDoubles.Test.Entities;
using EasyDoubles.Test.Entities.Configurations;
using TIKSN.Data.EntityFrameworkCore;

public class EntityCatalogTypeRepository :
    EntityQueryRepository<CatalogContext, CatalogType, int>,
    ICatalogTypeRepository,
    ICatalogTypeQueryRepository
{
    public EntityCatalogTypeRepository(CatalogContext dbContext)
        : base(dbContext)
    {
    }

    protected override IOrderedQueryable<CatalogType> OrderedEntities
        => this.Entities.OrderBy(x => x.ID);
}
