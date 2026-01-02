using Amazon.S3.Model;

namespace ModularPipelines.AmazonWebServices;

/// <summary>
/// Provides a high-level interface for interacting with Amazon S3.
/// </summary>
/// <remarks>
/// This interface encapsulates the AWS S3 SDK client and provides wrapper methods
/// for common S3 operations including bucket management, object operations, and
/// configuration settings.
/// </remarks>
public interface IS3 : IDisposable
{
    #region Bucket Operations

    /// <summary>
    /// Creates a new S3 bucket.
    /// </summary>
    /// <param name="request">The request containing bucket creation parameters.</param>
    /// <param name="cancellationToken">A cancellation token to cancel the operation.</param>
    /// <returns>The response from the PutBucket operation.</returns>
    Task<PutBucketResponse> CreateBucketAsync(PutBucketRequest request, CancellationToken cancellationToken = default);

    /// <summary>
    /// Deletes an S3 bucket.
    /// </summary>
    /// <param name="bucketName">The name of the bucket to delete.</param>
    /// <param name="cancellationToken">A cancellation token to cancel the operation.</param>
    /// <returns>The response from the DeleteBucket operation.</returns>
    Task<DeleteBucketResponse> DeleteBucketAsync(string bucketName, CancellationToken cancellationToken = default);

    /// <summary>
    /// Deletes an S3 bucket.
    /// </summary>
    /// <param name="request">The request containing bucket deletion parameters.</param>
    /// <param name="cancellationToken">A cancellation token to cancel the operation.</param>
    /// <returns>The response from the DeleteBucket operation.</returns>
    Task<DeleteBucketResponse> DeleteBucketAsync(DeleteBucketRequest request, CancellationToken cancellationToken = default);

    /// <summary>
    /// Lists all S3 buckets owned by the authenticated sender of the request.
    /// </summary>
    /// <param name="cancellationToken">A cancellation token to cancel the operation.</param>
    /// <returns>The response containing the list of buckets.</returns>
    Task<ListBucketsResponse> ListBucketsAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Checks if a bucket exists and you have permission to access it.
    /// </summary>
    /// <param name="bucketName">The name of the bucket to check.</param>
    /// <param name="cancellationToken">A cancellation token to cancel the operation.</param>
    /// <returns>True if the bucket exists and is accessible; otherwise, false.</returns>
    Task<bool> DoesBucketExistAsync(string bucketName, CancellationToken cancellationToken = default);

    #endregion

    #region Object Operations

    /// <summary>
    /// Uploads an object to S3.
    /// </summary>
    /// <param name="request">The request containing the object data and metadata.</param>
    /// <param name="cancellationToken">A cancellation token to cancel the operation.</param>
    /// <returns>The response from the PutObject operation.</returns>
    Task<PutObjectResponse> UploadObjectAsync(PutObjectRequest request, CancellationToken cancellationToken = default);

    /// <summary>
    /// Uploads a file to S3.
    /// </summary>
    /// <param name="bucketName">The name of the target bucket.</param>
    /// <param name="key">The object key (path) in the bucket.</param>
    /// <param name="filePath">The local file path to upload.</param>
    /// <param name="cancellationToken">A cancellation token to cancel the operation.</param>
    /// <returns>The response from the PutObject operation.</returns>
    Task<PutObjectResponse> UploadFileAsync(string bucketName, string key, string filePath, CancellationToken cancellationToken = default);

    /// <summary>
    /// Uploads content from a stream to S3.
    /// </summary>
    /// <param name="bucketName">The name of the target bucket.</param>
    /// <param name="key">The object key (path) in the bucket.</param>
    /// <param name="inputStream">The stream containing the content to upload.</param>
    /// <param name="cancellationToken">A cancellation token to cancel the operation.</param>
    /// <returns>The response from the PutObject operation.</returns>
    Task<PutObjectResponse> UploadStreamAsync(string bucketName, string key, Stream inputStream, CancellationToken cancellationToken = default);

    /// <summary>
    /// Downloads an object from S3.
    /// </summary>
    /// <param name="request">The request containing the object location.</param>
    /// <param name="cancellationToken">A cancellation token to cancel the operation.</param>
    /// <returns>The response containing the object data.</returns>
    Task<GetObjectResponse> DownloadObjectAsync(GetObjectRequest request, CancellationToken cancellationToken = default);

    /// <summary>
    /// Downloads an object from S3.
    /// </summary>
    /// <param name="bucketName">The name of the bucket containing the object.</param>
    /// <param name="key">The object key (path) to download.</param>
    /// <param name="cancellationToken">A cancellation token to cancel the operation.</param>
    /// <returns>The response containing the object data.</returns>
    Task<GetObjectResponse> DownloadObjectAsync(string bucketName, string key, CancellationToken cancellationToken = default);

    /// <summary>
    /// Downloads an object from S3 to a local file.
    /// </summary>
    /// <param name="bucketName">The name of the bucket containing the object.</param>
    /// <param name="key">The object key (path) to download.</param>
    /// <param name="filePath">The local file path to save the downloaded object.</param>
    /// <param name="cancellationToken">A cancellation token to cancel the operation.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    Task DownloadToFileAsync(string bucketName, string key, string filePath, CancellationToken cancellationToken = default);

    /// <summary>
    /// Deletes an object from S3.
    /// </summary>
    /// <param name="bucketName">The name of the bucket containing the object.</param>
    /// <param name="key">The object key (path) to delete.</param>
    /// <param name="cancellationToken">A cancellation token to cancel the operation.</param>
    /// <returns>The response from the DeleteObject operation.</returns>
    Task<DeleteObjectResponse> DeleteObjectAsync(string bucketName, string key, CancellationToken cancellationToken = default);

    /// <summary>
    /// Deletes an object from S3.
    /// </summary>
    /// <param name="request">The request containing the object location.</param>
    /// <param name="cancellationToken">A cancellation token to cancel the operation.</param>
    /// <returns>The response from the DeleteObject operation.</returns>
    Task<DeleteObjectResponse> DeleteObjectAsync(DeleteObjectRequest request, CancellationToken cancellationToken = default);

    /// <summary>
    /// Deletes multiple objects from S3.
    /// </summary>
    /// <param name="request">The request containing the objects to delete.</param>
    /// <param name="cancellationToken">A cancellation token to cancel the operation.</param>
    /// <returns>The response from the DeleteObjects operation.</returns>
    Task<DeleteObjectsResponse> DeleteObjectsAsync(DeleteObjectsRequest request, CancellationToken cancellationToken = default);

    /// <summary>
    /// Lists objects in an S3 bucket.
    /// </summary>
    /// <param name="bucketName">The name of the bucket to list objects from.</param>
    /// <param name="prefix">The prefix to filter objects by (optional).</param>
    /// <param name="cancellationToken">A cancellation token to cancel the operation.</param>
    /// <returns>The response containing the list of objects.</returns>
    Task<ListObjectsV2Response> ListObjectsAsync(string bucketName, string? prefix = null, CancellationToken cancellationToken = default);

    /// <summary>
    /// Lists objects in an S3 bucket.
    /// </summary>
    /// <param name="request">The request containing listing parameters.</param>
    /// <param name="cancellationToken">A cancellation token to cancel the operation.</param>
    /// <returns>The response containing the list of objects.</returns>
    Task<ListObjectsV2Response> ListObjectsAsync(ListObjectsV2Request request, CancellationToken cancellationToken = default);

    /// <summary>
    /// Checks if an object exists in S3.
    /// </summary>
    /// <param name="bucketName">The name of the bucket to check.</param>
    /// <param name="key">The object key (path) to check.</param>
    /// <param name="cancellationToken">A cancellation token to cancel the operation.</param>
    /// <returns>True if the object exists; otherwise, false.</returns>
    Task<bool> DoesObjectExistAsync(string bucketName, string key, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets the metadata for an object without downloading it.
    /// </summary>
    /// <param name="bucketName">The name of the bucket containing the object.</param>
    /// <param name="key">The object key (path).</param>
    /// <param name="cancellationToken">A cancellation token to cancel the operation.</param>
    /// <returns>The response containing object metadata.</returns>
    Task<GetObjectMetadataResponse> GetObjectMetadataAsync(string bucketName, string key, CancellationToken cancellationToken = default);

    /// <summary>
    /// Copies an object within S3.
    /// </summary>
    /// <param name="request">The request containing copy parameters.</param>
    /// <param name="cancellationToken">A cancellation token to cancel the operation.</param>
    /// <returns>The response from the CopyObject operation.</returns>
    Task<CopyObjectResponse> CopyObjectAsync(CopyObjectRequest request, CancellationToken cancellationToken = default);

    /// <summary>
    /// Copies an object within S3.
    /// </summary>
    /// <param name="sourceBucket">The source bucket name.</param>
    /// <param name="sourceKey">The source object key.</param>
    /// <param name="destinationBucket">The destination bucket name.</param>
    /// <param name="destinationKey">The destination object key.</param>
    /// <param name="cancellationToken">A cancellation token to cancel the operation.</param>
    /// <returns>The response from the CopyObject operation.</returns>
    Task<CopyObjectResponse> CopyObjectAsync(string sourceBucket, string sourceKey, string destinationBucket, string destinationKey, CancellationToken cancellationToken = default);

    #endregion

    #region Bucket Configuration

    /// <summary>
    /// Sets the encryption configuration for a bucket.
    /// </summary>
    /// <param name="request">The request containing encryption settings.</param>
    /// <param name="cancellationToken">A cancellation token to cancel the operation.</param>
    /// <returns>The response from the operation.</returns>
    Task<PutBucketEncryptionResponse> PutBucketEncryptionAsync(PutBucketEncryptionRequest request, CancellationToken cancellationToken = default);

    /// <summary>
    /// Sets the logging configuration for a bucket.
    /// </summary>
    /// <param name="request">The request containing logging settings.</param>
    /// <param name="cancellationToken">A cancellation token to cancel the operation.</param>
    /// <returns>The response from the operation.</returns>
    Task<PutBucketLoggingResponse> PutBucketLoggingAsync(PutBucketLoggingRequest request, CancellationToken cancellationToken = default);

    /// <summary>
    /// Sets the bucket policy.
    /// </summary>
    /// <param name="request">The request containing the policy.</param>
    /// <param name="cancellationToken">A cancellation token to cancel the operation.</param>
    /// <returns>The response from the operation.</returns>
    Task<PutBucketPolicyResponse> PutBucketPolicyAsync(PutBucketPolicyRequest request, CancellationToken cancellationToken = default);

    /// <summary>
    /// Configures the website settings for a bucket.
    /// </summary>
    /// <param name="request">The request containing website settings.</param>
    /// <param name="cancellationToken">A cancellation token to cancel the operation.</param>
    /// <returns>The response from the operation.</returns>
    Task<PutBucketWebsiteResponse> PutBucketWebsiteAsync(PutBucketWebsiteRequest request, CancellationToken cancellationToken = default);

    /// <summary>
    /// Sets the notification configuration for a bucket.
    /// </summary>
    /// <param name="request">The request containing notification settings.</param>
    /// <param name="cancellationToken">A cancellation token to cancel the operation.</param>
    /// <returns>The response from the operation.</returns>
    Task<PutBucketNotificationResponse> PutBucketNotificationAsync(PutBucketNotificationRequest request, CancellationToken cancellationToken = default);

    /// <summary>
    /// Sets the replication configuration for a bucket.
    /// </summary>
    /// <param name="request">The request containing replication settings.</param>
    /// <param name="cancellationToken">A cancellation token to cancel the operation.</param>
    /// <returns>The response from the operation.</returns>
    Task<PutBucketReplicationResponse> PutBucketReplicationAsync(PutBucketReplicationRequest request, CancellationToken cancellationToken = default);

    /// <summary>
    /// Sets the tagging configuration for a bucket.
    /// </summary>
    /// <param name="request">The request containing tagging settings.</param>
    /// <param name="cancellationToken">A cancellation token to cancel the operation.</param>
    /// <returns>The response from the operation.</returns>
    Task<PutBucketTaggingResponse> PutBucketTaggingAsync(PutBucketTaggingRequest request, CancellationToken cancellationToken = default);

    /// <summary>
    /// Sets the versioning configuration for a bucket.
    /// </summary>
    /// <param name="request">The request containing versioning settings.</param>
    /// <param name="cancellationToken">A cancellation token to cancel the operation.</param>
    /// <returns>The response from the operation.</returns>
    Task<PutBucketVersioningResponse> PutBucketVersioningAsync(PutBucketVersioningRequest request, CancellationToken cancellationToken = default);

    /// <summary>
    /// Sets the accelerate configuration for a bucket.
    /// </summary>
    /// <param name="request">The request containing accelerate settings.</param>
    /// <param name="cancellationToken">A cancellation token to cancel the operation.</param>
    /// <returns>The response from the operation.</returns>
    Task<PutBucketAccelerateConfigurationResponse> PutBucketAccelerateConfigurationAsync(PutBucketAccelerateConfigurationRequest request, CancellationToken cancellationToken = default);

    /// <summary>
    /// Sets the analytics configuration for a bucket.
    /// </summary>
    /// <param name="request">The request containing analytics settings.</param>
    /// <param name="cancellationToken">A cancellation token to cancel the operation.</param>
    /// <returns>The response from the operation.</returns>
    Task<PutBucketAnalyticsConfigurationResponse> PutBucketAnalyticsConfigurationAsync(PutBucketAnalyticsConfigurationRequest request, CancellationToken cancellationToken = default);

    /// <summary>
    /// Sets the inventory configuration for a bucket.
    /// </summary>
    /// <param name="request">The request containing inventory settings.</param>
    /// <param name="cancellationToken">A cancellation token to cancel the operation.</param>
    /// <returns>The response from the operation.</returns>
    Task<PutBucketInventoryConfigurationResponse> PutBucketInventoryConfigurationAsync(PutBucketInventoryConfigurationRequest request, CancellationToken cancellationToken = default);

    /// <summary>
    /// Sets the metrics configuration for a bucket.
    /// </summary>
    /// <param name="request">The request containing metrics settings.</param>
    /// <param name="cancellationToken">A cancellation token to cancel the operation.</param>
    /// <returns>The response from the operation.</returns>
    Task<PutBucketMetricsConfigurationResponse> PutBucketMetricsConfigurationAsync(PutBucketMetricsConfigurationRequest request, CancellationToken cancellationToken = default);

    /// <summary>
    /// Sets the ownership controls for a bucket.
    /// </summary>
    /// <param name="request">The request containing ownership control settings.</param>
    /// <param name="cancellationToken">A cancellation token to cancel the operation.</param>
    /// <returns>The response from the operation.</returns>
    Task<PutBucketOwnershipControlsResponse> PutBucketOwnershipControlsAsync(PutBucketOwnershipControlsRequest request, CancellationToken cancellationToken = default);

    /// <summary>
    /// Sets the request payment configuration for a bucket.
    /// </summary>
    /// <param name="request">The request containing payment settings.</param>
    /// <param name="cancellationToken">A cancellation token to cancel the operation.</param>
    /// <returns>The response from the operation.</returns>
    Task<PutBucketRequestPaymentResponse> PutBucketRequestPaymentAsync(PutBucketRequestPaymentRequest request, CancellationToken cancellationToken = default);

    /// <summary>
    /// Sets the intelligent tiering configuration for a bucket.
    /// </summary>
    /// <param name="request">The request containing intelligent tiering settings.</param>
    /// <param name="cancellationToken">A cancellation token to cancel the operation.</param>
    /// <returns>The response from the operation.</returns>
    Task<PutBucketIntelligentTieringConfigurationResponse> PutBucketIntelligentTieringConfigurationAsync(PutBucketIntelligentTieringConfigurationRequest request, CancellationToken cancellationToken = default);

    #endregion

    #region Presigned URLs

    /// <summary>
    /// Generates a presigned URL for downloading an object.
    /// </summary>
    /// <param name="bucketName">The name of the bucket containing the object.</param>
    /// <param name="key">The object key (path).</param>
    /// <param name="expiration">The expiration time for the URL.</param>
    /// <returns>A presigned URL that can be used to download the object.</returns>
    string GetPresignedUrl(string bucketName, string key, DateTime expiration);

    /// <summary>
    /// Generates a presigned URL based on the request parameters.
    /// </summary>
    /// <param name="request">The request containing presigned URL parameters.</param>
    /// <returns>A presigned URL.</returns>
    string GetPresignedUrl(GetPreSignedUrlRequest request);

    #endregion
}
