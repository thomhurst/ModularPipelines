using Microsoft.Extensions.DependencyInjection;
using ModularPipelines.Distributed.Artifacts.S3.Artifacts;
using ModularPipelines.Distributed.Artifacts.S3.Configuration;

namespace ModularPipelines.Distributed.Artifacts.S3.Extensions;

/// <summary>
/// Extension methods for registering the S3-compatible distributed artifact store.
/// </summary>
public static class S3DistributedExtensions
{
    /// <summary>
    /// Registers the S3-compatible distributed artifact store factory.
    /// Works with AWS S3, Cloudflare R2, Backblaze B2, and MinIO.
    /// Must be called after <c>AddDistributedMode</c>.
    /// </summary>
    public static PipelineBuilder AddS3DistributedArtifactStore(
        this PipelineBuilder builder,
        Action<S3ArtifactOptions> configure)
    {
        var s3Options = new S3ArtifactOptions();
        configure(s3Options);

        builder.Services.AddSingleton(s3Options);

        // Register artifact options if not already registered
        if (!builder.Services.Any(d => d.ServiceType == typeof(ArtifactOptions)))
        {
            builder.Services.AddSingleton(new ArtifactOptions());
        }

        builder.Services.AddSingleton<IDistributedArtifactStoreFactory, S3DistributedArtifactStoreFactory>();

        return builder;
    }

    /// <summary>
    /// Registers the S3-compatible distributed artifact store factory with custom artifact options.
    /// </summary>
    public static PipelineBuilder AddS3DistributedArtifactStore(
        this PipelineBuilder builder,
        Action<S3ArtifactOptions> configureS3,
        Action<ArtifactOptions> configureArtifacts)
    {
        var s3Options = new S3ArtifactOptions();
        configureS3(s3Options);

        var artifactOptions = new ArtifactOptions();
        configureArtifacts(artifactOptions);

        builder.Services.AddSingleton(s3Options);
        builder.Services.AddSingleton(artifactOptions);
        builder.Services.AddSingleton<IDistributedArtifactStoreFactory, S3DistributedArtifactStoreFactory>();

        return builder;
    }
}
