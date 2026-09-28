using System.Text.Json;
using Amazon.S3;
using Amazon.S3.Model;

namespace ModularPipelines.Distributed.Artifacts.S3.Artifacts;

/// <summary>
/// S3-compatible implementation of <see cref="IDistributedArtifactStore"/>.
/// Objects are keyed as {prefix}/artifacts/{runId}/{Uri.EscapeDataString(moduleId.Value)}/{artifactName}/{artifactId}.
/// Compatible with AWS S3, Cloudflare R2, Backblaze B2, and MinIO.
/// </summary>
internal sealed class S3DistributedArtifactStore : IDistributedArtifactStore, IDisposable
{
    private readonly IAmazonS3 _s3;
    private readonly string _bucketName;
    private readonly string _runPrefix;
    private readonly int _partSizeBytes;

    public S3DistributedArtifactStore(
        IAmazonS3 s3,
        S3StorageOptions options,
        string runId)
    {
        ArgumentNullException.ThrowIfNull(s3);
        ArgumentNullException.ThrowIfNull(options);
        _s3 = s3;
        _bucketName = options.BucketName;
        _runPrefix = $"{GetArtifactPrefix(options)}{runId}";
        _partSizeBytes = options.MultipartPartSizeBytes;
    }

    /// <summary>
    /// Gets the object key prefix under which every run's artifacts are stored.
    /// </summary>
    internal static string GetArtifactPrefix(S3StorageOptions options) =>
        $"{options.KeyPrefix.Trim('/')}/artifacts/";

    public async Task<ArtifactReference> UploadAsync(ArtifactDescriptor descriptor, Stream data, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(descriptor);
        ArgumentNullException.ThrowIfNull(data);

        var artifactId = Guid.NewGuid().ToString("N");
        var sizeBytes = await S3ObjectUploader.UploadAsync(
                _s3,
                _bucketName,
                BuildObjectKey(descriptor.ModuleId, descriptor.Name, artifactId),
                data,
                descriptor.ContentType ?? "application/octet-stream",
                descriptor.Metadata,
                _partSizeBytes,
                cancellationToken)
            .ConfigureAwait(false);

        var reference = new ArtifactReference(
            ArtifactId: artifactId,
            Name: descriptor.Name,
            ModuleId: descriptor.ModuleId,
            SizeBytes: sizeBytes,
            ContentType: descriptor.ContentType,
            UploadedAt: DateTimeOffset.UtcNow);

        // Store metadata as a separate JSON object for listing
        var metaRequest = new PutObjectRequest
        {
            BucketName = _bucketName,
            Key = BuildMetaKey(descriptor.ModuleId, artifactId),
            ContentBody = JsonSerializer.Serialize(reference),
            ContentType = "application/json",
            DisablePayloadSigning = true,
        };
        await _s3.PutObjectAsync(metaRequest, cancellationToken).ConfigureAwait(false);

        return reference;
    }

    public async Task<Stream> DownloadAsync(ArtifactReference reference, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(reference);

        var objectKey = BuildObjectKey(reference.ModuleId, reference.Name, reference.ArtifactId);
        using var response = await _s3.GetObjectAsync(_bucketName, objectKey, cancellationToken).ConfigureAwait(false);

        // Stream to a temp file instead of MemoryStream to avoid OOM on large artifacts
        var tempFile = Path.GetTempFileName();
        var fileStream = new FileStream(tempFile, FileMode.Create, FileAccess.ReadWrite, FileShare.None,
            bufferSize: 81920, FileOptions.Asynchronous | FileOptions.DeleteOnClose);

        try
        {
            await response.ResponseStream.CopyToAsync(fileStream, cancellationToken).ConfigureAwait(false);
            fileStream.Position = 0;
            return fileStream;
        }
        catch
        {
            await fileStream.DisposeAsync().ConfigureAwait(false);
            throw;
        }
    }

    public async Task<IReadOnlyList<ArtifactReference>> ListArtifactsAsync(ModuleId moduleId, CancellationToken cancellationToken)
    {
        var request = new ListObjectsV2Request
        {
            BucketName = _bucketName,
            Prefix = $"{_runPrefix}/{Uri.EscapeDataString(moduleId.Value)}/meta/",
        };

        var references = new List<ArtifactReference>();
        ListObjectsV2Response response;
        do
        {
            response = await _s3.ListObjectsV2Async(request, cancellationToken).ConfigureAwait(false);

            foreach (var s3Object in response.S3Objects ?? [])
            {
                using var getResponse = await _s3.GetObjectAsync(_bucketName, s3Object.Key, cancellationToken)
                    .ConfigureAwait(false);
                using var reader = new StreamReader(getResponse.ResponseStream);
                var json = await reader.ReadToEndAsync(cancellationToken).ConfigureAwait(false);
                try
                {
                    var reference = JsonSerializer.Deserialize<ArtifactReference>(json);
                    if (reference is not null)
                    {
                        references.Add(reference);
                    }
                }
                catch (JsonException)
                {
                    // Skip malformed metadata objects; storage and access failures propagate.
                }
            }

            request.ContinuationToken = response.NextContinuationToken;
        }
        while (response.IsTruncated == true);

        return references;
    }

    public async Task DeleteAsync(ArtifactReference reference, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(reference);

        var objectKey = BuildObjectKey(reference.ModuleId, reference.Name, reference.ArtifactId);
        var metaKey = BuildMetaKey(reference.ModuleId, reference.ArtifactId);

        await _s3.DeleteObjectAsync(_bucketName, objectKey, cancellationToken).ConfigureAwait(false);
        await _s3.DeleteObjectAsync(_bucketName, metaKey, cancellationToken).ConfigureAwait(false);
    }

    public void Dispose() => _s3.Dispose();

    private string BuildObjectKey(ModuleId moduleId, string artifactName, string artifactId)
        => $"{_runPrefix}/{Uri.EscapeDataString(moduleId.Value)}/{artifactName}/{artifactId}";

    private string BuildMetaKey(ModuleId moduleId, string artifactId)
        => $"{_runPrefix}/{Uri.EscapeDataString(moduleId.Value)}/meta/{artifactId}.json";
}
