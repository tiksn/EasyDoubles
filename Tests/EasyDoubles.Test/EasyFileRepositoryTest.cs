namespace EasyDoubles.Test;

using System.Threading.Tasks;
using Xunit;

public class EasyFileRepositoryTest
{
    [Fact]
    public async Task GivenStores_WhenSingleFileUploaded_ThenValuesShouldMatch()
    {
        // Arrange
        var stores = new EasyStores();
        var random = new Random(524943869);
        var easyFileBucket = stores.ResolveBucket<int, string>("images");
        var easyFileRepository = new EasyFileRepository<int, string>(stores, new RandomIdentityGenerator(random), "images");
        var content = new byte[100];
        random.NextBytes(content);

        // Act
        await easyFileRepository.UploadAsync(1000, "company.png", content, "Metadata1", default);

        // Assert
        Assert.NotNull(easyFileBucket.BucketContent);
        var single = Assert.Single(easyFileBucket.BucketContent);
        Assert.Equal(1000, single.Key);
        Assert.Equal("company.png", single.Value.Path);
        Assert.Equal("Metadata1", single.Value.Metadata);
        Assert.Equal(content, single.Value.Content.ToArray());
    }
}
