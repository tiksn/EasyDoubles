namespace EasyDoubles;

using TIKSN.Data;
using TIKSN.Data.LiteDB;
using TIKSN.Data.Mongo;
using TIKSN.Data.RavenDB;

/// <summary>
/// Easy Test Double Repository.
/// </summary>
/// <typeparam name="TEntity">Test Double Entity.</typeparam>
/// <typeparam name="TIdentity">Test Double Entity Identity.</typeparam>
#pragma warning disable S3444 // Interfaces should not simply inherit from base interfaces with colliding members

public interface IEasyRepository<TEntity, TIdentity> :
#pragma warning restore S3444 // Interfaces should not simply inherit from base interfaces with colliding members
    ILiteDbRepository<TEntity, TIdentity>,
    IMongoRepository<TEntity, TIdentity>,
    IRavenRepository<TEntity, TIdentity>
    where TEntity : IEntity<TIdentity>
    where TIdentity : IEquatable<TIdentity>;
