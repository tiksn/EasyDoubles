namespace EasyDoubles.Test.Services;

using System.Globalization;
using EasyDoubles.Test.Entities;
using EasyDoubles.Test.Repositories;
using Microsoft.Extensions.DependencyInjection;
using TIKSN.Data;

public class CatalogInitializer : ICatalogInitializer
{
    private readonly IUnitOfWorkFactory unitOfWorkFactory;

    public CatalogInitializer(
        IUnitOfWorkFactory unitOfWorkFactory)
        => this.unitOfWorkFactory = unitOfWorkFactory ?? throw new ArgumentNullException(nameof(unitOfWorkFactory));

    public async Task InitializeAsync(CancellationToken cancellationToken)
    {
        var unitOfWork = await this.unitOfWorkFactory.CreateAsync(cancellationToken).ConfigureAwait(false);
        await using (unitOfWork.ConfigureAwait(false))
        {
            await InitializeAsync(unitOfWork.Services, cancellationToken).ConfigureAwait(false);
            await unitOfWork.CompleteAsync(cancellationToken).ConfigureAwait(false);
        }
    }

    private static async Task InitializeAsync(IServiceProvider serviceProvider, CancellationToken cancellationToken)
    {
        var random = serviceProvider.GetRequiredService<Random>();

        var catalogBrands = await CreateEntitiesAsync(
            serviceProvider.GetRequiredService<ICatalogBrandRepository>(),
            CreateCatalogBrand,
            cancellationToken).ConfigureAwait(false);

        var catalogTypes = await CreateEntitiesAsync(
            serviceProvider.GetRequiredService<ICatalogTypeRepository>(),
            CreateCatalogType,
            cancellationToken).ConfigureAwait(false);

        _ = await CreateEntitiesAsync(
            serviceProvider.GetRequiredService<ICatalogItemRepository>(),
            x => CreateCatalogItem(
                x,
                catalogBrands[random.Next(catalogBrands.Length)],
                catalogTypes[random.Next(catalogTypes.Length)]),
            cancellationToken).ConfigureAwait(false);
    }

    private static async Task<TEntity[]> CreateEntitiesAsync<TEntity, TRepository>(
        TRepository repository,
        Func<int, TEntity> createEntity,
        CancellationToken cancellationToken)
        where TRepository : IRepository<TEntity>
    {
        ArgumentNullException.ThrowIfNull(repository);
        ArgumentNullException.ThrowIfNull(createEntity);

        var entities = Enumerable
            .Range(1, 10)
            .Select(createEntity)
            .ToArray();

        await repository.AddRangeAsync(entities, cancellationToken).ConfigureAwait(false);

        return entities;
    }

    private static CatalogItem CreateCatalogItem(int number, CatalogBrand catalogBrand, CatalogType catalogType) =>
        new()
        {
            ID = 100_000 + number,
            Name = $"Item Name #{number.ToString(CultureInfo.InvariantCulture)}",
            Description = $"Item Description #{number.ToString(CultureInfo.InvariantCulture)}",
            CatalogBrand = catalogBrand,
            CatalogBrandId = catalogBrand.ID,
            CatalogType = catalogType,
            CatalogTypeId = catalogType.ID,
            AvailableStock = number,
            RestockThreshold = 5,
            OnReorder = number <= 3,
            MaxStockThreshold = 10,
            PictureFileName = $"Picture{number.ToString(CultureInfo.InvariantCulture)}.png",
            Price = number * 10m,
        };

    private static CatalogType CreateCatalogType(int number) => new()
    {
        ID = 10_000 + number,
        Type = $"Type #{number.ToString(CultureInfo.InvariantCulture)}",
    };

    private static CatalogBrand CreateCatalogBrand(int number) => new()
    {
        ID = 1_000 + number,
        Brand = $"Brand #{number.ToString(CultureInfo.InvariantCulture)}",
    };
}
