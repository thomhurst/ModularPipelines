using Amazon.S3;
using Amazon.S3.Model;

namespace ModularPipelines.Distributed.Artifacts.S3.Artifacts;

/// <summary>
/// Uploads a forward-only stream of unknown length. Content that fits in one part is sent with a
/// single <c>PutObject</c>; anything larger uses a multipart upload, lifting the 5 GB single-request
/// limit. Bytes are counted as they are read, so the reported size never depends on the source
/// stream being seekable or positioned at zero.
/// </summary>
internal static class S3ObjectUploader
{
    private const int MaximumPartCount = 10_000;

    public static async Task<long> UploadAsync(
        IAmazonS3 s3,
        string bucketName,
        string key,
        Stream content,
        string contentType,
        IReadOnlyDictionary<string, string>? metadata,
        int partSizeBytes,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(content);
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(partSizeBytes);

        var buffer = new byte[partSizeBytes];
        var bytesRead = await ReadFullBufferAsync(content, buffer, cancellationToken).ConfigureAwait(false);
        if (bytesRead < buffer.Length)
        {
            await PutSingleObjectAsync(s3, bucketName, key, buffer, bytesRead, contentType, metadata, cancellationToken)
                .ConfigureAwait(false);
            return bytesRead;
        }

        var initiateRequest = new InitiateMultipartUploadRequest
        {
            BucketName = bucketName,
            Key = key,
            ContentType = contentType,
        };
        AddMetadata(initiateRequest.Metadata, metadata);
        var initiated = await s3.InitiateMultipartUploadAsync(initiateRequest, cancellationToken).ConfigureAwait(false);
        var uploadId = initiated.UploadId;

        try
        {
            var parts = new List<PartETag>();
            var totalBytes = 0L;
            while (bytesRead > 0)
            {
                var partNumber = parts.Count + 1;
                if (partNumber > MaximumPartCount)
                {
                    throw new InvalidOperationException(
                        $"Uploading '{key}' needs more than {MaximumPartCount} parts of {partSizeBytes} bytes. " +
                        $"Increase {nameof(S3StorageOptions)}.{nameof(S3StorageOptions.MultipartPartSizeBytes)}.");
                }

                using var body = new MemoryStream(buffer, 0, bytesRead, writable: false);
                var response = await s3.UploadPartAsync(
                        new UploadPartRequest
                        {
                            BucketName = bucketName,
                            Key = key,
                            UploadId = uploadId,
                            PartNumber = partNumber,
                            PartSize = bytesRead,
                            InputStream = body,
                            DisablePayloadSigning = true,
                        },
                        cancellationToken)
                    .ConfigureAwait(false);

                // Carry any part checksum the SDK computed so the completion request matches it.
                parts.Add(new PartETag(response, true) { PartNumber = partNumber });
                totalBytes += bytesRead;

                if (bytesRead < buffer.Length)
                {
                    break;
                }

                bytesRead = await ReadFullBufferAsync(content, buffer, cancellationToken).ConfigureAwait(false);
            }

            await s3.CompleteMultipartUploadAsync(
                    new CompleteMultipartUploadRequest
                    {
                        BucketName = bucketName,
                        Key = key,
                        UploadId = uploadId,
                        PartETags = parts,
                    },
                    cancellationToken)
                .ConfigureAwait(false);
            return totalBytes;
        }
        catch
        {
            await AbortAsync(s3, bucketName, key, uploadId).ConfigureAwait(false);
            throw;
        }
    }

    private static async Task PutSingleObjectAsync(
        IAmazonS3 s3,
        string bucketName,
        string key,
        byte[] buffer,
        int length,
        string contentType,
        IReadOnlyDictionary<string, string>? metadata,
        CancellationToken cancellationToken)
    {
        using var body = new MemoryStream(buffer, 0, length, writable: false);
        var request = new PutObjectRequest
        {
            BucketName = bucketName,
            Key = key,
            InputStream = body,
            ContentType = contentType,
            DisablePayloadSigning = true,
        };
        AddMetadata(request.Metadata, metadata);
        await s3.PutObjectAsync(request, cancellationToken).ConfigureAwait(false);
    }

    private static async Task AbortAsync(IAmazonS3 s3, string bucketName, string key, string uploadId)
    {
        try
        {
            // Not cancellable: the caller may already be canceled, and the abort is what frees
            // the uploaded parts.
            await s3.AbortMultipartUploadAsync(
                    new AbortMultipartUploadRequest
                    {
                        BucketName = bucketName,
                        Key = key,
                        UploadId = uploadId,
                    },
                    CancellationToken.None)
                .ConfigureAwait(false);
        }
        catch (Exception)
        {
            // Best effort: an abort failure must not replace the upload failure the caller
            // rethrows. Parts left behind are only removed by a bucket lifecycle rule with an
            // AbortIncompleteMultipartUpload clause, which S3StorageOptions.SetLifecycleRule
            // configures for artifact uploads.
        }
    }

    private static void AddMetadata(MetadataCollection target, IReadOnlyDictionary<string, string>? metadata)
    {
        if (metadata is null)
        {
            return;
        }

        foreach (var (name, value) in metadata)
        {
            target.Add(name, value);
        }
    }

    private static async Task<int> ReadFullBufferAsync(Stream stream, byte[] buffer, CancellationToken cancellationToken)
    {
        var totalRead = 0;
        while (totalRead < buffer.Length)
        {
            var read = await stream.ReadAsync(buffer.AsMemory(totalRead), cancellationToken).ConfigureAwait(false);
            if (read == 0)
            {
                break;
            }

            totalRead += read;
        }

        return totalRead;
    }
}
