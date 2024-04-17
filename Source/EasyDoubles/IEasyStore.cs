namespace EasyDoubles;

using System.Collections.Concurrent;
using TIKSN.Data;

/// <summary>
/// Easy Test Double Store.
/// </summary>
/// <typeparam name="TEntity">Test Double Entity.</typeparam>
/// <typeparam name="TIdentity">Test Double Entity Identity.</typeparam>
public interface IEasyStore<TEntity, TIdentity>
    where TEntity : IEntity<TIdentity>
    where TIdentity : IEquatable<TIdentity>
{
    /// <summary>
    /// Gets Entities in Store.
    /// </summary>
    ConcurrentDictionary<TIdentity, TEntity> Entities { get; }
}
