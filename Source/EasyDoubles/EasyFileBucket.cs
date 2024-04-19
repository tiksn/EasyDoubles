namespace EasyDoubles;

using System.Collections.Concurrent;
using Microsoft.IO;

/// <inheritdoc />
public class EasyFileBucket<TIdentity, TMetadata> : IEasyFileBucket<TIdentity, TMetadata>
    where TIdentity : IEquatable<TIdentity>
{
    private readonly RecyclableMemoryStreamManager streamManager;

    /// <summary>
    /// Initializes a new instance of the <see cref="EasyFileBucket{TIdentity, TMetadata}"/> class.
    /// </summary>
    /// <param name="bucketName">File Bucket Name.</param>
    /// <param name="streamManager">RecyclableMemoryStreamManager.</param>
    /// <param name="bucketContent">Test Double Bucket Content.</param>
    public EasyFileBucket(
        string bucketName,
        RecyclableMemoryStreamManager streamManager,
        ConcurrentDictionary<TIdentity, EasyFileBucketEntry<TMetadata>> bucketContent)
    {
        this.BucketName = bucketName ?? throw new ArgumentNullException(nameof(bucketName));
        this.streamManager = streamManager ?? throw new ArgumentNullException(nameof(streamManager));
        this.BucketContent = bucketContent ?? throw new ArgumentNullException(nameof(bucketContent));
    }

    /// <inheritdoc />
    public ConcurrentDictionary<TIdentity, EasyFileBucketEntry<TMetadata>> BucketContent { get; }

    /// <inheritdoc />
    public string BucketName { get; }

    /// <inheritdoc/>
    public RecyclableMemoryStream AcquireStream(TIdentity id)
        => this.streamManager.GetStream($"{this.BucketName}|{id}");
}
