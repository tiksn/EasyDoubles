namespace EasyDoubles;

using TIKSN.Data;

/// <summary>
/// Easy Test Double File Repository.
/// </summary>
/// <typeparam name="TIdentity">File Identity type.</typeparam>
/// <typeparam name="TMetadata">File Meta-data type.</typeparam>
public interface IEasyFileRepository<TIdentity, TMetadata> :
    IFileRepository,
    IFileRepository<TIdentity>,
    IFileRepository<TIdentity, TMetadata>
    where TIdentity : IEquatable<TIdentity>;
