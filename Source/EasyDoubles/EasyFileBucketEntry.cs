namespace EasyDoubles;

using Microsoft.IO;

/// <summary>
/// Easy Test Double File Bucket Entry.
/// </summary>
/// <typeparam name="TMetadata">File Meta-data type.</typeparam>
/// <param name="Path">File Path.</param>
/// <param name="Content">File Content.</param>
/// <param name="Metadata">File Metadata.</param>
public record struct EasyFileBucketEntry<TMetadata>(string Path, RecyclableMemoryStream Content, TMetadata Metadata);
