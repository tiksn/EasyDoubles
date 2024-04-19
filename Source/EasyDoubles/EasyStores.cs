namespace EasyDoubles;

using System.Collections.Concurrent;
using DotNext.Collections.Specialized;
using Microsoft.IO;
using TIKSN.Data;

/// <inheritdoc />
public class EasyStores : IEasyStores
{
    private readonly ConcurrentDictionary<string, object> buckets = new(StringComparer.OrdinalIgnoreCase);
    private readonly ConcurrentTypeMap<object> stores = new();
    private readonly RecyclableMemoryStreamManager streamManager = new();

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

    /// <inheritdoc />
    public IEasyFileBucket<TIdentity, TMetadata> ResolveBucket<TIdentity, TMetadata>(
        string bucketName)
        where TIdentity : IEquatable<TIdentity>
    {
        if (string.IsNullOrWhiteSpace(bucketName))
        {
            throw new ArgumentException($"'{nameof(bucketName)}' cannot be null or whitespace.", nameof(bucketName));
        }

#pragma warning disable MA0106 // Avoid closure by using an overload with the 'factoryArgument' parameter
        return (IEasyFileBucket<TIdentity, TMetadata>)this.buckets.GetOrAdd(
            bucketName,
            name => new EasyFileBucket<TIdentity, TMetadata>(
                name,
                this.streamManager,
                new ConcurrentDictionary<TIdentity, EasyFileBucketEntry<TMetadata>>()));
#pragma warning restore MA0106 // Avoid closure by using an overload with the 'factoryArgument' parameter
    }
}
