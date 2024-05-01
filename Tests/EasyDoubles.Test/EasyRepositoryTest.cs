namespace EasyDoubles.Test;

using EasyDoubles.Test.Entities;
using EasyDoubles.Test.Repositories;
using EasyDoubles.Test.Services;
using Microsoft.Extensions.DependencyInjection;
using TIKSN.Data;
using Xunit;
using Xunit.Abstractions;

public class EasyRepositoryTest
{
    private readonly ITestOutputHelper testOutputHelper;

    public EasyRepositoryTest(ITestOutputHelper testOutputHelper)
        => this.testOutputHelper = testOutputHelper ?? throw new ArgumentNullException(nameof(testOutputHelper));

    [Fact]
    public async Task GivenInitializedDatabases_WhenPerCollectionSingleEntitiesCreated_ThenAllMatch()
    {
        // Arrange
        var tester = new MultiDatabaseTester(this.testOutputHelper);
        await tester.InitializeAsync(default);
        await tester.ForEachAsync(provider =>
            provider.GetRequiredService<ICatalogInitializer>().InitializeAsync(default));

        // Act
        static async Task CreateSingleEntitiesAsync(IServiceProvider provider)
        {
            var unitOfWorkFactory = provider.GetRequiredService<IUnitOfWorkFactory>();
            var unitOfWork = await unitOfWorkFactory.CreateAsync(default).ConfigureAwait(false);
            await using (unitOfWork.ConfigureAwait(false))
            {
                var typeEntity = new CatalogType()
                {
                    ID = 10101,
                    Type = "Footwear",
                };
                await provider.GetRequiredService<ICatalogTypeRepository>().AddAsync(typeEntity, default).ConfigureAwait(false);

                var brandEntity = new CatalogBrand()
                {
                    ID = 1101,
                    Brand = "Daybird",
                };
                await provider.GetRequiredService<ICatalogBrandRepository>().AddAsync(brandEntity, default).ConfigureAwait(false);

                var itemEntity = new CatalogItem()
                {
                    ID = 100101,
                    CatalogType = typeEntity,
                    CatalogTypeId = typeEntity.ID,
                    CatalogBrand = brandEntity,
                    CatalogBrandId = brandEntity.ID,
                    Name = "Wanderer Black Hiking Boots",
                    Description = "Daybird's Wanderer Hiking Boots in sleek black are perfect for all your outdoor adventures. These boots are made with a waterproof leather upper and a durable rubber sole for superior traction. With their cushioned insole and padded collar, these boots will keep you comfortable all day long.",
                    Price = 109.99m,
                };
                await provider.GetRequiredService<ICatalogItemRepository>().AddAsync(itemEntity, default).ConfigureAwait(false);

                await unitOfWork.CompleteAsync(default).ConfigureAwait(false);
            }
        }

        await tester.ForEachAsync(CreateSingleEntitiesAsync);

        // Assert
        await tester.AssertAllAsync(default);
    }
}
