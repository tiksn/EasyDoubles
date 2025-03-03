namespace EasyDoubles.Test;

using System.Collections.Frozen;
using System.Data.Common;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using EasyDoubles.Test.DependencyInjection;
using EasyDoubles.Test.Entities;
using EasyDoubles.Test.Entities.Configurations;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using TIKSN.Data;
using TIKSN.DependencyInjection;
using Xunit;
using Xunit.Abstractions;

public sealed class MultiDatabaseTester
{
    private readonly (IServiceProvider Easy, IServiceProvider Sqlite) providers
        = (BuildEasyServiceProvider(), BuildSqliteServiceProvider());

    private readonly ITestOutputHelper testOutputHelper;

    public MultiDatabaseTester(ITestOutputHelper testOutputHelper)
        => this.testOutputHelper = testOutputHelper ?? throw new ArgumentNullException(nameof(testOutputHelper));

    public async Task AssertAllAsync(CancellationToken cancellationToken)
    {
        var easyScope = this.providers.Easy.CreateAsyncScope();
        await using (easyScope.ConfigureAwait(false))
        {
            var sqliteScope = this.providers.Sqlite.CreateAsyncScope();
            await using (sqliteScope.ConfigureAwait(false))
            {
                await this.AssertForEntityAsync<CatalogBrand, int>(
                    easyScope.ServiceProvider,
                    sqliteScope.ServiceProvider,
                    cancellationToken).ConfigureAwait(false);
                await this.AssertForEntityAsync<CatalogType, int>(
                    easyScope.ServiceProvider,
                    sqliteScope.ServiceProvider,
                    cancellationToken).ConfigureAwait(false);
                await this.AssertForEntityAsync<CatalogItem, int>(
                    easyScope.ServiceProvider,
                    sqliteScope.ServiceProvider,
                    cancellationToken).ConfigureAwait(false);
            }
        }
    }

    public async Task ForEachAsync(Func<IServiceProvider, Task> body)
    {
        ArgumentNullException.ThrowIfNull(body);

        var easyScope = this.providers.Easy.CreateAsyncScope();
        await using (easyScope.ConfigureAwait(false))
        {
            var sqliteScope = this.providers.Sqlite.CreateAsyncScope();
            await using (sqliteScope.ConfigureAwait(false))
            {
                await Task.WhenAll(body(easyScope.ServiceProvider), body(sqliteScope.ServiceProvider))
                    .ConfigureAwait(false);
            }
        }
    }

    public async Task<TResult> ForEachAsync<TResult, TEntity, TIdentity>(
        Func<IServiceProvider, Task<TResult>> body,
        Func<TResult, IReadOnlyList<TEntity>> map)
        where TEntity : class, IEntity<TIdentity>
        where TIdentity : IEquatable<TIdentity>
    {
        ArgumentNullException.ThrowIfNull(body);

        var easyScope = this.providers.Easy.CreateAsyncScope();
        await using (easyScope.ConfigureAwait(false))
        {
            var sqliteScope = this.providers.Sqlite.CreateAsyncScope();
            await using (sqliteScope.ConfigureAwait(false))
            {
                var easyResult = await body(easyScope.ServiceProvider).ConfigureAwait(false);
                var sqliteResult = await body(sqliteScope.ServiceProvider).ConfigureAwait(false);

                this.AssertForEach<TEntity, TIdentity>(map(easyResult), map(sqliteResult));

                return easyResult;
            }
        }
    }

    public async Task InitializeAsync(CancellationToken cancellationToken)
    {
        var catalogContext = this.providers.Sqlite.GetRequiredService<CatalogContext>();
        _ = await catalogContext.Database.EnsureCreatedAsync(cancellationToken).ConfigureAwait(false);
    }

    private static AutofacServiceProvider BuildEasyServiceProvider()
    {
        var services = new ServiceCollection()
            .AddEasyDoubles()
            .AddFrameworkCore();
        var builder = new ContainerBuilder();
        builder.Populate(services);
        _ = builder.RegisterModule<EasyModule>();
        var container = builder.Build();
        return new AutofacServiceProvider(container);
    }

    private static AutofacServiceProvider BuildSqliteServiceProvider()
    {
        var services = new ServiceCollection()
            .AddSingleton<DbConnection>(_ =>
            {
                var connection = new SqliteConnection("DataSource=:memory:");
                connection.Open();

                return connection;
            }).AddDbContext<CatalogContext>((container, options) =>
            {
                var connection = container.GetRequiredService<DbConnection>();
                _ = options.UseSqlite(connection);
            })
            .AddFrameworkCore()
            .AddScoped<IUnitOfWorkFactory, EntityUnitOfWorkFactory>();
        var builder = new ContainerBuilder();
        builder.Populate(services);
        _ = builder.RegisterModule<EntityModule>();
        var container = builder.Build();
        return new AutofacServiceProvider(container);
    }

    private void AssertForEach<TEntity, TIdentity>(
        IReadOnlyList<TEntity> easyStoreEntities,
        IReadOnlyList<TEntity> dbSetEntities)
        where TEntity : class, IEntity<TIdentity>
        where TIdentity : IEquatable<TIdentity>
    {
        var easyStoreEntityIds = easyStoreEntities.Select(x => x.ID).ToFrozenSet();
        var dbSetEntityIds = dbSetEntities.Select(x => x.ID).ToFrozenSet();
        var allIds = easyStoreEntityIds.Concat(dbSetEntityIds).ToFrozenSet();

        Assert.NotEmpty(easyStoreEntityIds);
        Assert.NotEmpty(dbSetEntityIds);
        Assert.NotEmpty(allIds);

        var edges =
            easyStoreEntityIds.Except(dbSetEntityIds)
                .Concat(
                    dbSetEntityIds.Except(easyStoreEntityIds))
                .ToFrozenSet();

        Assert.Empty(edges);

        foreach (var id in allIds)
        {
            var entityType = typeof(TEntity);
            this.testOutputHelper.WriteLine($"Asserting '{entityType.Name}' [ID {id}]");

            var easyStoreEntity = easyStoreEntities.Single(x => x.ID.Equals(id));
            var dbSetEntity = dbSetEntities.Single(x => x.ID.Equals(id));

            var properties = entityType.GetProperties().ToList();
            properties = [.. properties.Where(x =>
                !properties.Exists(y => string.Equals(y.Name, $"{x.Name}Id", StringComparison.Ordinal)))];
            foreach (var property in properties)
            {
                var easyStoreEntityValue = property.GetValue(easyStoreEntity, index: null);
                var dbSetEntityValue = property.GetValue(dbSetEntity, index: null);

                Assert.Equal(easyStoreEntityValue, dbSetEntityValue);
            }
        }
    }

    private async Task AssertForEntityAsync<TEntity, TIdentity>(
        IServiceProvider easyServiceProvider,
        IServiceProvider sqliteServiceProvider,
        CancellationToken cancellationToken)
        where TEntity : class, IEntity<TIdentity>
        where TIdentity : IEquatable<TIdentity>
    {
        var easyStores = easyServiceProvider.GetRequiredService<IEasyStores>();
        var catalogContext = sqliteServiceProvider.GetRequiredService<CatalogContext>();

        var easyStore = easyStores.Resolve<TEntity, TIdentity>();
        var dbSet = catalogContext.Set<TEntity>();

        var easyStoreEntities = easyStore.Entities.Values.ToList();
        var dbSetEntities = await dbSet.AsNoTracking().ToListAsync(cancellationToken).ConfigureAwait(false);

        this.AssertForEach<TEntity, TIdentity>(easyStoreEntities, dbSetEntities);
    }
}
