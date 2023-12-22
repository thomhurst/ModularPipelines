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

    public async Task<PutBucketResponse> Bucket(PutBucketRequest request)
    {
        return await Client.PutBucketAsync(request);
    }

    public async Task<PutBucketEncryptionResponse> BucketEncryption(PutBucketEncryptionRequest request)
    {
        return await Client.PutBucketEncryptionAsync(request);
    }

    public async Task<PutBucketLoggingResponse> BucketLogging(PutBucketLoggingRequest request)
    {
        return await Client.PutBucketLoggingAsync(request);
    }

    public async Task<PutBucketPolicyResponse> BucketPolicy(PutBucketPolicyRequest request)
    {
        return await Client.PutBucketPolicyAsync(request);
    }

    public async Task<PutBucketWebsiteResponse> BucketWebsite(PutBucketWebsiteRequest request)
    {
        return await Client.PutBucketWebsiteAsync(request);
    }

    public async Task<PutBucketNotificationResponse> BucketNotification(PutBucketNotificationRequest request)
    {
        return await Client.PutBucketNotificationAsync(request);
    }

    public async Task<PutBucketReplicationResponse> BucketReplication(PutBucketReplicationRequest request)
    {
        return await Client.PutBucketReplicationAsync(request);
    }

    public async Task<PutBucketTaggingResponse> BucketTagging(PutBucketTaggingRequest request)
    {
        return await Client.PutBucketTaggingAsync(request);
    }

    public async Task<PutBucketVersioningResponse> BucketVersioning(PutBucketVersioningRequest request)
    {
        return await Client.PutBucketVersioningAsync(request);
    }

    public async Task<PutBucketAccelerateConfigurationResponse> BucketAccelerateConfiguration(PutBucketAccelerateConfigurationRequest request)
    {
        return await Client.PutBucketAccelerateConfigurationAsync(request);
    }

    public async Task<PutBucketAnalyticsConfigurationResponse> BucketAnalyticsConfiguration(PutBucketAnalyticsConfigurationRequest request)
    {
        return await Client.PutBucketAnalyticsConfigurationAsync(request);
    }

    public async Task<PutBucketInventoryConfigurationResponse> BucketInventoryConfiguration(PutBucketInventoryConfigurationRequest request)
    {
        return await Client.PutBucketInventoryConfigurationAsync(request);
    }

    public async Task<PutBucketMetricsConfigurationResponse> BucketMetricsConfiguration(PutBucketMetricsConfigurationRequest request)
    {
        return await Client.PutBucketMetricsConfigurationAsync(request);
    }

    public async Task<PutBucketOwnershipControlsResponse> BucketOwnershipControls(PutBucketOwnershipControlsRequest request)
    {
        return await Client.PutBucketOwnershipControlsAsync(request);
    }

    public async Task<PutBucketRequestPaymentResponse> BucketRequestPayment(PutBucketRequestPaymentRequest request)
    {
        return await Client.PutBucketRequestPaymentAsync(request);
    }

    public async Task<PutBucketIntelligentTieringConfigurationResponse> BucketIntelligentTieringConfiguration(PutBucketIntelligentTieringConfigurationRequest request)
    {
        return await Client.PutBucketIntelligentTieringConfigurationAsync(request);
    }
}