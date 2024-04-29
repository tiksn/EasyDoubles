namespace EasyDoubles.Test.Repositories;

using EasyDoubles.Test.Entities;

public class EasyCatalogTypeRepository :
    EasyRepository<CatalogType, int>,
    ICatalogTypeRepository,
    ICatalogTypeQueryRepository
{
    public EasyCatalogTypeRepository(IEasyStores easyStores)
        : base(easyStores)
    {
    }
}
