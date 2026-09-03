using Amazon.S3;
using Amazon.S3.Model;
using Microsoft.Extensions.Options;
using ModularPipelines.Distributed.Artifacts.S3;
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
        IOptions<S3ArtifactOptions> s3Options,
        IOptions<ArtifactOptions> artifactOptions)
    {
        _s3Options = s3Options.Value;
        _artifactOptions = artifactOptions.Value;
    }

    public async Task<IDistributedArtifactStore> CreateAsync(CancellationToken cancellationToken)
    {
        var s3 = S3ClientFactory.Create(_s3Options);
        try
        {
            var runId = RunIdentifierResolver.Resolve(_s3Options.RunIdentifier);

            if (_s3Options.SetLifecycleRule)
            {
                await TrySetLifecycleRuleAsync(s3, cancellationToken);
            }

            return new S3DistributedArtifactStore(
                s3,
                _s3Options.BucketName,
                _s3Options.KeyPrefix,
                runId);
        }
        catch
        {
            s3.Dispose();
            throw;
        }
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
                                Days = Math.Max(1, (int) Math.Ceiling(_artifactOptions.TimeToLive.TotalDays)),
                            },
                        },
                    ],
                },
            };

            await s3.PutLifecycleConfigurationAsync(request, cancellationToken);
        }
        catch (Exception ex) when (ex is not OperationCanceledException)
        {
            // Lifecycle configuration may not be supported by all S3-compatible providers
        }
    }
}
