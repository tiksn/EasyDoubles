namespace EasyDoubles;

using System.Collections.Concurrent;
using DotNext.Collections.Specialized;
using TIKSN.Data;

/// <inheritdoc />
public class EasyStores : IEasyStores
{
    private readonly ConcurrentTypeMap<object> stores = new();

    /// <inheritdoc />
    public IEasyStore<TEntity, TIdentity> Resolve<TEntity, TIdentity>()
        where TEntity : IEntity<TIdentity>
        where TIdentity : IEquatable<TIdentity>
    {
        if (this.stores.TryGetValue<TEntity>(out var existingValue))
        {
            return (IEasyStore<TEntity, TIdentity>)existingValue;
        }

        return (IEasyStore<TEntity, TIdentity>)this.stores.GetOrAdd<TEntity>(
            new EasyStore<TEntity, TIdentity>(
                new ConcurrentDictionary<TIdentity, TEntity>()),
            out _);
    }
}
