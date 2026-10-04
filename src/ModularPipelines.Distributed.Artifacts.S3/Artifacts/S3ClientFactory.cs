using Amazon;
using Amazon.S3;

namespace ModularPipelines.Distributed.Artifacts.S3.Artifacts;

internal static class S3ClientFactory
{
    public static IAmazonS3 Create(S3StorageOptions options)
    {
        var config = new AmazonS3Config
        {
            RegionEndpoint = RegionEndpoint.GetBySystemName(options.Region),
            ForcePathStyle = options.ForcePathStyle,
        };

        if (options.ServiceUrl is { } serviceUrl)
        {
            config.ServiceURL = serviceUrl.OriginalString;
        }

        return !string.IsNullOrEmpty(options.AccessKey) && !string.IsNullOrEmpty(options.SecretKey)
            ? new AmazonS3Client(options.AccessKey, options.SecretKey, config)
            : new AmazonS3Client(config);
    }
}
