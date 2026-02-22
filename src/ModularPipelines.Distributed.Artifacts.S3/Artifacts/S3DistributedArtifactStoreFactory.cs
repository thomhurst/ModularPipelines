using Amazon;
using Amazon.S3;
using Amazon.S3.Model;
using ModularPipelines.Distributed.Artifacts.S3.Configuration;

namespace ModularPipelines.Distributed.Artifacts.S3.Artifacts;

/// <summary>
/// Factory that creates a <see cref="S3DistributedArtifactStore"/> by initializing the S3 client.
/// Optionally configures a lifecycle rule for automatic artifact expiration.
/// </summary>
internal sealed class S3DistributedArtifactStoreFactory : IDistributedArtifactStoreFactory
{
    private readonly S3ArtifactOptions _s3Options;
    private readonly ArtifactOptions _artifactOptions;

    public S3DistributedArtifactStoreFactory(
        S3ArtifactOptions s3Options,
        ArtifactOptions artifactOptions)
    {
        _s3Options = s3Options;
        _artifactOptions = artifactOptions;
    }

    public async Task<IDistributedArtifactStore> CreateAsync(CancellationToken cancellationToken)
    {
        var config = new AmazonS3Config
        {
            RegionEndpoint = RegionEndpoint.GetBySystemName(_s3Options.Region),
            ForcePathStyle = _s3Options.ForcePathStyle,
        };

        if (!string.IsNullOrEmpty(_s3Options.ServiceUrl))
        {
            config.ServiceURL = _s3Options.ServiceUrl;
        }

        IAmazonS3 s3;
        if (!string.IsNullOrEmpty(_s3Options.AccessKey) && !string.IsNullOrEmpty(_s3Options.SecretKey))
        {
            s3 = new AmazonS3Client(_s3Options.AccessKey, _s3Options.SecretKey, config);
        }
        else
        {
            s3 = new AmazonS3Client(config);
        }

        var runId = RunIdentifierResolver.Resolve(_s3Options.RunIdentifier);

        if (_s3Options.SetLifecycleRule)
        {
            await TrySetLifecycleRuleAsync(s3, cancellationToken);
        }

        return new S3DistributedArtifactStore(
            s3,
            _s3Options.BucketName,
            _s3Options.KeyPrefix,
            runId,
            _artifactOptions.TimeToLiveSeconds);
    }

    private async Task TrySetLifecycleRuleAsync(IAmazonS3 s3, CancellationToken cancellationToken)
    {
        try
        {
            var request = new PutLifecycleConfigurationRequest
            {
                BucketName = _s3Options.BucketName,
                Configuration = new LifecycleConfiguration
                {
                    Rules =
                    [
                        new LifecycleRule
                        {
                            Id = "modpipe-artifact-expiration",
                            Status = LifecycleRuleStatus.Enabled,
                            Filter = new LifecycleFilter
                            {
                                LifecycleFilterPredicate = new LifecyclePrefixPredicate
                                {
                                    Prefix = _s3Options.KeyPrefix,
                                },
                            },
                            Expiration = new LifecycleRuleExpiration
                            {
                                Days = Math.Max(1, _artifactOptions.TimeToLiveSeconds / 86400),
                            },
                        },
                    ],
                },
            };

            await s3.PutLifecycleConfigurationAsync(request, cancellationToken);
        }
        catch
        {
            // Lifecycle configuration may not be supported by all S3-compatible providers
        }
    }
}
