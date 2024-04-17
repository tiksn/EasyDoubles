namespace EasyDoubles;

using TIKSN.Data;

/// <summary>
/// Easy Test Double Stores.
/// </summary>
public interface IEasyStores
{
    /// <summary>
    /// Adds or Gets Easy Double Store.
    /// </summary>
    /// <returns>Returns Easy Double Store.</returns>
    /// <typeparam name="TEntity">Test Double Entity.</typeparam>
    /// <typeparam name="TIdentity">Test Double Entity Identity.</typeparam>
    IEasyStore<TEntity, TIdentity> Resolve<TEntity, TIdentity>()
        where TEntity : IEntity<TIdentity>
        where TIdentity : IEquatable<TIdentity>;
}
