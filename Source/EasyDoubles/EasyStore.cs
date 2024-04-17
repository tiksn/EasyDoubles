namespace EasyDoubles;

using System.Collections.Concurrent;
using TIKSN.Data;

/// <inheritdoc />
public class EasyStore<TEntity, TIdentity> : IEasyStore<TEntity, TIdentity>
    where TEntity : IEntity<TIdentity>
    where TIdentity : IEquatable<TIdentity>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="EasyStore{TEntity, TIdentity}"/> class.
    /// </summary>
    /// <param name="entities">Test Double Entities.</param>
    public EasyStore(ConcurrentDictionary<TIdentity, TEntity> entities)
        => this.Entities = entities ?? throw new ArgumentNullException(nameof(entities));

    /// <inheritdoc />
    public ConcurrentDictionary<TIdentity, TEntity> Entities { get; }
}
