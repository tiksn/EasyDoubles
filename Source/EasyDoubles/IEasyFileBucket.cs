namespace EasyDoubles;

using System.Collections.Concurrent;
using Microsoft.IO;

/// <summary>
/// Easy Test Double File Bucket.
/// </summary>
/// <typeparam name="TIdentity">File Identity type.</typeparam>
/// <typeparam name="TMetadata">File Meta-data type.</typeparam>
public interface IEasyFileBucket<TIdentity, TMetadata>
    where TIdentity : IEquatable<TIdentity>
{
    /// <summary>
    /// Gets File Bucket Content.
    /// </summary>
    ConcurrentDictionary<TIdentity, EasyFileBucketEntry<TMetadata>> BucketContent { get; }

    /// <summary>
    /// Gets File Bucket Name.
    /// </summary>
    string BucketName { get; }

    /// <summary>
    /// Gets Recyclable Memory Stream for file.
    /// </summary>
    /// <param name="id">File ID.</param>
    /// <returns>Returns Recyclable Memory Stream for file.</returns>
    RecyclableMemoryStream AcquireStream(TIdentity id);
}
