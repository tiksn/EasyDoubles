namespace EasyDoubles;

using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TIKSN.Data;

/// <inheritdoc />
public class EasyRepository<TEntity, TIdentity> : IEasyRepository<TEntity, TIdentity>
    where TEntity : IEntity<TIdentity>
    where TIdentity : IEquatable<TIdentity>
{
    private readonly IEasyStore<TEntity, TIdentity> easyStore;

    /// <summary>
    /// Initializes a new instance of the <see cref="EasyRepository{TEntity, TIdentity}"/> class.
    /// </summary>
    /// <param name="easyStores">Easy Test Double Stores.</param>
    public EasyRepository(IEasyStores easyStores)
        => this.easyStore = easyStores?.Resolve<TEntity, TIdentity>() ?? throw new ArgumentNullException(nameof(easyStores));

    /// <inheritdoc/>
    public Task AddAsync(TEntity entity, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var added = this.easyStore.Entities.TryAdd(entity.ID, entity);

        if (added)
        {
            return Task.CompletedTask;
        }

        return Task.FromException(new InvalidOperationException($"Failed to add entity with id {entity.ID}"));
    }

    /// <inheritdoc/>
    public Task AddOrUpdateAsync(TEntity entity, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        _ = this.easyStore.Entities.AddOrUpdate(entity.ID, entity, (_, v) => v);

        return Task.CompletedTask;
    }

    /// <inheritdoc/>
    public Task AddOrUpdateRangeAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        return BatchOperationHelper.BatchOperationAsync(entities, this.AddOrUpdateAsync, cancellationToken);
    }

    /// <inheritdoc/>
    public Task AddRangeAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        return BatchOperationHelper.BatchOperationAsync(entities, this.AddAsync, cancellationToken);
    }

    /// <inheritdoc/>
    public Task<bool> ExistsAsync(TIdentity id, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        return Task.FromResult(this.easyStore.Entities.ContainsKey(id));
    }

    /// <inheritdoc/>
    public Task<TEntity> GetAsync(TIdentity id, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        if (this.easyStore.Entities.TryGetValue(id, out var entity))
        {
            return Task.FromResult(entity);
        }

        return Task.FromException<TEntity>(new EntityNotFoundException());
    }

    /// <inheritdoc/>
    public Task<TEntity?> GetOrDefaultAsync(TIdentity id, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        if (this.easyStore.Entities.TryGetValue(id, out var entity))
        {
            return Task.FromResult<TEntity?>(entity);
        }

        return Task.FromResult<TEntity?>(default);
    }

    /// <inheritdoc/>
    public Task<IReadOnlyList<TEntity>> ListAsync(IEnumerable<TIdentity> ids, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var result = new List<TEntity>();

        foreach (var id in ids)
        {
            if (this.easyStore.Entities.TryGetValue(id, out var entity))
            {
                result.Add(entity);
            }
        }

        return Task.FromResult<IReadOnlyList<TEntity>>(result);
    }

    /// <inheritdoc/>
    public Task<PageResult<TEntity>> PageAsync(PageQuery pageQuery, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        return PaginationQueryableHelper.PageAsync(
            this.easyStore.Entities.Values.AsQueryable().OrderBy(x => x.ID),
            pageQuery,
            static (q, _) => Task.FromResult(q.ToList()),
            static (q, _) => Task.FromResult(q.LongCount()),
            cancellationToken);
    }

    /// <inheritdoc/>
    public Task RemoveAsync(TEntity entity, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        _ = this.easyStore.Entities.TryRemove(entity.ID, out _);

        return Task.CompletedTask;
    }

    /// <inheritdoc/>
    public Task RemoveRangeAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        return BatchOperationHelper.BatchOperationAsync(entities, this.RemoveAsync, cancellationToken);
    }

    /// <inheritdoc/>
    public IAsyncEnumerable<TEntity> StreamAllAsync(CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        return this.easyStore.Entities.Values.ToAsyncEnumerable();
    }

    /// <inheritdoc/>
    public Task UpdateAsync(TEntity entity, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        if (this.easyStore.Entities.TryGetValue(entity.ID, out var oldEntity)
            && this.easyStore.Entities.TryUpdate(entity.ID, entity, oldEntity))
        {
            return Task.CompletedTask;
        }

        return Task.FromException<TEntity>(new EntityNotFoundException());
    }

    /// <inheritdoc/>
    public Task UpdateRangeAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        return BatchOperationHelper.BatchOperationAsync(entities, this.UpdateAsync, cancellationToken);
    }
}
