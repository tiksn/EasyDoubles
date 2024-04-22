namespace EasyDoubles.Test;

using System.Threading.Tasks;
using Xunit;

public class EasyFileRepositoryTest
{
    [Fact]
    public async Task GivenEmptyStores_WhenSingleFileUploaded_ThenValuesShouldMatch()
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

    [Fact]
    public async Task GivenTwoFiledStores_WhenDeletedById_ThenValuesShouldBeRemoved()
    {
        // Arrange
        var stores = new EasyStores();
        var random = new Random(524943869);
        var easyFileBucket = stores.ResolveBucket<int, string>("images");
        var easyFileRepository = new EasyFileRepository<int, string>(stores, new RandomIdentityGenerator(random), "images");
        var content1 = new byte[200];
        random.NextBytes(content1);
        var content2 = new byte[100];
        random.NextBytes(content2);
        await easyFileRepository.UploadAsync(1000, "company.png", content1, "Metadata1", default);
        await easyFileRepository.UploadAsync(1001, "logo.png", content2, "Metadata2", default);

        // Act
        await easyFileRepository.DeleteByIdAsync(1000, default);

        // Assert
        Assert.NotNull(easyFileBucket.BucketContent);
        Assert.DoesNotContain(easyFileBucket.BucketContent, x => string.Equals(x.Value.Path, "company.png", StringComparison.OrdinalIgnoreCase));
    }

    [Fact]
    public async Task GivenTwoFiledStores_WhenDeletedByPath_ThenValuesShouldBeRemoved()
    {
        // Arrange
        var stores = new EasyStores();
        var random = new Random(524943869);
        var easyFileBucket = stores.ResolveBucket<int, string>("images");
        var easyFileRepository = new EasyFileRepository<int, string>(stores, new RandomIdentityGenerator(random), "images");
        var content1 = new byte[200];
        random.NextBytes(content1);
        var content2 = new byte[100];
        random.NextBytes(content2);
        await easyFileRepository.UploadAsync(1000, "company.png", content1, "Metadata1", default);
        await easyFileRepository.UploadAsync(1001, "logo.png", content2, "Metadata2", default);

        // Act
        await easyFileRepository.DeleteAsync("company.png", default);

        // Assert
        Assert.NotNull(easyFileBucket.BucketContent);
        Assert.DoesNotContain(easyFileBucket.BucketContent, x => string.Equals(x.Value.Path, "company.png", StringComparison.OrdinalIgnoreCase));
    }

    [Fact]
    public async Task GivenTwoFiledStores_WhenDownloadedById_ThenValuesShouldMatch()
    {
        // Arrange
        var stores = new EasyStores();
        var random = new Random(524943869);
        var easyFileRepository = new EasyFileRepository<int, string>(stores, new RandomIdentityGenerator(random), "images");
        var content1 = new byte[200];
        random.NextBytes(content1);
        var content2 = new byte[100];
        random.NextBytes(content2);
        await easyFileRepository.UploadAsync(1000, "company.png", content1, "Metadata1", default);
        await easyFileRepository.UploadAsync(1001, "logo.png", content2, "Metadata2", default);

        // Act
        var file = await easyFileRepository.DownloadByIdAsync(1000, default);

        // Assert
        Assert.NotNull(file);
        Assert.Equal(1000, file.ID);
        Assert.Equal("company.png", file.Path);
        Assert.Equal(content1, file.Content);
    }

    [Fact]
    public async Task GivenTwoFiledStores_WhenDownloadedByPath_ThenValuesShouldMatch()
    {
        // Arrange
        var stores = new EasyStores();
        var random = new Random(524943869);
        var easyFileRepository = new EasyFileRepository<int, string>(stores, new RandomIdentityGenerator(random), "images");
        var content1 = new byte[200];
        random.NextBytes(content1);
        var content2 = new byte[100];
        random.NextBytes(content2);
        await easyFileRepository.UploadAsync(1000, "company.png", content1, "Metadata1", default);
        await easyFileRepository.UploadAsync(1001, "logo.png", content2, "Metadata2", default);

        // Act
        var file = await easyFileRepository.DownloadAsync("company.png", default);

        // Assert
        Assert.NotNull(file);
        Assert.Equal("company.png", file.Path);
        Assert.Equal(content1, file.Content);
    }

    [Fact]
    public async Task GivenTwoFiledStores_WhenDownloadedOnlyMetadata_ThenValuesShouldMatch()
    {
        // Arrange
        var stores = new EasyStores();
        var random = new Random(524943869);
        var easyFileRepository = new EasyFileRepository<int, string>(stores, new RandomIdentityGenerator(random), "images");
        var content1 = new byte[200];
        random.NextBytes(content1);
        var content2 = new byte[100];
        random.NextBytes(content2);
        await easyFileRepository.UploadAsync(1000, "company.png", content1, "Metadata1", default);
        await easyFileRepository.UploadAsync(1001, "logo.png", content2, "Metadata2", default);

        // Act
        var file = await easyFileRepository.DownloadOnlyMetadataAsync(1000, default);

        // Assert
        Assert.NotNull(file);
        Assert.Equal(1000, file.ID);
        Assert.Equal("company.png", file.Path);
        Assert.Equal("Metadata1", file.Metadata);
    }

    [Fact]
    public async Task GivenTwoFiledStores_WhenDownloadedWithMetadata_ThenValuesShouldMatch()
    {
        // Arrange
        var stores = new EasyStores();
        var random = new Random(524943869);
        var easyFileRepository = new EasyFileRepository<int, string>(stores, new RandomIdentityGenerator(random), "images");
        var content1 = new byte[200];
        random.NextBytes(content1);
        var content2 = new byte[100];
        random.NextBytes(content2);
        await easyFileRepository.UploadAsync(1000, "company.png", content1, "Metadata1", default);
        await easyFileRepository.UploadAsync(1001, "logo.png", content2, "Metadata2", default);

        // Act
        var file = await easyFileRepository.DownloadWithMetadataAsync(1000, default);

        // Assert
        Assert.NotNull(file);
        Assert.Equal(1000, file.ID);
        Assert.Equal("company.png", file.Path);
        Assert.Equal("Metadata1", file.Metadata);
        Assert.Equal(content1, file.Content);
    }

    [Fact]
    public async Task GivenTwoFiledStores_WhenExistsById_ThenValuesShouldBeFound()
    {
        // Arrange
        var stores = new EasyStores();
        var random = new Random(524943869);
        var easyFileRepository = new EasyFileRepository<int, string>(stores, new RandomIdentityGenerator(random), "images");
        var content1 = new byte[200];
        random.NextBytes(content1);
        var content2 = new byte[100];
        random.NextBytes(content2);
        await easyFileRepository.UploadAsync(1000, "company.png", content1, "Metadata1", default);
        await easyFileRepository.UploadAsync(1001, "logo.png", content2, "Metadata2", default);

        // Act
        var exists = await easyFileRepository.ExistsByIdAsync(1000, default);

        // Assert
        Assert.True(exists);
    }

    [Fact]
    public async Task GivenTwoFiledStores_WhenExistsById_ThenValuesShouldBeMissed()
    {
        // Arrange
        var stores = new EasyStores();
        var random = new Random(524943869);
        var easyFileRepository = new EasyFileRepository<int, string>(stores, new RandomIdentityGenerator(random), "images");
        var content1 = new byte[200];
        random.NextBytes(content1);
        var content2 = new byte[100];
        random.NextBytes(content2);
        await easyFileRepository.UploadAsync(1000, "company.png", content1, "Metadata1", default);
        await easyFileRepository.UploadAsync(1001, "logo.png", content2, "Metadata2", default);

        // Act
        var exists = await easyFileRepository.ExistsByIdAsync(1003, default);

        // Assert
        Assert.False(exists);
    }

    [Fact]
    public async Task GivenTwoFiledStores_WhenExistsByPath_ThenValuesShouldBeFound()
    {
        // Arrange
        var stores = new EasyStores();
        var random = new Random(524943869);
        var easyFileRepository = new EasyFileRepository<int, string>(stores, new RandomIdentityGenerator(random), "images");
        var content1 = new byte[200];
        random.NextBytes(content1);
        var content2 = new byte[100];
        random.NextBytes(content2);
        await easyFileRepository.UploadAsync(1000, "company.png", content1, "Metadata1", default);
        await easyFileRepository.UploadAsync(1001, "logo.png", content2, "Metadata2", default);

        // Act
        var exists = await easyFileRepository.ExistsAsync("company.png", default);

        // Assert
        Assert.True(exists);
    }

    [Fact]
    public async Task GivenTwoFiledStores_WhenExistsByPath_ThenValuesShouldBeMissed()
    {
        // Arrange
        var stores = new EasyStores();
        var random = new Random(524943869);
        var easyFileRepository = new EasyFileRepository<int, string>(stores, new RandomIdentityGenerator(random), "images");
        var content1 = new byte[200];
        random.NextBytes(content1);
        var content2 = new byte[100];
        random.NextBytes(content2);
        await easyFileRepository.UploadAsync(1000, "company.png", content1, "Metadata1", default);
        await easyFileRepository.UploadAsync(1001, "logo.png", content2, "Metadata2", default);

        // Act
        var exists = await easyFileRepository.ExistsAsync("enterprise.png", default);

        // Assert
        Assert.False(exists);
    }

    [Fact]
    public async Task GivenTwoFiledStores_WhenSingleFileUploaded_ThenValuesShouldMatch()
    {
        // Arrange
        var stores = new EasyStores();
        var random = new Random(524943869);
        var easyFileBucket = stores.ResolveBucket<int, string>("images");
        var easyFileRepository = new EasyFileRepository<int, string>(stores, new RandomIdentityGenerator(random), "images");
        var content1 = new byte[200];
        random.NextBytes(content1);
        var content2 = new byte[100];
        random.NextBytes(content2);

        // Act
        await easyFileRepository.UploadAsync(1000, "company.png", content1, "Metadata1", default);
        await easyFileRepository.UploadAsync("logo.png", content2, default);

        // Assert
        Assert.NotNull(easyFileBucket.BucketContent);
        var logo = Assert.Single(easyFileBucket.BucketContent, x => string.Equals(x.Value.Path, "logo.png", StringComparison.OrdinalIgnoreCase));
        Assert.NotEqual(1000, logo.Key);
        Assert.Equal("logo.png", logo.Value.Path);
        Assert.Null(logo.Value.Metadata);
        Assert.Equal(content2, logo.Value.Content.ToArray());
    }
}
