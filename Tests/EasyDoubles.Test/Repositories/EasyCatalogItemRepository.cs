namespace EasyDoubles.Test.Repositories;

using EasyDoubles.Test.Entities;

public class EasyCatalogItemRepository :
    EasyRepository<CatalogItem, int>,
    ICatalogItemRepository,
    ICatalogItemQueryRepository
{
    public EasyCatalogItemRepository(IEasyStores easyStores)
        : base(easyStores)
    {
    }
}
