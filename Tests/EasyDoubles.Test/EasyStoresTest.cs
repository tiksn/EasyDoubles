namespace EasyDoubles.Test;

using EasyDoubles.Test.Entities;
using Xunit;

public class EasyStoresTest
{
    [Fact]
    public void GivenStores_WhenBucketResolvedDifferent_ThenTheyShouldNotBeTheSameObject()
    {
        // Arrange
        var stores = new EasyStores();

        // Act
        var easyFileBucket1 = stores.ResolveBucket<int, string>("videos");
        var easyFileBucket2 = stores.ResolveBucket<int, string>("photos");

        // Assert
        Assert.NotNull(easyFileBucket1);
        Assert.NotNull(easyFileBucket2);
        Assert.NotSame(easyFileBucket1, easyFileBucket2);
        Assert.NotSame(easyFileBucket1.BucketContent, easyFileBucket2.BucketContent);
        Assert.NotSame(easyFileBucket1.BucketName, easyFileBucket2.BucketName);
    }

    [Fact]
    public void GivenStores_WhenBucketResolvedTwice_ThenTheyShouldBeTheSameObject()
    {
        // Arrange
        var stores = new EasyStores();

        // Act
        var easyFileBucket1 = stores.ResolveBucket<int, string>("videos");
        var easyFileBucket2 = stores.ResolveBucket<int, string>("videos");

        // Assert
        Assert.NotNull(easyFileBucket1);
        Assert.NotNull(easyFileBucket2);
        Assert.Same(easyFileBucket1, easyFileBucket2);
        Assert.Same(easyFileBucket1.BucketContent, easyFileBucket2.BucketContent);
        Assert.Same(easyFileBucket1.BucketName, easyFileBucket2.BucketName);
    }

    [Fact]
    public void GivenStores_WhenResolvingEmptyNamedBucket_ThenExceptionShowBeThrown()
    {
        // Arrange
        var stores = new EasyStores();

        // Act
        IEasyFileBucket<int, string> ResolveBucket() => stores.ResolveBucket<int, string>(string.Empty);

        // Assert
        _ = Assert.Throws<ArgumentException>(ResolveBucket);
    }

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
