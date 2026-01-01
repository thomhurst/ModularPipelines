using System.Diagnostics.CodeAnalysis;
using Amazon.S3;
using Amazon.S3.Model;

namespace ModularPipelines.AmazonWebServices;

[ExcludeFromCodeCoverage]
public class S3
{
    public AmazonS3Client Client { get; }

    public S3(AmazonS3Client client)
    {
        Client = client;
    }

    public async Task<PutBucketResponse> Bucket(PutBucketRequest request, CancellationToken cancellationToken = default)
    {
        return await Client.PutBucketAsync(request, cancellationToken).ConfigureAwait(false);
    }

    public async Task<PutBucketEncryptionResponse> BucketEncryption(PutBucketEncryptionRequest request, CancellationToken cancellationToken = default)
    {
        return await Client.PutBucketEncryptionAsync(request, cancellationToken).ConfigureAwait(false);
    }

    public async Task<PutBucketLoggingResponse> BucketLogging(PutBucketLoggingRequest request, CancellationToken cancellationToken = default)
    {
        return await Client.PutBucketLoggingAsync(request, cancellationToken).ConfigureAwait(false);
    }

    public async Task<PutBucketPolicyResponse> BucketPolicy(PutBucketPolicyRequest request, CancellationToken cancellationToken = default)
    {
        return await Client.PutBucketPolicyAsync(request, cancellationToken).ConfigureAwait(false);
    }

    public async Task<PutBucketWebsiteResponse> BucketWebsite(PutBucketWebsiteRequest request, CancellationToken cancellationToken = default)
    {
        return await Client.PutBucketWebsiteAsync(request, cancellationToken).ConfigureAwait(false);
    }

    public async Task<PutBucketNotificationResponse> BucketNotification(PutBucketNotificationRequest request, CancellationToken cancellationToken = default)
    {
        return await Client.PutBucketNotificationAsync(request, cancellationToken).ConfigureAwait(false);
    }

    public async Task<PutBucketReplicationResponse> BucketReplication(PutBucketReplicationRequest request, CancellationToken cancellationToken = default)
    {
        return await Client.PutBucketReplicationAsync(request, cancellationToken).ConfigureAwait(false);
    }

    public async Task<PutBucketTaggingResponse> BucketTagging(PutBucketTaggingRequest request, CancellationToken cancellationToken = default)
    {
        return await Client.PutBucketTaggingAsync(request, cancellationToken).ConfigureAwait(false);
    }

    public async Task<PutBucketVersioningResponse> BucketVersioning(PutBucketVersioningRequest request, CancellationToken cancellationToken = default)
    {
        return await Client.PutBucketVersioningAsync(request, cancellationToken).ConfigureAwait(false);
    }

    public async Task<PutBucketAccelerateConfigurationResponse> BucketAccelerateConfiguration(PutBucketAccelerateConfigurationRequest request, CancellationToken cancellationToken = default)
    {
        return await Client.PutBucketAccelerateConfigurationAsync(request, cancellationToken).ConfigureAwait(false);
    }

    public async Task<PutBucketAnalyticsConfigurationResponse> BucketAnalyticsConfiguration(PutBucketAnalyticsConfigurationRequest request, CancellationToken cancellationToken = default)
    {
        return await Client.PutBucketAnalyticsConfigurationAsync(request, cancellationToken).ConfigureAwait(false);
    }

    public async Task<PutBucketInventoryConfigurationResponse> BucketInventoryConfiguration(PutBucketInventoryConfigurationRequest request, CancellationToken cancellationToken = default)
    {
        return await Client.PutBucketInventoryConfigurationAsync(request, cancellationToken).ConfigureAwait(false);
    }

    public async Task<PutBucketMetricsConfigurationResponse> BucketMetricsConfiguration(PutBucketMetricsConfigurationRequest request, CancellationToken cancellationToken = default)
    {
        return await Client.PutBucketMetricsConfigurationAsync(request, cancellationToken).ConfigureAwait(false);
    }

    public async Task<PutBucketOwnershipControlsResponse> BucketOwnershipControls(PutBucketOwnershipControlsRequest request, CancellationToken cancellationToken = default)
    {
        return await Client.PutBucketOwnershipControlsAsync(request, cancellationToken).ConfigureAwait(false);
    }

    public async Task<PutBucketRequestPaymentResponse> BucketRequestPayment(PutBucketRequestPaymentRequest request, CancellationToken cancellationToken = default)
    {
        return await Client.PutBucketRequestPaymentAsync(request, cancellationToken).ConfigureAwait(false);
    }

    public async Task<PutBucketIntelligentTieringConfigurationResponse> BucketIntelligentTieringConfiguration(PutBucketIntelligentTieringConfigurationRequest request, CancellationToken cancellationToken = default)
    {
        return await Client.PutBucketIntelligentTieringConfigurationAsync(request, cancellationToken).ConfigureAwait(false);
    }
}
