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
    /// Gets the underlying Amazon S3 client.
    /// </summary>
    /// <remarks>
    /// Direct access to the underlying client is deprecated. Use the wrapper methods
    /// (CreateBucketAsync, UploadObjectAsync, etc.) instead for a more consistent API.
    /// </remarks>
    [Obsolete("Direct access to the underlying client is deprecated. Use the wrapper methods instead. This property will be removed in a future version.")]
    public AmazonS3Client Client => _client;

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

    #region Legacy Methods (Backward Compatibility)

    /// <summary>
    /// Creates a new S3 bucket.
    /// </summary>
    /// <param name="request">The request containing bucket creation parameters.</param>
    /// <param name="cancellationToken">A cancellation token to cancel the operation.</param>
    /// <returns>The response from the PutBucket operation.</returns>
    /// <remarks>This method is maintained for backward compatibility. Use <see cref="CreateBucketAsync"/> instead.</remarks>
    [Obsolete("Use CreateBucketAsync instead. This method will be removed in a future version.")]
    public async Task<PutBucketResponse> Bucket(PutBucketRequest request, CancellationToken cancellationToken = default)
    {
        return await CreateBucketAsync(request, cancellationToken).ConfigureAwait(false);
    }

    /// <summary>
    /// Sets the encryption configuration for a bucket.
    /// </summary>
    /// <param name="request">The request containing encryption settings.</param>
    /// <param name="cancellationToken">A cancellation token to cancel the operation.</param>
    /// <returns>The response from the operation.</returns>
    /// <remarks>This method is maintained for backward compatibility. Use <see cref="PutBucketEncryptionAsync"/> instead.</remarks>
    [Obsolete("Use PutBucketEncryptionAsync instead. This method will be removed in a future version.")]
    public async Task<PutBucketEncryptionResponse> BucketEncryption(PutBucketEncryptionRequest request, CancellationToken cancellationToken = default)
    {
        return await PutBucketEncryptionAsync(request, cancellationToken).ConfigureAwait(false);
    }

    /// <summary>
    /// Sets the logging configuration for a bucket.
    /// </summary>
    /// <param name="request">The request containing logging settings.</param>
    /// <param name="cancellationToken">A cancellation token to cancel the operation.</param>
    /// <returns>The response from the operation.</returns>
    /// <remarks>This method is maintained for backward compatibility. Use <see cref="PutBucketLoggingAsync"/> instead.</remarks>
    [Obsolete("Use PutBucketLoggingAsync instead. This method will be removed in a future version.")]
    public async Task<PutBucketLoggingResponse> BucketLogging(PutBucketLoggingRequest request, CancellationToken cancellationToken = default)
    {
        return await PutBucketLoggingAsync(request, cancellationToken).ConfigureAwait(false);
    }

    /// <summary>
    /// Sets the bucket policy.
    /// </summary>
    /// <param name="request">The request containing the policy.</param>
    /// <param name="cancellationToken">A cancellation token to cancel the operation.</param>
    /// <returns>The response from the operation.</returns>
    /// <remarks>This method is maintained for backward compatibility. Use <see cref="PutBucketPolicyAsync"/> instead.</remarks>
    [Obsolete("Use PutBucketPolicyAsync instead. This method will be removed in a future version.")]
    public async Task<PutBucketPolicyResponse> BucketPolicy(PutBucketPolicyRequest request, CancellationToken cancellationToken = default)
    {
        return await PutBucketPolicyAsync(request, cancellationToken).ConfigureAwait(false);
    }

    /// <summary>
    /// Configures the website settings for a bucket.
    /// </summary>
    /// <param name="request">The request containing website settings.</param>
    /// <param name="cancellationToken">A cancellation token to cancel the operation.</param>
    /// <returns>The response from the operation.</returns>
    /// <remarks>This method is maintained for backward compatibility. Use <see cref="PutBucketWebsiteAsync"/> instead.</remarks>
    [Obsolete("Use PutBucketWebsiteAsync instead. This method will be removed in a future version.")]
    public async Task<PutBucketWebsiteResponse> BucketWebsite(PutBucketWebsiteRequest request, CancellationToken cancellationToken = default)
    {
        return await PutBucketWebsiteAsync(request, cancellationToken).ConfigureAwait(false);
    }

    /// <summary>
    /// Sets the notification configuration for a bucket.
    /// </summary>
    /// <param name="request">The request containing notification settings.</param>
    /// <param name="cancellationToken">A cancellation token to cancel the operation.</param>
    /// <returns>The response from the operation.</returns>
    /// <remarks>This method is maintained for backward compatibility. Use <see cref="PutBucketNotificationAsync"/> instead.</remarks>
    [Obsolete("Use PutBucketNotificationAsync instead. This method will be removed in a future version.")]
    public async Task<PutBucketNotificationResponse> BucketNotification(PutBucketNotificationRequest request, CancellationToken cancellationToken = default)
    {
        return await PutBucketNotificationAsync(request, cancellationToken).ConfigureAwait(false);
    }

    /// <summary>
    /// Sets the replication configuration for a bucket.
    /// </summary>
    /// <param name="request">The request containing replication settings.</param>
    /// <param name="cancellationToken">A cancellation token to cancel the operation.</param>
    /// <returns>The response from the operation.</returns>
    /// <remarks>This method is maintained for backward compatibility. Use <see cref="PutBucketReplicationAsync"/> instead.</remarks>
    [Obsolete("Use PutBucketReplicationAsync instead. This method will be removed in a future version.")]
    public async Task<PutBucketReplicationResponse> BucketReplication(PutBucketReplicationRequest request, CancellationToken cancellationToken = default)
    {
        return await PutBucketReplicationAsync(request, cancellationToken).ConfigureAwait(false);
    }

    /// <summary>
    /// Sets the tagging configuration for a bucket.
    /// </summary>
    /// <param name="request">The request containing tagging settings.</param>
    /// <param name="cancellationToken">A cancellation token to cancel the operation.</param>
    /// <returns>The response from the operation.</returns>
    /// <remarks>This method is maintained for backward compatibility. Use <see cref="PutBucketTaggingAsync"/> instead.</remarks>
    [Obsolete("Use PutBucketTaggingAsync instead. This method will be removed in a future version.")]
    public async Task<PutBucketTaggingResponse> BucketTagging(PutBucketTaggingRequest request, CancellationToken cancellationToken = default)
    {
        return await PutBucketTaggingAsync(request, cancellationToken).ConfigureAwait(false);
    }

    /// <summary>
    /// Sets the versioning configuration for a bucket.
    /// </summary>
    /// <param name="request">The request containing versioning settings.</param>
    /// <param name="cancellationToken">A cancellation token to cancel the operation.</param>
    /// <returns>The response from the operation.</returns>
    /// <remarks>This method is maintained for backward compatibility. Use <see cref="PutBucketVersioningAsync"/> instead.</remarks>
    [Obsolete("Use PutBucketVersioningAsync instead. This method will be removed in a future version.")]
    public async Task<PutBucketVersioningResponse> BucketVersioning(PutBucketVersioningRequest request, CancellationToken cancellationToken = default)
    {
        return await PutBucketVersioningAsync(request, cancellationToken).ConfigureAwait(false);
    }

    /// <summary>
    /// Sets the accelerate configuration for a bucket.
    /// </summary>
    /// <param name="request">The request containing accelerate settings.</param>
    /// <param name="cancellationToken">A cancellation token to cancel the operation.</param>
    /// <returns>The response from the operation.</returns>
    /// <remarks>This method is maintained for backward compatibility. Use <see cref="PutBucketAccelerateConfigurationAsync"/> instead.</remarks>
    [Obsolete("Use PutBucketAccelerateConfigurationAsync instead. This method will be removed in a future version.")]
    public async Task<PutBucketAccelerateConfigurationResponse> BucketAccelerateConfiguration(PutBucketAccelerateConfigurationRequest request, CancellationToken cancellationToken = default)
    {
        return await PutBucketAccelerateConfigurationAsync(request, cancellationToken).ConfigureAwait(false);
    }

    /// <summary>
    /// Sets the analytics configuration for a bucket.
    /// </summary>
    /// <param name="request">The request containing analytics settings.</param>
    /// <param name="cancellationToken">A cancellation token to cancel the operation.</param>
    /// <returns>The response from the operation.</returns>
    /// <remarks>This method is maintained for backward compatibility. Use <see cref="PutBucketAnalyticsConfigurationAsync"/> instead.</remarks>
    [Obsolete("Use PutBucketAnalyticsConfigurationAsync instead. This method will be removed in a future version.")]
    public async Task<PutBucketAnalyticsConfigurationResponse> BucketAnalyticsConfiguration(PutBucketAnalyticsConfigurationRequest request, CancellationToken cancellationToken = default)
    {
        return await PutBucketAnalyticsConfigurationAsync(request, cancellationToken).ConfigureAwait(false);
    }

    /// <summary>
    /// Sets the inventory configuration for a bucket.
    /// </summary>
    /// <param name="request">The request containing inventory settings.</param>
    /// <param name="cancellationToken">A cancellation token to cancel the operation.</param>
    /// <returns>The response from the operation.</returns>
    /// <remarks>This method is maintained for backward compatibility. Use <see cref="PutBucketInventoryConfigurationAsync"/> instead.</remarks>
    [Obsolete("Use PutBucketInventoryConfigurationAsync instead. This method will be removed in a future version.")]
    public async Task<PutBucketInventoryConfigurationResponse> BucketInventoryConfiguration(PutBucketInventoryConfigurationRequest request, CancellationToken cancellationToken = default)
    {
        return await PutBucketInventoryConfigurationAsync(request, cancellationToken).ConfigureAwait(false);
    }

    /// <summary>
    /// Sets the metrics configuration for a bucket.
    /// </summary>
    /// <param name="request">The request containing metrics settings.</param>
    /// <param name="cancellationToken">A cancellation token to cancel the operation.</param>
    /// <returns>The response from the operation.</returns>
    /// <remarks>This method is maintained for backward compatibility. Use <see cref="PutBucketMetricsConfigurationAsync"/> instead.</remarks>
    [Obsolete("Use PutBucketMetricsConfigurationAsync instead. This method will be removed in a future version.")]
    public async Task<PutBucketMetricsConfigurationResponse> BucketMetricsConfiguration(PutBucketMetricsConfigurationRequest request, CancellationToken cancellationToken = default)
    {
        return await PutBucketMetricsConfigurationAsync(request, cancellationToken).ConfigureAwait(false);
    }

    /// <summary>
    /// Sets the ownership controls for a bucket.
    /// </summary>
    /// <param name="request">The request containing ownership control settings.</param>
    /// <param name="cancellationToken">A cancellation token to cancel the operation.</param>
    /// <returns>The response from the operation.</returns>
    /// <remarks>This method is maintained for backward compatibility. Use <see cref="PutBucketOwnershipControlsAsync"/> instead.</remarks>
    [Obsolete("Use PutBucketOwnershipControlsAsync instead. This method will be removed in a future version.")]
    public async Task<PutBucketOwnershipControlsResponse> BucketOwnershipControls(PutBucketOwnershipControlsRequest request, CancellationToken cancellationToken = default)
    {
        return await PutBucketOwnershipControlsAsync(request, cancellationToken).ConfigureAwait(false);
    }

    /// <summary>
    /// Sets the request payment configuration for a bucket.
    /// </summary>
    /// <param name="request">The request containing payment settings.</param>
    /// <param name="cancellationToken">A cancellation token to cancel the operation.</param>
    /// <returns>The response from the operation.</returns>
    /// <remarks>This method is maintained for backward compatibility. Use <see cref="PutBucketRequestPaymentAsync"/> instead.</remarks>
    [Obsolete("Use PutBucketRequestPaymentAsync instead. This method will be removed in a future version.")]
    public async Task<PutBucketRequestPaymentResponse> BucketRequestPayment(PutBucketRequestPaymentRequest request, CancellationToken cancellationToken = default)
    {
        return await PutBucketRequestPaymentAsync(request, cancellationToken).ConfigureAwait(false);
    }

    /// <summary>
    /// Sets the intelligent tiering configuration for a bucket.
    /// </summary>
    /// <param name="request">The request containing intelligent tiering settings.</param>
    /// <param name="cancellationToken">A cancellation token to cancel the operation.</param>
    /// <returns>The response from the operation.</returns>
    /// <remarks>This method is maintained for backward compatibility. Use <see cref="PutBucketIntelligentTieringConfigurationAsync"/> instead.</remarks>
    [Obsolete("Use PutBucketIntelligentTieringConfigurationAsync instead. This method will be removed in a future version.")]
    public async Task<PutBucketIntelligentTieringConfigurationResponse> BucketIntelligentTieringConfiguration(PutBucketIntelligentTieringConfigurationRequest request, CancellationToken cancellationToken = default)
    {
        return await PutBucketIntelligentTieringConfigurationAsync(request, cancellationToken).ConfigureAwait(false);
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
