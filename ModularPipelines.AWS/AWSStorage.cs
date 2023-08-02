using Amazon.S3;
using Amazon.S3.Model;

namespace ModularPipelines.AWS;

public class AWSStorage
{
    public AmazonS3Client S3Client { get; }

    public AWSStorage(AmazonS3Client s3Client)
    {
        S3Client = s3Client;
    }

    public async Task<PutBucketResponse> Bucket(PutBucketRequest putBucketRequest)
    {
        return await S3Client.PutBucketAsync(putBucketRequest);
    }
}