using System.Diagnostics.CodeAnalysis;
using Amazon.S3;
using Amazon.S3.Model;

namespace ModularPipelines.AWS;

[ExcludeFromCodeCoverage]
public class S3
{
    public AmazonS3Client S3Client { get; }

    public S3(AmazonS3Client s3Client)
    {
        S3Client = s3Client;
    }

    public async Task<PutBucketResponse> Bucket(PutBucketRequest request)
    {
        return await S3Client.PutBucketAsync(request);
    }
    
    public async Task<PutBucketEncryptionResponse> BucketEncryption(PutBucketEncryptionRequest request)
    {
        return await S3Client.PutBucketEncryptionAsync(request);
    }
    
    public async Task<PutBucketLoggingResponse> BucketLogging(PutBucketLoggingRequest request)
    {
        return await S3Client.PutBucketLoggingAsync(request);
    }
    
    public async Task<PutBucketPolicyResponse> BucketPolicy(PutBucketPolicyRequest request)
    {
        return await S3Client.PutBucketPolicyAsync(request);
    }
    
    public async Task<PutBucketWebsiteResponse> BucketWebsite(PutBucketWebsiteRequest request)
    {
        return await S3Client.PutBucketWebsiteAsync(request);
    }
    
    public async Task<PutBucketNotificationResponse> BucketNotification(PutBucketNotificationRequest request)
    {
        return await S3Client.PutBucketNotificationAsync(request);
    }
    
    public async Task<PutBucketReplicationResponse> BucketReplication(PutBucketReplicationRequest request)
    {
        return await S3Client.PutBucketReplicationAsync(request);
    }

    public async Task<PutBucketTaggingResponse> BucketTagging(PutBucketTaggingRequest request)
    {
        return await S3Client.PutBucketTaggingAsync(request);
    }

    public async Task<PutBucketVersioningResponse> BucketVersioning(PutBucketVersioningRequest request)
    {
        return await S3Client.PutBucketVersioningAsync(request);
    }
    
    public async Task<PutBucketAccelerateConfigurationResponse> BucketAccelerateConfiguration(PutBucketAccelerateConfigurationRequest request)
    {
        return await S3Client.PutBucketAccelerateConfigurationAsync(request);
    }
    
    public async Task<PutBucketAnalyticsConfigurationResponse> BucketAnalyticsConfiguration(PutBucketAnalyticsConfigurationRequest request)
    {
        return await S3Client.PutBucketAnalyticsConfigurationAsync(request);
    }
    
    public async Task<PutBucketInventoryConfigurationResponse> BucketInventoryConfiguration(PutBucketInventoryConfigurationRequest request)
    {
        return await S3Client.PutBucketInventoryConfigurationAsync(request);
    }
    
    public async Task<PutBucketMetricsConfigurationResponse> BucketMetricsConfiguration(PutBucketMetricsConfigurationRequest request)
    {
        return await S3Client.PutBucketMetricsConfigurationAsync(request);
    }
    
    public async Task<PutBucketOwnershipControlsResponse> BucketOwnershipControls(PutBucketOwnershipControlsRequest request)
    {
        return await S3Client.PutBucketOwnershipControlsAsync(request);
    }
    
    public async Task<PutBucketRequestPaymentResponse> BucketRequestPayment(PutBucketRequestPaymentRequest request)
    {
        return await S3Client.PutBucketRequestPaymentAsync(request);
    }
    
    public async Task<PutBucketIntelligentTieringConfigurationResponse> BucketIntelligentTieringConfiguration(PutBucketIntelligentTieringConfigurationRequest request)
    {
        return await S3Client.PutBucketIntelligentTieringConfigurationAsync(request);
    }
}