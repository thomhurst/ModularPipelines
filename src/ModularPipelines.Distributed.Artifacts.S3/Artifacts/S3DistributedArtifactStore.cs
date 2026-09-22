using System.Text.Json;
using Amazon.S3;
using Amazon.S3.Model;

namespace ModularPipelines.Distributed.Artifacts.S3.Artifacts;

/// <summary>
/// S3-compatible implementation of <see cref="IDistributedArtifactStore"/>.
/// Objects are keyed as {prefix}/{runId}/{Uri.EscapeDataString(moduleId.Value)}/{artifactName}.
/// Compatible with AWS S3, Cloudflare R2, Backblaze B2, and MinIO.
/// </summary>
internal sealed class S3DistributedArtifactStore : IDistributedArtifactStore, IDisposable
{
    private readonly IAmazonS3 _s3;
    private readonly string _bucketName;
    private readonly string _keyPrefix;
    private readonly string _runId;

    public S3DistributedArtifactStore(
        IAmazonS3 s3,
        string bucketName,
        string keyPrefix,
        string runId)
    {
        _s3 = s3;
        _bucketName = bucketName;
        _keyPrefix = keyPrefix;
        _runId = runId;
    }

    public async Task<ArtifactReference> UploadAsync(ArtifactDescriptor descriptor, Stream data, CancellationToken cancellationToken)
    {
        var artifactId = Guid.NewGuid().ToString("N");
        var objectKey = BuildObjectKey(descriptor.ModuleId, descriptor.Name, artifactId);

        var request = new PutObjectRequest
        {
            BucketName = _bucketName,
            Key = objectKey,
            InputStream = data,
            ContentType = descriptor.ContentType ?? "application/octet-stream",
            DisablePayloadSigning = true,
        };

        if (descriptor.Metadata is not null)
        {
            foreach (var kvp in descriptor.Metadata)
            {
                request.Metadata.Add(kvp.Key, kvp.Value);
            }
        }

        // Capture size before upload (stream may be consumed)
        var sizeBytes = data.CanSeek ? data.Length : 0;

        await _s3.PutObjectAsync(request, cancellationToken);

        // If we couldn't get size before, try position after
        if (sizeBytes == 0 && data.CanSeek)
        {
            sizeBytes = data.Position;
        }

        var reference = new ArtifactReference(
            ArtifactId: artifactId,
            Name: descriptor.Name,
            ModuleId: descriptor.ModuleId,
            SizeBytes: sizeBytes,
            ContentType: descriptor.ContentType,
            UploadedAt: DateTimeOffset.UtcNow);

        // Store metadata as a separate JSON object for listing
        var metaKey = BuildMetaKey(descriptor.ModuleId, artifactId);
        var metaJson = JsonSerializer.Serialize(reference);
        var metaRequest = new PutObjectRequest
        {
            BucketName = _bucketName,
            Key = metaKey,
            ContentBody = metaJson,
            ContentType = "application/json",
            DisablePayloadSigning = true,
        };
        await _s3.PutObjectAsync(metaRequest, cancellationToken);

        return reference;
    }

    public async Task<Stream> DownloadAsync(ArtifactReference reference, CancellationToken cancellationToken)
    {
        var objectKey = BuildObjectKey(reference.ModuleId, reference.Name, reference.ArtifactId);
        var response = await _s3.GetObjectAsync(_bucketName, objectKey, cancellationToken);

        // Stream to a temp file instead of MemoryStream to avoid OOM on large artifacts
        var tempFile = Path.GetTempFileName();
        var fileStream = new FileStream(tempFile, FileMode.Create, FileAccess.ReadWrite, FileShare.None,
            bufferSize: 81920, FileOptions.DeleteOnClose);

        try
        {
            await response.ResponseStream.CopyToAsync(fileStream, cancellationToken);
            fileStream.Position = 0;
            return fileStream;
        }
        catch
        {
            await fileStream.DisposeAsync();
            throw;
        }
    }

    public async Task<IReadOnlyList<ArtifactReference>> ListArtifactsAsync(ModuleId moduleId, CancellationToken cancellationToken)
    {
        var prefix = $"{_keyPrefix}/{_runId}/{Uri.EscapeDataString(moduleId.Value)}/meta/";
        var request = new ListObjectsV2Request
        {
            BucketName = _bucketName,
            Prefix = prefix,
        };

        var references = new List<ArtifactReference>();
        ListObjectsV2Response response;
        do
        {
            response = await _s3.ListObjectsV2Async(request, cancellationToken);

            foreach (var s3Object in response.S3Objects ?? [])
            {
                try
                {
                    var getResponse = await _s3.GetObjectAsync(_bucketName, s3Object.Key, cancellationToken);
                    using var reader = new StreamReader(getResponse.ResponseStream);
                    var json = await reader.ReadToEndAsync(cancellationToken);
                    var reference = JsonSerializer.Deserialize<ArtifactReference>(json);
                    if (reference is not null)
                    {
                        references.Add(reference);
                    }
                }
                catch (Exception ex) when (ex is not OperationCanceledException)
                {
                    // Skip invalid metadata objects
                }
            }

            request.ContinuationToken = response.NextContinuationToken;
        }
        while (response.IsTruncated == true);

        return references;
    }

    public async Task DeleteAsync(ArtifactReference reference, CancellationToken cancellationToken)
    {
        var objectKey = BuildObjectKey(reference.ModuleId, reference.Name, reference.ArtifactId);
        var metaKey = BuildMetaKey(reference.ModuleId, reference.ArtifactId);

        await _s3.DeleteObjectAsync(_bucketName, objectKey, cancellationToken);
        await _s3.DeleteObjectAsync(_bucketName, metaKey, cancellationToken);
    }

    public void Dispose() => _s3.Dispose();

    private string BuildObjectKey(ModuleId moduleId, string artifactName, string artifactId)
        => $"{_keyPrefix}/{_runId}/{Uri.EscapeDataString(moduleId.Value)}/{artifactName}/{artifactId}";

    private string BuildMetaKey(ModuleId moduleId, string artifactId)
        => $"{_keyPrefix}/{_runId}/{Uri.EscapeDataString(moduleId.Value)}/meta/{artifactId}.json";
}
