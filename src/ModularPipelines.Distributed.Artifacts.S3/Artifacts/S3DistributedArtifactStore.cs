using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using Amazon.S3;
using Amazon.S3.Model;
using Microsoft.Extensions.Logging;

namespace ModularPipelines.Distributed.Artifacts.S3.Artifacts;

/// <summary>
/// S3-compatible implementation of <see cref="IDistributedArtifactStore"/>.
/// Payloads are keyed as {prefix}/artifacts/{runId}/{escapedModuleId}/data/{escapedArtifactName}/{artifactId}.
/// Names that exceed the remaining key budget use a bounded SHA256 segment instead.
/// Metadata is stored under the same module prefix in meta/{artifactId}.json, separate from payloads.
/// Compatible with AWS S3, Cloudflare R2, Backblaze B2, and MinIO. Run cleanup deletes every object under
/// the run's prefix.
/// </summary>
internal sealed class S3DistributedArtifactStore : IDistributedArtifactStore, IDistributedRunCleanup, IDisposable
{
    private readonly IAmazonS3 _s3;
    private readonly string _bucketName;
    private readonly string _runPrefix;
    private readonly int _partSizeBytes;
    private readonly ILogger _logger;

    public S3DistributedArtifactStore(
        IAmazonS3 s3,
        S3StorageOptions options,
        string runId,
        ILogger logger)
    {
        ArgumentNullException.ThrowIfNull(s3);
        ArgumentNullException.ThrowIfNull(options);
        ArgumentNullException.ThrowIfNull(logger);
        _s3 = s3;
        _bucketName = options.BucketName;
        _runPrefix = $"{GetArtifactPrefix(options)}{runId}";
        _partSizeBytes = options.MultipartPartSizeBytes;
        _logger = logger;
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

        var reference = new ArtifactReference
        {
            ArtifactId = artifactId,
            Name = descriptor.Name,
            ModuleId = descriptor.ModuleId,
            SizeBytes = sizeBytes,
            ContentType = descriptor.ContentType,
            UploadedAt = DateTimeOffset.UtcNow,
        };

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

    /// <summary>
    /// Deletes every object stored for the run, including artifacts uploaded by other processes. In a
    /// versioned bucket every version and delete marker is deleted when version listing is available.
    /// Otherwise cleanup deletes only current objects, leaving old versions and delete markers behind.
    /// </summary>
    public async Task CleanupRunAsync(DistributedRunCleanupContext context, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(context);

        // The trailing separator keeps a run from matching another run whose identifier it prefixes.
        var prefix = $"{_runPrefix}/";
        await DeleteVersionsAsync(prefix, cancellationToken).ConfigureAwait(false);
    }

    private async Task DeleteVersionsAsync(string prefix, CancellationToken cancellationToken)
    {
        var request = new ListVersionsRequest
        {
            BucketName = _bucketName,
            Prefix = prefix,
        };

        ListVersionsResponse response;
        do
        {
            try
            {
                response = await _s3.ListVersionsAsync(request, cancellationToken).ConfigureAwait(false);
            }
            catch (AmazonS3Exception exception) when (exception.StatusCode is
                HttpStatusCode.BadRequest or HttpStatusCode.Forbidden or
                HttpStatusCode.MethodNotAllowed or HttpStatusCode.NotImplemented)
            {
                if (_logger.IsEnabled(LogLevel.Information))
                {
                    _logger.LogInformation(
                        "S3 version listing for {Prefix} in bucket {BucketName} returned HTTP {StatusCode}; " +
                        "falling back to current-object cleanup. In a versioned bucket, old versions and delete markers remain, " +
                        "and deleting current objects only adds delete markers.",
                        prefix,
                        _bucketName,
                        (int) exception.StatusCode);
                }

                await DeleteObjectsAsync(prefix, cancellationToken).ConfigureAwait(false);
                return;
            }

            await DeleteBatchAsync(
                    [.. (response.Versions ?? []).Select(static version => new KeyVersion
                    {
                        Key = version.Key,
                        VersionId = version.VersionId,
                    })],
                    cancellationToken)
                .ConfigureAwait(false);

            request.KeyMarker = response.NextKeyMarker;
            request.VersionIdMarker = response.NextVersionIdMarker;
        }
        while (response.IsTruncated == true);
    }

    private async Task DeleteObjectsAsync(string prefix, CancellationToken cancellationToken)
    {
        var request = new ListObjectsV2Request
        {
            BucketName = _bucketName,
            Prefix = prefix,
        };

        ListObjectsV2Response response;
        do
        {
            response = await _s3.ListObjectsV2Async(request, cancellationToken).ConfigureAwait(false);
            await DeleteBatchAsync(
                    [.. (response.S3Objects ?? []).Select(static s3Object => new KeyVersion { Key = s3Object.Key })],
                    cancellationToken)
                .ConfigureAwait(false);

            request.ContinuationToken = response.NextContinuationToken;
        }
        while (response.IsTruncated == true);
    }

    // A listing page holds at most 1,000 entries, the DeleteObjects limit.
    private async Task DeleteBatchAsync(List<KeyVersion> objects, CancellationToken cancellationToken)
    {
        if (objects.Count == 0)
        {
            return;
        }

        var response = await _s3.DeleteObjectsAsync(
                new DeleteObjectsRequest
                {
                    BucketName = _bucketName,
                    Objects = objects,
                    Quiet = true,
                },
                cancellationToken)
            .ConfigureAwait(false);

        // The AWS client throws this itself; check too, so a client that only reports per-key errors cannot
        // make cleanup look complete while objects remain.
        if (response?.DeleteErrors is { Count: > 0 })
        {
            throw new DeleteObjectsException(response);
        }
    }

    public void Dispose() => _s3.Dispose();

    private string BuildObjectKey(ModuleId moduleId, string artifactName, string artifactId)
    {
        const int maxKeyBytes = 1024;
        var prefix = $"{_runPrefix}/{Uri.EscapeDataString(moduleId.Value)}/data/";
        var nameBudget = maxKeyBytes - Encoding.UTF8.GetByteCount($"{prefix}/{artifactId}");
        // The metadata key needs four more bytes than a payload key with an empty name.
        if (nameBudget < 4)
        {
            throw new InvalidOperationException("The S3 artifact key prefix and identifier leave insufficient space within the 1,024-byte key limit.");
        }

        var nameSegment = Uri.EscapeDataString(artifactName);
        if (nameSegment.Length > nameBudget)
        {
            var hash = Convert.ToHexString(SHA256.HashData(Encoding.UTF8.GetBytes(artifactName)));
            // The artifact ID provides uniqueness even when the name hash must be shortened.
            nameSegment = hash[..Math.Min(hash.Length, nameBudget)];
        }

        return $"{prefix}{nameSegment}/{artifactId}";
    }

    private string BuildMetaKey(ModuleId moduleId, string artifactId)
        => $"{_runPrefix}/{Uri.EscapeDataString(moduleId.Value)}/meta/{artifactId}.json";
}
