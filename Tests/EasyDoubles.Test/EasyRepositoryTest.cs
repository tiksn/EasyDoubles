namespace EasyDoubles.Test;

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
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

    [Fact]
    public async Task GivenInitializedDatabases_WhenPerCollectionMultipleEntitiesCreated_ThenAllMatch()
    {
        // Arrange
        var tester = new MultiDatabaseTester(this.testOutputHelper);
        await tester.InitializeAsync(default);
        await tester.ForEachAsync(provider =>
            provider.GetRequiredService<ICatalogInitializer>().InitializeAsync(default));

        // Act
        static async Task CreateMultipleEntitiesAsync(IServiceProvider provider)
        {
            var unitOfWorkFactory = provider.GetRequiredService<IUnitOfWorkFactory>();
            var unitOfWork = await unitOfWorkFactory.CreateAsync(default).ConfigureAwait(false);
            await using (unitOfWork.ConfigureAwait(false))
            {
                var type101Entity = new CatalogType()
                {
                    ID = 10101,
                    Type = "Footwear",
                };
                var type102Entity = new CatalogType()
                {
                    ID = 10102,
                    Type = "Climbing",
                };
                await provider.GetRequiredService<ICatalogTypeRepository>().AddRangeAsync([type101Entity, type102Entity], default).ConfigureAwait(false);

                var brand101Entity = new CatalogBrand()
                {
                    ID = 1101,
                    Brand = "Daybird",
                };
                var brand102Entity = new CatalogBrand()
                {
                    ID = 1102,
                    Brand = "Gravitator",
                };
                await provider.GetRequiredService<ICatalogBrandRepository>().AddRangeAsync([brand101Entity, brand102Entity], default).ConfigureAwait(false);

                var item101Entity = new CatalogItem()
                {
                    ID = 100101,
                    CatalogType = type101Entity,
                    CatalogTypeId = type101Entity.ID,
                    CatalogBrand = brand101Entity,
                    CatalogBrandId = brand101Entity.ID,
                    Name = "Wanderer Black Hiking Boots",
                    Description = "Daybird's Wanderer Hiking Boots in sleek black are perfect for all your outdoor adventures. These boots are made with a waterproof leather upper and a durable rubber sole for superior traction. With their cushioned insole and padded collar, these boots will keep you comfortable all day long.",
                    Price = 109.99m,
                };
                var item102Entity = new CatalogItem()
                {
                    ID = 100102,
                    CatalogType = type102Entity,
                    CatalogTypeId = type102Entity.ID,
                    CatalogBrand = brand102Entity,
                    CatalogBrandId = brand102Entity.ID,
                    Name = "Summit Pro Harness",
                    Description = "Conquer new heights with the Summit Pro Harness by Gravitator. This lightweight and durable climbing harness features adjustable leg loops and waist belt for a customized fit. With its vibrant blue color, you'll look stylish while maneuvering difficult routes. Safety is a top priority with a reinforced tie-in point and strong webbing loops.",
                    Price = 89.99m,
                };
                await provider.GetRequiredService<ICatalogItemRepository>().AddRangeAsync([item101Entity, item102Entity], default).ConfigureAwait(false);

                await unitOfWork.CompleteAsync(default).ConfigureAwait(false);
            }
        }

        await tester.ForEachAsync(CreateMultipleEntitiesAsync);

        // Assert
        await tester.AssertAllAsync(default);
    }

    [Fact]
    public async Task GivenInitializedDatabases_WhenPerCollectionSingleEntitiesUpdated_ThenAllMatch()
    {
        // Arrange
        var tester = new MultiDatabaseTester(this.testOutputHelper);
        await tester.InitializeAsync(default);
        await tester.ForEachAsync(provider =>
            provider.GetRequiredService<ICatalogInitializer>().InitializeAsync(default));

        // Act
        static async Task UpdateSingleEntitiesAsync(IServiceProvider provider)
        {
            var unitOfWorkFactory = provider.GetRequiredService<IUnitOfWorkFactory>();
            var unitOfWork = await unitOfWorkFactory.CreateAsync(default).ConfigureAwait(false);
            await using (unitOfWork.ConfigureAwait(false))
            {
                var typeEntity = new CatalogType()
                {
                    ID = 10001,
                    Type = "Footwear",
                };
                await provider.GetRequiredService<ICatalogTypeRepository>().UpdateAsync(typeEntity, default).ConfigureAwait(false);

                var brandEntity = new CatalogBrand()
                {
                    ID = 1001,
                    Brand = "Daybird",
                };
                await provider.GetRequiredService<ICatalogBrandRepository>().UpdateAsync(brandEntity, default).ConfigureAwait(false);

                var itemEntity = new CatalogItem()
                {
                    ID = 100001,
                    CatalogType = typeEntity,
                    CatalogTypeId = typeEntity.ID,
                    CatalogBrand = brandEntity,
                    CatalogBrandId = brandEntity.ID,
                    Name = "Wanderer Black Hiking Boots",
                    Description = "Daybird's Wanderer Hiking Boots in sleek black are perfect for all your outdoor adventures. These boots are made with a waterproof leather upper and a durable rubber sole for superior traction. With their cushioned insole and padded collar, these boots will keep you comfortable all day long.",
                    Price = 109.99m,
                };
                await provider.GetRequiredService<ICatalogItemRepository>().UpdateAsync(itemEntity, default).ConfigureAwait(false);

                await unitOfWork.CompleteAsync(default).ConfigureAwait(false);
            }
        }

        await tester.ForEachAsync(UpdateSingleEntitiesAsync);

        // Assert
        await tester.AssertAllAsync(default);
    }

    [Fact]
    public async Task GivenInitializedDatabases_WhenPerCollectionMultipleEntitiesUpdated_ThenAllMatch()
    {
        // Arrange
        var tester = new MultiDatabaseTester(this.testOutputHelper);
        await tester.InitializeAsync(default);
        await tester.ForEachAsync(provider =>
            provider.GetRequiredService<ICatalogInitializer>().InitializeAsync(default));

        // Act
        static async Task UpdateMultipleEntitiesAsync(IServiceProvider provider)
        {
            var unitOfWorkFactory = provider.GetRequiredService<IUnitOfWorkFactory>();
            var unitOfWork = await unitOfWorkFactory.CreateAsync(default).ConfigureAwait(false);
            await using (unitOfWork.ConfigureAwait(false))
            {
                var type001Entity = new CatalogType()
                {
                    ID = 10001,
                    Type = "Footwear",
                };
                var type002Entity = new CatalogType()
                {
                    ID = 10002,
                    Type = "Climbing",
                };
                await provider.GetRequiredService<ICatalogTypeRepository>().UpdateRangeAsync([type001Entity, type002Entity], default).ConfigureAwait(false);

                var brand001Entity = new CatalogBrand()
                {
                    ID = 1001,
                    Brand = "Daybird",
                };
                var brand002Entity = new CatalogBrand()
                {
                    ID = 1002,
                    Brand = "Gravitator",
                };
                await provider.GetRequiredService<ICatalogBrandRepository>().UpdateRangeAsync([brand001Entity, brand002Entity], default).ConfigureAwait(false);

                var item001Entity = new CatalogItem()
                {
                    ID = 100001,
                    CatalogType = type001Entity,
                    CatalogTypeId = type001Entity.ID,
                    CatalogBrand = brand001Entity,
                    CatalogBrandId = brand001Entity.ID,
                    Name = "Wanderer Black Hiking Boots",
                    Description = "Daybird's Wanderer Hiking Boots in sleek black are perfect for all your outdoor adventures. These boots are made with a waterproof leather upper and a durable rubber sole for superior traction. With their cushioned insole and padded collar, these boots will keep you comfortable all day long.",
                    Price = 109.99m,
                };
                var item002Entity = new CatalogItem()
                {
                    ID = 100002,
                    CatalogType = type002Entity,
                    CatalogTypeId = type002Entity.ID,
                    CatalogBrand = brand002Entity,
                    CatalogBrandId = brand002Entity.ID,
                    Name = "Summit Pro Harness",
                    Description = "Conquer new heights with the Summit Pro Harness by Gravitator. This lightweight and durable climbing harness features adjustable leg loops and waist belt for a customized fit. With its vibrant blue color, you'll look stylish while maneuvering difficult routes. Safety is a top priority with a reinforced tie-in point and strong webbing loops.",
                    Price = 89.99m,
                };
                await provider.GetRequiredService<ICatalogItemRepository>().UpdateRangeAsync([item001Entity, item002Entity], default).ConfigureAwait(false);

                await unitOfWork.CompleteAsync(default).ConfigureAwait(false);
            }
        }

        await tester.ForEachAsync(UpdateMultipleEntitiesAsync);

        // Assert
        await tester.AssertAllAsync(default);
    }

    [Fact]
    public async Task GivenInitializedDatabases_WhenPerCollectionSingleEntitiesDeleted_ThenAllMatch()
    {
        // Arrange
        var tester = new MultiDatabaseTester(this.testOutputHelper);
        await tester.InitializeAsync(default);
        await tester.ForEachAsync(provider =>
            provider.GetRequiredService<ICatalogInitializer>().InitializeAsync(default));

        // Act
        static async Task DeleteSingleEntitiesAsync(IServiceProvider provider)
        {
            var unitOfWorkFactory = provider.GetRequiredService<IUnitOfWorkFactory>();
            var unitOfWork = await unitOfWorkFactory.CreateAsync(default).ConfigureAwait(false);
            await using (unitOfWork.ConfigureAwait(false))
            {
                var itemEntity = await provider.GetRequiredService<ICatalogItemQueryRepository>().GetAsync(100001, default).ConfigureAwait(false);
                await provider.GetRequiredService<ICatalogItemRepository>().RemoveAsync(itemEntity, default).ConfigureAwait(false);

                var typeEntity = await provider.GetRequiredService<ICatalogTypeQueryRepository>().GetAsync(10001, default).ConfigureAwait(false);
                await provider.GetRequiredService<ICatalogTypeRepository>().RemoveAsync(typeEntity, default).ConfigureAwait(false);

                var brandEntity = await provider.GetRequiredService<ICatalogBrandQueryRepository>().GetAsync(1001, default).ConfigureAwait(false);
                await provider.GetRequiredService<ICatalogBrandRepository>().RemoveAsync(brandEntity, default).ConfigureAwait(false);

                await unitOfWork.CompleteAsync(default).ConfigureAwait(false);
            }
        }

        await tester.ForEachAsync(DeleteSingleEntitiesAsync);

        // Assert
        await tester.AssertAllAsync(default);
    }

    [Fact]
    public async Task GivenInitializedDatabases_WhenPerCollectionMultipleEntitiesDeleted_ThenAllMatch()
    {
        // Arrange
        var tester = new MultiDatabaseTester(this.testOutputHelper);
        await tester.InitializeAsync(default);
        await tester.ForEachAsync(provider =>
            provider.GetRequiredService<ICatalogInitializer>().InitializeAsync(default));

        // Act
        static async Task DeleteMultipleEntitiesAsync(IServiceProvider provider)
        {
            var unitOfWorkFactory = provider.GetRequiredService<IUnitOfWorkFactory>();
            var unitOfWork = await unitOfWorkFactory.CreateAsync(default).ConfigureAwait(false);
            await using (unitOfWork.ConfigureAwait(false))
            {
                var item001Entity = await provider.GetRequiredService<ICatalogItemQueryRepository>().GetAsync(100001, default).ConfigureAwait(false);
                var item002Entity = await provider.GetRequiredService<ICatalogItemQueryRepository>().GetAsync(100002, default).ConfigureAwait(false);
                await provider.GetRequiredService<ICatalogItemRepository>().RemoveRangeAsync([item001Entity, item002Entity], default).ConfigureAwait(false);

                var type001Entity = await provider.GetRequiredService<ICatalogTypeQueryRepository>().GetAsync(10001, default).ConfigureAwait(false);
                var type002Entity = await provider.GetRequiredService<ICatalogTypeQueryRepository>().GetAsync(10002, default).ConfigureAwait(false);
                await provider.GetRequiredService<ICatalogTypeRepository>().RemoveRangeAsync([type001Entity, type002Entity], default).ConfigureAwait(false);

                var brand001Entity = await provider.GetRequiredService<ICatalogBrandQueryRepository>().GetAsync(1001, default).ConfigureAwait(false);
                var brand002Entity = await provider.GetRequiredService<ICatalogBrandQueryRepository>().GetAsync(1002, default).ConfigureAwait(false);
                await provider.GetRequiredService<ICatalogBrandRepository>().RemoveRangeAsync([brand001Entity, brand002Entity], default).ConfigureAwait(false);

                await unitOfWork.CompleteAsync(default).ConfigureAwait(false);
            }
        }

        await tester.ForEachAsync(DeleteMultipleEntitiesAsync);

        // Assert
        await tester.AssertAllAsync(default);
    }

    [Fact]
    public async Task GivenInitializedDatabases_WhenPerCollectionRetrievedById_ThenAllMatch()
    {
        // Arrange
        var tester = new MultiDatabaseTester(this.testOutputHelper);
        await tester.InitializeAsync(default);
        await tester.ForEachAsync(provider =>
            provider.GetRequiredService<ICatalogInitializer>().InitializeAsync(default));

        // Act
        static async Task GetByIdSingleEntitiesAsync(IServiceProvider provider)
        {
            var unitOfWorkFactory = provider.GetRequiredService<IUnitOfWorkFactory>();
            var unitOfWork = await unitOfWorkFactory.CreateAsync(default).ConfigureAwait(false);
            await using (unitOfWork.ConfigureAwait(false))
            {
                var itemEntity = await provider.GetRequiredService<ICatalogItemQueryRepository>().GetAsync(100001, default).ConfigureAwait(false);
                Assert.Equal(100001, itemEntity.ID);
                Assert.Equal(10001, itemEntity.CatalogTypeId);
                Assert.Equal(1001, itemEntity.CatalogBrandId);

                var typeEntity = await provider.GetRequiredService<ICatalogTypeQueryRepository>().GetAsync(10001, default).ConfigureAwait(false);
                Assert.Equal(10001, typeEntity.ID);

                var brandEntity = await provider.GetRequiredService<ICatalogBrandQueryRepository>().GetAsync(1001, default).ConfigureAwait(false);
                Assert.Equal(1001, brandEntity.ID);

                await unitOfWork.CompleteAsync(default).ConfigureAwait(false);
            }
        }

        await tester.ForEachAsync(GetByIdSingleEntitiesAsync);

        // Assert
        await tester.AssertAllAsync(default);
    }

    [Fact]
    public async Task GivenInitializedDatabases_WhenPerCollectionExistingRetrievedByIdOrDefault_ThenResultMatch()
    {
        // Arrange
        var tester = new MultiDatabaseTester(this.testOutputHelper);
        await tester.InitializeAsync(default);
        await tester.ForEachAsync(provider =>
            provider.GetRequiredService<ICatalogInitializer>().InitializeAsync(default));

        // Act
        static async Task GetByIdSingleEntitiesAsync(IServiceProvider provider)
        {
            var unitOfWorkFactory = provider.GetRequiredService<IUnitOfWorkFactory>();
            var unitOfWork = await unitOfWorkFactory.CreateAsync(default).ConfigureAwait(false);
            await using (unitOfWork.ConfigureAwait(false))
            {
                var itemEntity = await provider.GetRequiredService<ICatalogItemQueryRepository>().GetOrDefaultAsync(100001, default).ConfigureAwait(false);
                Assert.NotNull(itemEntity);
                Assert.Equal(100001, itemEntity.ID);
                Assert.Equal(10001, itemEntity.CatalogTypeId);
                Assert.Equal(1001, itemEntity.CatalogBrandId);

                var typeEntity = await provider.GetRequiredService<ICatalogTypeQueryRepository>().GetOrDefaultAsync(10001, default).ConfigureAwait(false);
                Assert.NotNull(typeEntity);
                Assert.Equal(10001, typeEntity.ID);

                var brandEntity = await provider.GetRequiredService<ICatalogBrandQueryRepository>().GetOrDefaultAsync(1001, default).ConfigureAwait(false);
                Assert.NotNull(brandEntity);
                Assert.Equal(1001, brandEntity.ID);

                await unitOfWork.CompleteAsync(default).ConfigureAwait(false);
            }
        }

        await tester.ForEachAsync(GetByIdSingleEntitiesAsync);

        // Assert
        await tester.AssertAllAsync(default);
    }

    [Fact]
    public async Task GivenInitializedDatabases_WhenPerCollectionMissingRetrievedByIdOrDefault_ThenResultIsNull()
    {
        // Arrange
        var tester = new MultiDatabaseTester(this.testOutputHelper);
        await tester.InitializeAsync(default);
        await tester.ForEachAsync(provider =>
            provider.GetRequiredService<ICatalogInitializer>().InitializeAsync(default));

        // Act
        static async Task GetByIdSingleEntitiesAsync(IServiceProvider provider)
        {
            var unitOfWorkFactory = provider.GetRequiredService<IUnitOfWorkFactory>();
            var unitOfWork = await unitOfWorkFactory.CreateAsync(default).ConfigureAwait(false);
            await using (unitOfWork.ConfigureAwait(false))
            {
                var itemEntity = await provider.GetRequiredService<ICatalogItemQueryRepository>().GetOrDefaultAsync(100101, default).ConfigureAwait(false);
                Assert.Null(itemEntity);

                var typeEntity = await provider.GetRequiredService<ICatalogTypeQueryRepository>().GetOrDefaultAsync(10101, default).ConfigureAwait(false);
                Assert.Null(typeEntity);

                var brandEntity = await provider.GetRequiredService<ICatalogBrandQueryRepository>().GetOrDefaultAsync(1101, default).ConfigureAwait(false);
                Assert.Null(brandEntity);

                await unitOfWork.CompleteAsync(default).ConfigureAwait(false);
            }
        }

        await tester.ForEachAsync(GetByIdSingleEntitiesAsync);

        // Assert
        await tester.AssertAllAsync(default);
    }

    [Fact]
    public async Task GivenInitializedDatabases_WhenPerCollectionExistingEntityExistsChecked_ThenItShouldBeTrue()
    {
        // Arrange
        var tester = new MultiDatabaseTester(this.testOutputHelper);
        await tester.InitializeAsync(default);
        await tester.ForEachAsync(provider =>
            provider.GetRequiredService<ICatalogInitializer>().InitializeAsync(default));

        // Act
        static async Task GetByIdSingleEntitiesAsync(IServiceProvider provider)
        {
            var unitOfWorkFactory = provider.GetRequiredService<IUnitOfWorkFactory>();
            var unitOfWork = await unitOfWorkFactory.CreateAsync(default).ConfigureAwait(false);
            await using (unitOfWork.ConfigureAwait(false))
            {
                var itemEntityExists = await provider.GetRequiredService<ICatalogItemQueryRepository>().ExistsAsync(100001, default).ConfigureAwait(false);
                Assert.True(itemEntityExists);

                var typeEntityExists = await provider.GetRequiredService<ICatalogTypeQueryRepository>().ExistsAsync(10001, default).ConfigureAwait(false);
                Assert.True(typeEntityExists);

                var brandEntityExists = await provider.GetRequiredService<ICatalogBrandQueryRepository>().ExistsAsync(1001, default).ConfigureAwait(false);
                Assert.True(brandEntityExists);

                await unitOfWork.CompleteAsync(default).ConfigureAwait(false);
            }
        }

        await tester.ForEachAsync(GetByIdSingleEntitiesAsync);

        // Assert
        await tester.AssertAllAsync(default);
    }

    [Fact]
    public async Task GivenInitializedDatabases_WhenPerCollectionMissingEntityExistsChecked_ThenItShouldBeFalse()
    {
        // Arrange
        var tester = new MultiDatabaseTester(this.testOutputHelper);
        await tester.InitializeAsync(default);
        await tester.ForEachAsync(provider =>
            provider.GetRequiredService<ICatalogInitializer>().InitializeAsync(default));

        // Act
        static async Task GetByIdSingleEntitiesAsync(IServiceProvider provider)
        {
            var unitOfWorkFactory = provider.GetRequiredService<IUnitOfWorkFactory>();
            var unitOfWork = await unitOfWorkFactory.CreateAsync(default).ConfigureAwait(false);
            await using (unitOfWork.ConfigureAwait(false))
            {
                var itemEntityExists = await provider.GetRequiredService<ICatalogItemQueryRepository>().ExistsAsync(100101, default).ConfigureAwait(false);
                Assert.False(itemEntityExists);

                var typeEntityExists = await provider.GetRequiredService<ICatalogTypeQueryRepository>().ExistsAsync(10101, default).ConfigureAwait(false);
                Assert.False(typeEntityExists);

                var brandEntityExists = await provider.GetRequiredService<ICatalogBrandQueryRepository>().ExistsAsync(1101, default).ConfigureAwait(false);
                Assert.False(brandEntityExists);

                await unitOfWork.CompleteAsync(default).ConfigureAwait(false);
            }
        }

        await tester.ForEachAsync(GetByIdSingleEntitiesAsync);

        // Assert
        await tester.AssertAllAsync(default);
    }

    [Fact]
    public async Task GivenInitializedDatabases_WhenPerCollectionAllExistingEntitiesListed_ThenListShouldHaveRequestedNumberOfItems()
    {
        // Arrange
        var tester = new MultiDatabaseTester(this.testOutputHelper);
        await tester.InitializeAsync(default);
        await tester.ForEachAsync(provider =>
            provider.GetRequiredService<ICatalogInitializer>().InitializeAsync(default));

        // Act
        static async Task<IReadOnlyList<CatalogItem>> ListItemEntitiesAsync(IServiceProvider provider)
        {
            var unitOfWorkFactory = provider.GetRequiredService<IUnitOfWorkFactory>();
            var unitOfWork = await unitOfWorkFactory.CreateAsync(default).ConfigureAwait(false);
            await using (unitOfWork.ConfigureAwait(false))
            {
                var itemEntities = await provider.GetRequiredService<ICatalogItemQueryRepository>().ListAsync([100001, 100002, 100003], default).ConfigureAwait(false);

                await unitOfWork.CompleteAsync(default).ConfigureAwait(false);

                return itemEntities;
            }
        }

        static async Task<IReadOnlyList<CatalogType>> ListTypeEntitiesAsync(IServiceProvider provider)
        {
            var unitOfWorkFactory = provider.GetRequiredService<IUnitOfWorkFactory>();
            var unitOfWork = await unitOfWorkFactory.CreateAsync(default).ConfigureAwait(false);
            await using (unitOfWork.ConfigureAwait(false))
            {
                var typeEntities = await provider.GetRequiredService<ICatalogTypeQueryRepository>().ListAsync([10001, 10002, 10003], default).ConfigureAwait(false);

                await unitOfWork.CompleteAsync(default).ConfigureAwait(false);

                return typeEntities;
            }
        }

        static async Task<IReadOnlyList<CatalogBrand>> ListBrandEntitiesAsync(IServiceProvider provider)
        {
            var unitOfWorkFactory = provider.GetRequiredService<IUnitOfWorkFactory>();
            var unitOfWork = await unitOfWorkFactory.CreateAsync(default).ConfigureAwait(false);
            await using (unitOfWork.ConfigureAwait(false))
            {
                var brandEntities = await provider.GetRequiredService<ICatalogBrandQueryRepository>().ListAsync([1001, 1002, 1003], default).ConfigureAwait(false);

                await unitOfWork.CompleteAsync(default).ConfigureAwait(false);

                return brandEntities;
            }
        }

        var itemEntities = await tester.ForEachAsync<IReadOnlyList<CatalogItem>, CatalogItem, int>(ListItemEntitiesAsync, x => x);
        var typeEntities = await tester.ForEachAsync<IReadOnlyList<CatalogType>, CatalogType, int>(ListTypeEntitiesAsync, x => x);
        var brandEntities = await tester.ForEachAsync<IReadOnlyList<CatalogBrand>, CatalogBrand, int>(ListBrandEntitiesAsync, x => x);

        // Assert
        await tester.AssertAllAsync(default);
        Assert.Equal(3, itemEntities.Count);
        Assert.Equal(3, typeEntities.Count);
        Assert.Equal(3, brandEntities.Count);
    }

    [Fact]
    public async Task GivenInitializedDatabases_WhenPerCollectionSomeExistingEntitiesListed_ThenListShouldBeLessThanRequestedNumberOfItems()
    {
        // Arrange
        var tester = new MultiDatabaseTester(this.testOutputHelper);
        await tester.InitializeAsync(default);
        await tester.ForEachAsync(provider =>
            provider.GetRequiredService<ICatalogInitializer>().InitializeAsync(default));

        // Act
        static async Task<IReadOnlyList<CatalogItem>> ListItemEntitiesAsync(IServiceProvider provider)
        {
            var unitOfWorkFactory = provider.GetRequiredService<IUnitOfWorkFactory>();
            var unitOfWork = await unitOfWorkFactory.CreateAsync(default).ConfigureAwait(false);
            await using (unitOfWork.ConfigureAwait(false))
            {
                var itemEntities = await provider.GetRequiredService<ICatalogItemQueryRepository>().ListAsync([100001, 100002, 100003, 100099], default).ConfigureAwait(false);

                await unitOfWork.CompleteAsync(default).ConfigureAwait(false);

                return itemEntities;
            }
        }

        static async Task<IReadOnlyList<CatalogType>> ListTypeEntitiesAsync(IServiceProvider provider)
        {
            var unitOfWorkFactory = provider.GetRequiredService<IUnitOfWorkFactory>();
            var unitOfWork = await unitOfWorkFactory.CreateAsync(default).ConfigureAwait(false);
            await using (unitOfWork.ConfigureAwait(false))
            {
                var typeEntities = await provider.GetRequiredService<ICatalogTypeQueryRepository>().ListAsync([10001, 10002, 10003, 10099], default).ConfigureAwait(false);

                await unitOfWork.CompleteAsync(default).ConfigureAwait(false);

                return typeEntities;
            }
        }

        static async Task<IReadOnlyList<CatalogBrand>> ListBrandEntitiesAsync(IServiceProvider provider)
        {
            var unitOfWorkFactory = provider.GetRequiredService<IUnitOfWorkFactory>();
            var unitOfWork = await unitOfWorkFactory.CreateAsync(default).ConfigureAwait(false);
            await using (unitOfWork.ConfigureAwait(false))
            {
                var brandEntities = await provider.GetRequiredService<ICatalogBrandQueryRepository>().ListAsync([1001, 1002, 1003, 1099], default).ConfigureAwait(false);

                await unitOfWork.CompleteAsync(default).ConfigureAwait(false);

                return brandEntities;
            }
        }

        var itemEntities = await tester.ForEachAsync<IReadOnlyList<CatalogItem>, CatalogItem, int>(ListItemEntitiesAsync, x => x);
        var typeEntities = await tester.ForEachAsync<IReadOnlyList<CatalogType>, CatalogType, int>(ListTypeEntitiesAsync, x => x);
        var brandEntities = await tester.ForEachAsync<IReadOnlyList<CatalogBrand>, CatalogBrand, int>(ListBrandEntitiesAsync, x => x);

        // Assert
        await tester.AssertAllAsync(default);
        Assert.Equal(3, itemEntities.Count);
        Assert.Equal(3, typeEntities.Count);
        Assert.Equal(3, brandEntities.Count);
    }

    [Fact]
    public async Task GivenInitializedDatabases_WhenPerCollectionFirstPageRequested_ThenItemsShouldHaveRequestedNumberOfItems()
    {
        // Arrange
        var firstPage = new Page(1, 3);
        var tester = new MultiDatabaseTester(this.testOutputHelper);
        await tester.InitializeAsync(default);
        await tester.ForEachAsync(provider =>
            provider.GetRequiredService<ICatalogInitializer>().InitializeAsync(default));

        // Act
        async Task<PageResult<CatalogItem>> PageItemEntitiesAsync(IServiceProvider provider)
        {
            var unitOfWorkFactory = provider.GetRequiredService<IUnitOfWorkFactory>();
            var unitOfWork = await unitOfWorkFactory.CreateAsync(default).ConfigureAwait(false);
            await using (unitOfWork.ConfigureAwait(false))
            {
                var itemEntities = await provider.GetRequiredService<ICatalogItemQueryRepository>().PageAsync(new PageQuery(firstPage, estimateTotalItems: true), default).ConfigureAwait(false);

                await unitOfWork.CompleteAsync(default).ConfigureAwait(false);

                return itemEntities;
            }
        }

        async Task<PageResult<CatalogType>> PageTypeEntitiesAsync(IServiceProvider provider)
        {
            var unitOfWorkFactory = provider.GetRequiredService<IUnitOfWorkFactory>();
            var unitOfWork = await unitOfWorkFactory.CreateAsync(default).ConfigureAwait(false);
            await using (unitOfWork.ConfigureAwait(false))
            {
                var typeEntities = await provider.GetRequiredService<ICatalogTypeQueryRepository>().PageAsync(new PageQuery(firstPage, estimateTotalItems: true), default).ConfigureAwait(false);

                await unitOfWork.CompleteAsync(default).ConfigureAwait(false);

                return typeEntities;
            }
        }

        async Task<PageResult<CatalogBrand>> PageBrandEntitiesAsync(IServiceProvider provider)
        {
            var unitOfWorkFactory = provider.GetRequiredService<IUnitOfWorkFactory>();
            var unitOfWork = await unitOfWorkFactory.CreateAsync(default).ConfigureAwait(false);
            await using (unitOfWork.ConfigureAwait(false))
            {
                var brandEntities = await provider.GetRequiredService<ICatalogBrandQueryRepository>().PageAsync(new PageQuery(firstPage, estimateTotalItems: true), default).ConfigureAwait(false);

                await unitOfWork.CompleteAsync(default).ConfigureAwait(false);

                return brandEntities;
            }
        }

        var itemEntities = await tester.ForEachAsync<PageResult<CatalogItem>, CatalogItem, int>(PageItemEntitiesAsync, x => x.Items);
        var typeEntities = await tester.ForEachAsync<PageResult<CatalogType>, CatalogType, int>(PageTypeEntitiesAsync, x => x.Items);
        var brandEntities = await tester.ForEachAsync<PageResult<CatalogBrand>, CatalogBrand, int>(PageBrandEntitiesAsync, x => x.Items);

        // Assert
        await tester.AssertAllAsync(default);
        Assert.Equal(3, itemEntities.Items.Count);
        Assert.Equal(10, itemEntities.TotalItems);
        Assert.Equal(4, itemEntities.TotalPages);
        Assert.Equal(3, typeEntities.Items.Count);
        Assert.Equal(10, typeEntities.TotalItems);
        Assert.Equal(4, typeEntities.TotalPages);
        Assert.Equal(3, brandEntities.Items.Count);
        Assert.Equal(10, brandEntities.TotalItems);
        Assert.Equal(4, brandEntities.TotalPages);
    }

    [Fact]
    public async Task GivenInitializedDatabases_WhenPerCollectionLastPageRequested_ThenItemsShouldHaveLastItems()
    {
        // Arrange
        var lastPage = new Page(4, 3);
        var tester = new MultiDatabaseTester(this.testOutputHelper);
        await tester.InitializeAsync(default);
        await tester.ForEachAsync(provider =>
            provider.GetRequiredService<ICatalogInitializer>().InitializeAsync(default));

        // Act
        async Task<PageResult<CatalogItem>> PageItemEntitiesAsync(IServiceProvider provider)
        {
            var unitOfWorkFactory = provider.GetRequiredService<IUnitOfWorkFactory>();
            var unitOfWork = await unitOfWorkFactory.CreateAsync(default).ConfigureAwait(false);
            await using (unitOfWork.ConfigureAwait(false))
            {
                var itemEntities = await provider.GetRequiredService<ICatalogItemQueryRepository>().PageAsync(new PageQuery(lastPage, estimateTotalItems: true), default).ConfigureAwait(false);

                await unitOfWork.CompleteAsync(default).ConfigureAwait(false);

                return itemEntities;
            }
        }

        async Task<PageResult<CatalogType>> PageTypeEntitiesAsync(IServiceProvider provider)
        {
            var unitOfWorkFactory = provider.GetRequiredService<IUnitOfWorkFactory>();
            var unitOfWork = await unitOfWorkFactory.CreateAsync(default).ConfigureAwait(false);
            await using (unitOfWork.ConfigureAwait(false))
            {
                var typeEntities = await provider.GetRequiredService<ICatalogTypeQueryRepository>().PageAsync(new PageQuery(lastPage, estimateTotalItems: true), default).ConfigureAwait(false);

                await unitOfWork.CompleteAsync(default).ConfigureAwait(false);

                return typeEntities;
            }
        }

        async Task<PageResult<CatalogBrand>> PageBrandEntitiesAsync(IServiceProvider provider)
        {
            var unitOfWorkFactory = provider.GetRequiredService<IUnitOfWorkFactory>();
            var unitOfWork = await unitOfWorkFactory.CreateAsync(default).ConfigureAwait(false);
            await using (unitOfWork.ConfigureAwait(false))
            {
                var brandEntities = await provider.GetRequiredService<ICatalogBrandQueryRepository>().PageAsync(new PageQuery(lastPage, estimateTotalItems: true), default).ConfigureAwait(false);

                await unitOfWork.CompleteAsync(default).ConfigureAwait(false);

                return brandEntities;
            }
        }

        var itemEntities = await tester.ForEachAsync<PageResult<CatalogItem>, CatalogItem, int>(PageItemEntitiesAsync, x => x.Items);
        var typeEntities = await tester.ForEachAsync<PageResult<CatalogType>, CatalogType, int>(PageTypeEntitiesAsync, x => x.Items);
        var brandEntities = await tester.ForEachAsync<PageResult<CatalogBrand>, CatalogBrand, int>(PageBrandEntitiesAsync, x => x.Items);

        // Assert
        await tester.AssertAllAsync(default);
        Assert.Single(itemEntities.Items);
        Assert.Equal(10, itemEntities.TotalItems);
        Assert.Equal(4, itemEntities.TotalPages);
        Assert.Single(typeEntities.Items);
        Assert.Equal(10, typeEntities.TotalItems);
        Assert.Equal(4, typeEntities.TotalPages);
        Assert.Single(brandEntities.Items);
        Assert.Equal(10, brandEntities.TotalItems);
        Assert.Equal(4, brandEntities.TotalPages);
    }
}
