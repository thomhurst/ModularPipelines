using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using ModularPipelines.Caching;
using ModularPipelines.Distributed.Artifacts.S3.Artifacts;
using ModularPipelines.Distributed.Artifacts.S3.Caching;
using ModularPipelines.Distributed.Artifacts.S3.Configuration;
using ModularPipelines.Distributed.Extensions;
using ModularPipelines.Extensions;

namespace ModularPipelines.Distributed.Artifacts.S3.Extensions;

/// <summary>
/// Extension methods for registering the S3-compatible distributed artifact store.
/// </summary>
public static class S3DistributedExtensions
{
    /// <summary>
    /// Enables a shareable S3-backed module cache without enabling distributed execution.
    /// </summary>
    /// <param name="builder">The pipeline builder.</param>
    /// <param name="configureS3">Configures the S3-compatible service.</param>
    /// <param name="configureCache">Optionally configures fingerprinting behavior.</param>
    /// <returns>The same builder instance for chaining.</returns>
    public static PipelineBuilder AddS3ModuleCache(
        this PipelineBuilder builder,
        Action<S3ArtifactOptions> configureS3,
        Action<ModuleCacheOptions>? configureCache = null)
    {
        var options = new S3ArtifactOptions();
        configureS3(options);
        builder.Services.AddSingleton(serviceProvider => new S3ModuleCache(
            options,
            serviceProvider.GetRequiredService<IOptions<ModuleCacheOptions>>().Value));
        return builder.AddModuleCache<S3ModuleCache>(configureCache);
    }

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
        return builder.AddDistributedArtifactStoreFactory<S3DistributedArtifactStoreFactory>();
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

        builder.Services.AddSingleton(s3Options);
        builder.Services.Configure(configureArtifacts);
        return builder.AddDistributedArtifactStoreFactory<S3DistributedArtifactStoreFactory>();
    }
}
