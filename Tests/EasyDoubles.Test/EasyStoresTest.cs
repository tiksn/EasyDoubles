namespace EasyDoubles.Test;

using EasyDoubles.Test.Entities;
using Xunit;

public class EasyStoresTest
{
    [Fact]
    public void GivenStores_WhenStoreResolvedDifferent_ThenTheyShouldNotBeTheSameObject()
    {
        // Arrange
        var stores = new EasyStores();

        // Act
        var easyStore1 = stores.Resolve<CatalogType, int>();
        var easyStore2 = stores.Resolve<CatalogBrand, int>();

        // Assert
        Assert.NotNull(easyStore1);
        Assert.NotNull(easyStore2);
        Assert.NotSame(easyStore1, easyStore2);
        Assert.NotSame(easyStore1.Entities, easyStore2.Entities);
    }

    [Fact]
    public void GivenStores_WhenStoreResolvedTwice_ThenTheyShouldBeTheSameObject()
    {
        // Arrange
        var stores = new EasyStores();

        // Act
        var easyStore1 = stores.Resolve<CatalogType, int>();
        var easyStore2 = stores.Resolve<CatalogType, int>();

        // Assert
        Assert.NotNull(easyStore1);
        Assert.NotNull(easyStore2);
        Assert.Same(easyStore1, easyStore2);
        Assert.Same(easyStore1.Entities, easyStore2.Entities);
    }
}
