namespace EasyDoubles.Test.Repositories;

using EasyDoubles.Test.Entities;

public class EasyCatalogBrandRepository :
    EasyRepository<CatalogBrand, int>,
    ICatalogBrandRepository,
    ICatalogBrandQueryRepository
{
    public EasyCatalogBrandRepository(IEasyStores easyStores)
        : base(easyStores)
    {
    }
}
