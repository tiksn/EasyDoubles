namespace EasyDoubles;

using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using LanguageExt;
using TIKSN.Data;
using TIKSN.Identity;

/// <inheritdoc />
public class EasyFileRepository<TIdentity, TMetadata> : IEasyFileRepository<TIdentity, TMetadata>
    where TIdentity : IEquatable<TIdentity>
{
    private readonly IEasyFileBucket<TIdentity, TMetadata> easyFileBucket;
    private readonly IIdentityGenerator<TIdentity> identityGenerator;

    /// <summary>
    /// Initializes a new instance of the <see cref="EasyFileRepository{TIdentity, TMetadata}"/> class.
    /// </summary>
    /// <param name="easyStores">Easy Test Double Stores.</param>
    /// <param name="identityGenerator">Identity Generator.</param>
    /// <param name="bucketName">File Bucket Name.</param>
    public EasyFileRepository(
        IEasyStores easyStores,
        IIdentityGenerator<TIdentity> identityGenerator,
        string bucketName)
    {
        this.easyFileBucket = easyStores?.ResolveBucket<TIdentity, TMetadata>(bucketName) ?? throw new ArgumentNullException(nameof(easyStores));
        this.identityGenerator = identityGenerator ?? throw new ArgumentNullException(nameof(identityGenerator));
    }

    /// <inheritdoc />
    public Task DeleteAsync(
        string path,
        CancellationToken cancellationToken)
        => this.QueryByPath(
            path,
            () => throw new InvalidOperationException("File not found."),
            x => this.DeleteByIdAsync(x.Key, cancellationToken),
            (_, _) => throw new InvalidOperationException("More than one file were found."));

    /// <inheritdoc />
    public async Task DeleteByIdAsync(
        TIdentity id,
        CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        if (this.easyFileBucket.BucketContent.TryRemove(id, out var content))
        {
            await content.Content.DisposeAsync().ConfigureAwait(false);
        }
    }

    /// <inheritdoc />
    public async Task<IFile> DownloadAsync(
        string path,
        CancellationToken cancellationToken)
        => await this.QueryByPath(
            path,
            () => throw new InvalidOperationException("File not found."),
            x => this.DownloadByIdAsync(x.Key, cancellationToken),
            (_, _) => throw new InvalidOperationException("More than one file were found."))
        .ConfigureAwait(false);

    /// <inheritdoc />
    public Task<IFile<TIdentity>> DownloadByIdAsync(
        TIdentity id,
        CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        if (this.easyFileBucket.BucketContent.TryGetValue(id, out var content))
        {
            _ = content.Content.Seek(0, SeekOrigin.Begin);
            var bytes = new byte[content.Content.Length];
            _ = content.Content.Read(bytes);
            var file = new File<TIdentity>(id, content.Path, bytes);
            return Task.FromResult<IFile<TIdentity>>(file);
        }

        throw new InvalidOperationException("Failed to find the file.");
    }

    /// <inheritdoc />
    public Task<IFileInfo<TIdentity, TMetadata>> DownloadOnlyMetadataAsync(
        TIdentity id,
        CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        if (this.easyFileBucket.BucketContent.TryGetValue(id, out var content))
        {
            var fileInfo = new FileInfo<TIdentity, TMetadata>(id, content.Path, content.Metadata);
            return Task.FromResult<IFileInfo<TIdentity, TMetadata>>(fileInfo);
        }

        throw new InvalidOperationException("Failed to find the file.");
    }

    /// <inheritdoc />
    public Task<IFile<TIdentity, TMetadata>> DownloadWithMetadataAsync(
        TIdentity id,
        CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        if (this.easyFileBucket.BucketContent.TryGetValue(id, out var content))
        {
            _ = content.Content.Seek(0, SeekOrigin.Begin);
            var bytes = new byte[content.Content.Length];
            _ = content.Content.Read(bytes);
            var file = new File<TIdentity, TMetadata>(id, content.Path, content.Metadata, bytes);
            return Task.FromResult<IFile<TIdentity, TMetadata>>(file);
        }

        throw new InvalidOperationException("Failed to find the file.");
    }

    /// <inheritdoc />
    public Task<bool> ExistsAsync(
        string path,
        CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var exists = this.QueryByPath(
            path,
            () => false,
            _ => true,
            (_, _) => true);
        return Task.FromResult(exists);
    }

    /// <inheritdoc />
    public Task<bool> ExistsByIdAsync(
        TIdentity id,
        CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        return Task.FromResult(this.easyFileBucket.BucketContent.TryGetValue(id, out _));
    }

    /// <inheritdoc />
    public Task UploadAsync(
        string path,
        byte[] content,
        CancellationToken cancellationToken)
        => this.QueryByPath(
            path,
            () => this.UploadByIdAsync(this.identityGenerator.Generate(), path, content, cancellationToken),
            x => this.UploadByIdAsync(x.Key, path, content, cancellationToken),
            (_, _) => throw new InvalidOperationException("More than one file were found."));

    /// <inheritdoc />
    public async Task UploadAsync(
        TIdentity id,
        string path,
        byte[] content,
        TMetadata metadata,
        CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

#pragma warning disable MA0106 // Avoid closure by using an overload with the 'factoryArgument' parameter
        var bucketEntry = this.easyFileBucket.BucketContent.AddOrUpdate(
            id,
            xId => new EasyFileBucketEntry<TMetadata>(path, this.easyFileBucket.AcquireStream(xId), metadata),
            (_, entry) =>
            {
                AssertPathIsTheSame(entry, path);
                entry.Metadata = metadata;

                return entry;
            });
#pragma warning restore MA0106 // Avoid closure by using an overload with the 'factoryArgument' parameter

        await bucketEntry.Content.WriteAsync(content, cancellationToken).ConfigureAwait(false);
    }

    /// <inheritdoc />
    public async Task UploadByIdAsync(
        TIdentity id,
        string path,
        byte[] content,
        CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

#pragma warning disable MA0106 // Avoid closure by using an overload with the 'factoryArgument' parameter
#pragma warning disable CS8604 // Possible null reference argument.
        var bucketEntry = this.easyFileBucket.BucketContent.AddOrUpdate(
            id,
            xId => new EasyFileBucketEntry<TMetadata>(path, this.easyFileBucket.AcquireStream(xId), default),
            (_, entry) =>
            {
                AssertPathIsTheSame(entry, path);

                return entry;
            });
#pragma warning restore CS8604 // Possible null reference argument.
#pragma warning restore MA0106 // Avoid closure by using an overload with the 'factoryArgument' parameter

        await bucketEntry.Content.WriteAsync(content, cancellationToken).ConfigureAwait(false);
    }

    private static void AssertPathIsTheSame(
        EasyFileBucketEntry<TMetadata> entry,
        string path)
    {
        if (!string.Equals(entry.Path, path, StringComparison.Ordinal))
        {
            throw new InvalidOperationException("Path should not be changed");
        }
    }

    private TResult QueryByPath<TResult>(
        string path,
        Func<TResult> onEmpty,
        Func<KeyValuePair<TIdentity, EasyFileBucketEntry<TMetadata>>, TResult> onOne,
        Func<KeyValuePair<TIdentity, EasyFileBucketEntry<TMetadata>>, Seq<KeyValuePair<TIdentity, EasyFileBucketEntry<TMetadata>>>, TResult> onMore)
        => this.easyFileBucket.BucketContent
        .Where(x => string.Equals(x.Value.Path, path, StringComparison.Ordinal))
        .Match(onEmpty, onOne, onMore);
}
