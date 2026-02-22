using System.Text.Json;
using Amazon.S3;
using Amazon.S3.Model;

namespace ModularPipelines.Distributed.Artifacts.S3.Artifacts;

/// <summary>
/// S3-compatible implementation of <see cref="IDistributedArtifactStore"/>.
/// Objects are keyed as {prefix}/{runId}/{moduleType}/{artifactName}.
/// Compatible with AWS S3, Cloudflare R2, Backblaze B2, and MinIO.
/// </summary>
internal sealed class S3DistributedArtifactStore : IDistributedArtifactStore
{
    private readonly IAmazonS3 _s3;
    private readonly string _bucketName;
    private readonly string _keyPrefix;
    private readonly string _runId;
    private readonly int _ttlSeconds;

    public S3DistributedArtifactStore(
        IAmazonS3 s3,
        string bucketName,
        string keyPrefix,
        string runId,
        int ttlSeconds)
    {
        _s3 = s3;
        _bucketName = bucketName;
        _keyPrefix = keyPrefix;
        _runId = runId;
        _ttlSeconds = ttlSeconds;
    }

    public async Task<ArtifactReference> UploadAsync(ArtifactDescriptor descriptor, Stream data, CancellationToken cancellationToken)
    {
        var artifactId = Guid.NewGuid().ToString("N");
        var objectKey = BuildObjectKey(descriptor.ModuleTypeName, descriptor.Name, artifactId);
        var expiresAt = DateTimeOffset.UtcNow.AddSeconds(_ttlSeconds);

        var request = new PutObjectRequest
        {
            BucketName = _bucketName,
            Key = objectKey,
            InputStream = data,
            ContentType = descriptor.ContentType ?? "application/octet-stream",
            TagSet =
            [
                new Tag { Key = "expires-at", Value = expiresAt.ToUnixTimeSeconds().ToString() },
                new Tag { Key = "artifact-name", Value = descriptor.Name },
                new Tag { Key = "module-type", Value = descriptor.ModuleTypeName },
            ],
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
            ModuleTypeName: descriptor.ModuleTypeName,
            SizeBytes: sizeBytes,
            ContentType: descriptor.ContentType,
            UploadedAt: DateTimeOffset.UtcNow);

        // Store metadata as a separate JSON object for listing
        var metaKey = BuildMetaKey(descriptor.ModuleTypeName, artifactId);
        var metaJson = JsonSerializer.Serialize(reference);
        var metaRequest = new PutObjectRequest
        {
            BucketName = _bucketName,
            Key = metaKey,
            ContentBody = metaJson,
            ContentType = "application/json",
        };
        await _s3.PutObjectAsync(metaRequest, cancellationToken);

        return reference;
    }

    public async Task<Stream> DownloadAsync(ArtifactReference reference, CancellationToken cancellationToken)
    {
        var objectKey = BuildObjectKey(reference.ModuleTypeName, reference.Name, reference.ArtifactId);
        var response = await _s3.GetObjectAsync(_bucketName, objectKey, cancellationToken);

        // Copy to MemoryStream so the S3 response stream isn't held open
        var ms = new MemoryStream();
        await response.ResponseStream.CopyToAsync(ms, cancellationToken);
        ms.Position = 0;
        return ms;
    }

    public async Task<IReadOnlyList<ArtifactReference>> ListArtifactsAsync(string moduleTypeName, CancellationToken cancellationToken)
    {
        var prefix = $"{_keyPrefix}/{_runId}/{moduleTypeName}/meta/";
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

            foreach (var s3Object in response.S3Objects)
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
                catch
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
        var objectKey = BuildObjectKey(reference.ModuleTypeName, reference.Name, reference.ArtifactId);
        var metaKey = BuildMetaKey(reference.ModuleTypeName, reference.ArtifactId);

        await _s3.DeleteObjectAsync(_bucketName, objectKey, cancellationToken);
        await _s3.DeleteObjectAsync(_bucketName, metaKey, cancellationToken);
    }

    private string BuildObjectKey(string moduleTypeName, string artifactName, string artifactId)
        => $"{_keyPrefix}/{_runId}/{moduleTypeName}/{artifactName}/{artifactId}";

    private string BuildMetaKey(string moduleTypeName, string artifactId)
        => $"{_keyPrefix}/{_runId}/{moduleTypeName}/meta/{artifactId}.json";
}
