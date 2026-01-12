using System.Diagnostics.CodeAnalysis;
using Amazon.S3;
using Amazon.S3.Model;

namespace ModularPipelines.AmazonWebServices;

/// <summary>
/// Provides a high-level wrapper for interacting with Amazon S3.
/// </summary>
/// <remarks>
/// This class encapsulates the AWS S3 SDK client and provides wrapper methods
/// for common S3 operations including bucket management, object operations, and
/// configuration settings.
/// </remarks>
[ExcludeFromCodeCoverage]
public class S3 : IS3, IDisposable
{
    private readonly AmazonS3Client _client;
    private bool _disposed;

    /// <summary>
    /// Initializes a new instance of the <see cref="S3"/> class.
    /// </summary>
    /// <param name="client">The Amazon S3 client to wrap.</param>
    public S3(AmazonS3Client client)
    {
        _client = client;
    }

    #region Bucket Operations

    /// <inheritdoc />
    public async Task<PutBucketResponse> CreateBucketAsync(PutBucketRequest request, CancellationToken cancellationToken = default)
    {
        return await _client.PutBucketAsync(request, cancellationToken).ConfigureAwait(false);
    }

    /// <inheritdoc />
    public async Task<DeleteBucketResponse> DeleteBucketAsync(string bucketName, CancellationToken cancellationToken = default)
    {
        return await _client.DeleteBucketAsync(bucketName, cancellationToken).ConfigureAwait(false);
    }

    /// <inheritdoc />
    public async Task<DeleteBucketResponse> DeleteBucketAsync(DeleteBucketRequest request, CancellationToken cancellationToken = default)
    {
        return await _client.DeleteBucketAsync(request, cancellationToken).ConfigureAwait(false);
    }

    /// <inheritdoc />
    public async Task<ListBucketsResponse> ListBucketsAsync(CancellationToken cancellationToken = default)
    {
        return await _client.ListBucketsAsync(cancellationToken).ConfigureAwait(false);
    }

    /// <inheritdoc />
    public async Task<bool> DoesBucketExistAsync(string bucketName, CancellationToken cancellationToken = default)
    {
        try
        {
            await _client.GetBucketLocationAsync(bucketName, cancellationToken).ConfigureAwait(false);
            return true;
        }
        catch (AmazonS3Exception ex) when (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
        {
            return false;
        }
    }

    #endregion

    #region Object Operations

    /// <inheritdoc />
    public async Task<PutObjectResponse> UploadObjectAsync(PutObjectRequest request, CancellationToken cancellationToken = default)
    {
        return await _client.PutObjectAsync(request, cancellationToken).ConfigureAwait(false);
    }

    /// <inheritdoc />
    public async Task<PutObjectResponse> UploadFileAsync(string bucketName, string key, string filePath, CancellationToken cancellationToken = default)
    {
        var request = new PutObjectRequest
        {
            BucketName = bucketName,
            Key = key,
            FilePath = filePath,
        };
        return await _client.PutObjectAsync(request, cancellationToken).ConfigureAwait(false);
    }

    /// <inheritdoc />
    public async Task<PutObjectResponse> UploadStreamAsync(string bucketName, string key, Stream inputStream, CancellationToken cancellationToken = default)
    {
        var request = new PutObjectRequest
        {
            BucketName = bucketName,
            Key = key,
            InputStream = inputStream,
        };
        return await _client.PutObjectAsync(request, cancellationToken).ConfigureAwait(false);
    }

    /// <inheritdoc />
    public async Task<GetObjectResponse> DownloadObjectAsync(GetObjectRequest request, CancellationToken cancellationToken = default)
    {
        return await _client.GetObjectAsync(request, cancellationToken).ConfigureAwait(false);
    }

    /// <inheritdoc />
    public async Task<GetObjectResponse> DownloadObjectAsync(string bucketName, string key, CancellationToken cancellationToken = default)
    {
        return await _client.GetObjectAsync(bucketName, key, cancellationToken).ConfigureAwait(false);
    }

    /// <inheritdoc />
    public async Task DownloadToFileAsync(string bucketName, string key, string filePath, CancellationToken cancellationToken = default)
    {
        using var response = await _client.GetObjectAsync(bucketName, key, cancellationToken).ConfigureAwait(false);
        await response.WriteResponseStreamToFileAsync(filePath, false, cancellationToken).ConfigureAwait(false);
    }

    /// <inheritdoc />
    public async Task<DeleteObjectResponse> DeleteObjectAsync(string bucketName, string key, CancellationToken cancellationToken = default)
    {
        return await _client.DeleteObjectAsync(bucketName, key, cancellationToken).ConfigureAwait(false);
    }

    /// <inheritdoc />
    public async Task<DeleteObjectResponse> DeleteObjectAsync(DeleteObjectRequest request, CancellationToken cancellationToken = default)
    {
        return await _client.DeleteObjectAsync(request, cancellationToken).ConfigureAwait(false);
    }

    /// <inheritdoc />
    public async Task<DeleteObjectsResponse> DeleteObjectsAsync(DeleteObjectsRequest request, CancellationToken cancellationToken = default)
    {
        return await _client.DeleteObjectsAsync(request, cancellationToken).ConfigureAwait(false);
    }

    /// <inheritdoc />
    public async Task<ListObjectsV2Response> ListObjectsAsync(string bucketName, string? prefix = null, CancellationToken cancellationToken = default)
    {
        var request = new ListObjectsV2Request
        {
            BucketName = bucketName,
            Prefix = prefix,
        };
        return await _client.ListObjectsV2Async(request, cancellationToken).ConfigureAwait(false);
    }

    /// <inheritdoc />
    public async Task<ListObjectsV2Response> ListObjectsAsync(ListObjectsV2Request request, CancellationToken cancellationToken = default)
    {
        return await _client.ListObjectsV2Async(request, cancellationToken).ConfigureAwait(false);
    }

    /// <inheritdoc />
    public async Task<bool> DoesObjectExistAsync(string bucketName, string key, CancellationToken cancellationToken = default)
    {
        try
        {
            await _client.GetObjectMetadataAsync(bucketName, key, cancellationToken).ConfigureAwait(false);
            return true;
        }
        catch (AmazonS3Exception ex) when (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
        {
            return false;
        }
    }

    /// <inheritdoc />
    public async Task<GetObjectMetadataResponse> GetObjectMetadataAsync(string bucketName, string key, CancellationToken cancellationToken = default)
    {
        return await _client.GetObjectMetadataAsync(bucketName, key, cancellationToken).ConfigureAwait(false);
    }

    /// <inheritdoc />
    public async Task<CopyObjectResponse> CopyObjectAsync(CopyObjectRequest request, CancellationToken cancellationToken = default)
    {
        return await _client.CopyObjectAsync(request, cancellationToken).ConfigureAwait(false);
    }

    /// <inheritdoc />
    public async Task<CopyObjectResponse> CopyObjectAsync(string sourceBucket, string sourceKey, string destinationBucket, string destinationKey, CancellationToken cancellationToken = default)
    {
        return await _client.CopyObjectAsync(sourceBucket, sourceKey, destinationBucket, destinationKey, cancellationToken).ConfigureAwait(false);
    }

    #endregion

    #region Bucket Configuration

    /// <inheritdoc />
    public async Task<PutBucketEncryptionResponse> PutBucketEncryptionAsync(PutBucketEncryptionRequest request, CancellationToken cancellationToken = default)
    {
        return await _client.PutBucketEncryptionAsync(request, cancellationToken).ConfigureAwait(false);
    }

    /// <inheritdoc />
    public async Task<PutBucketLoggingResponse> PutBucketLoggingAsync(PutBucketLoggingRequest request, CancellationToken cancellationToken = default)
    {
        return await _client.PutBucketLoggingAsync(request, cancellationToken).ConfigureAwait(false);
    }

    /// <inheritdoc />
    public async Task<PutBucketPolicyResponse> PutBucketPolicyAsync(PutBucketPolicyRequest request, CancellationToken cancellationToken = default)
    {
        return await _client.PutBucketPolicyAsync(request, cancellationToken).ConfigureAwait(false);
    }

    /// <inheritdoc />
    public async Task<PutBucketWebsiteResponse> PutBucketWebsiteAsync(PutBucketWebsiteRequest request, CancellationToken cancellationToken = default)
    {
        return await _client.PutBucketWebsiteAsync(request, cancellationToken).ConfigureAwait(false);
    }

    /// <inheritdoc />
    public async Task<PutBucketNotificationResponse> PutBucketNotificationAsync(PutBucketNotificationRequest request, CancellationToken cancellationToken = default)
    {
        return await _client.PutBucketNotificationAsync(request, cancellationToken).ConfigureAwait(false);
    }

    /// <inheritdoc />
    public async Task<PutBucketReplicationResponse> PutBucketReplicationAsync(PutBucketReplicationRequest request, CancellationToken cancellationToken = default)
    {
        return await _client.PutBucketReplicationAsync(request, cancellationToken).ConfigureAwait(false);
    }

    /// <inheritdoc />
    public async Task<PutBucketTaggingResponse> PutBucketTaggingAsync(PutBucketTaggingRequest request, CancellationToken cancellationToken = default)
    {
        return await _client.PutBucketTaggingAsync(request, cancellationToken).ConfigureAwait(false);
    }

    /// <inheritdoc />
    public async Task<PutBucketVersioningResponse> PutBucketVersioningAsync(PutBucketVersioningRequest request, CancellationToken cancellationToken = default)
    {
        return await _client.PutBucketVersioningAsync(request, cancellationToken).ConfigureAwait(false);
    }

    /// <inheritdoc />
    public async Task<PutBucketAccelerateConfigurationResponse> PutBucketAccelerateConfigurationAsync(PutBucketAccelerateConfigurationRequest request, CancellationToken cancellationToken = default)
    {
        return await _client.PutBucketAccelerateConfigurationAsync(request, cancellationToken).ConfigureAwait(false);
    }

    /// <inheritdoc />
    public async Task<PutBucketAnalyticsConfigurationResponse> PutBucketAnalyticsConfigurationAsync(PutBucketAnalyticsConfigurationRequest request, CancellationToken cancellationToken = default)
    {
        return await _client.PutBucketAnalyticsConfigurationAsync(request, cancellationToken).ConfigureAwait(false);
    }

    /// <inheritdoc />
    public async Task<PutBucketInventoryConfigurationResponse> PutBucketInventoryConfigurationAsync(PutBucketInventoryConfigurationRequest request, CancellationToken cancellationToken = default)
    {
        return await _client.PutBucketInventoryConfigurationAsync(request, cancellationToken).ConfigureAwait(false);
    }

    /// <inheritdoc />
    public async Task<PutBucketMetricsConfigurationResponse> PutBucketMetricsConfigurationAsync(PutBucketMetricsConfigurationRequest request, CancellationToken cancellationToken = default)
    {
        return await _client.PutBucketMetricsConfigurationAsync(request, cancellationToken).ConfigureAwait(false);
    }

    /// <inheritdoc />
    public async Task<PutBucketOwnershipControlsResponse> PutBucketOwnershipControlsAsync(PutBucketOwnershipControlsRequest request, CancellationToken cancellationToken = default)
    {
        return await _client.PutBucketOwnershipControlsAsync(request, cancellationToken).ConfigureAwait(false);
    }

    /// <inheritdoc />
    public async Task<PutBucketRequestPaymentResponse> PutBucketRequestPaymentAsync(PutBucketRequestPaymentRequest request, CancellationToken cancellationToken = default)
    {
        return await _client.PutBucketRequestPaymentAsync(request, cancellationToken).ConfigureAwait(false);
    }

    /// <inheritdoc />
    public async Task<PutBucketIntelligentTieringConfigurationResponse> PutBucketIntelligentTieringConfigurationAsync(PutBucketIntelligentTieringConfigurationRequest request, CancellationToken cancellationToken = default)
    {
        return await _client.PutBucketIntelligentTieringConfigurationAsync(request, cancellationToken).ConfigureAwait(false);
    }

    #endregion

    #region Presigned URLs

    /// <inheritdoc />
    public string GetPresignedUrl(string bucketName, string key, DateTime expiration)
    {
        var request = new GetPreSignedUrlRequest
        {
            BucketName = bucketName,
            Key = key,
            Expires = expiration,
        };
        return _client.GetPreSignedURL(request);
    }

    /// <inheritdoc />
    public string GetPresignedUrl(GetPreSignedUrlRequest request)
    {
        return _client.GetPreSignedURL(request);
    }

    #endregion

    #region IDisposable

    /// <summary>
    /// Disposes the underlying Amazon S3 client.
    /// </summary>
    public void Dispose()
    {
        Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }

    /// <summary>
    /// Disposes the underlying Amazon S3 client.
    /// </summary>
    /// <param name="disposing">True if called from Dispose(), false if called from finalizer.</param>
    protected virtual void Dispose(bool disposing)
    {
        if (_disposed)
        {
            return;
        }

        if (disposing)
        {
            _client.Dispose();
        }

        _disposed = true;
    }

    #endregion
}
