using System.Diagnostics.CodeAnalysis;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using ModularPipelines.Caching;
using ModularPipelines.Distributed;
using ModularPipelines.Distributed.Artifacts.S3.Artifacts;
using ModularPipelines.Distributed.Artifacts.S3.Caching;
using ModularPipelines.Extensions;

namespace ModularPipelines.Distributed.Artifacts.S3;

/// <summary>
/// Extension methods for registering the S3-compatible distributed artifact store.
/// </summary>
public static class S3DistributedExtensions
{
    private const string ModuleCacheOptionsName = "ModularPipelines.S3ModuleCache";

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
        builder.Services.Configure(ModuleCacheOptionsName, configureS3);
        return AddS3ModuleCacheServices(builder, configureCache);
    }

    /// <summary>
    /// Enables a shareable S3-backed module cache from configuration without enabling distributed execution.
    /// </summary>
    [RequiresUnreferencedCode("Configuration binding requires members of S3ArtifactOptions that cannot be statically discovered.")]
    [RequiresDynamicCode("Configuration binding may require runtime code generation.")]
    public static PipelineBuilder AddS3ModuleCache(
        this PipelineBuilder builder,
        IConfigurationSection section,
        Action<ModuleCacheOptions>? configureCache = null)
    {
        builder.Services.Configure<S3ArtifactOptions>(ModuleCacheOptionsName, section);
        return AddS3ModuleCacheServices(builder, configureCache);
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
        builder.Services.Configure(configure);
        return AddS3DistributedArtifactStoreFactory(builder);
    }

    /// <summary>
    /// Registers the S3-compatible distributed artifact store factory from configuration.
    /// </summary>
    [RequiresUnreferencedCode("Configuration binding requires members of S3ArtifactOptions that cannot be statically discovered.")]
    [RequiresDynamicCode("Configuration binding may require runtime code generation.")]
    public static PipelineBuilder AddS3DistributedArtifactStore(
        this PipelineBuilder builder,
        IConfigurationSection section)
    {
        builder.Services.Configure<S3ArtifactOptions>(section);
        return AddS3DistributedArtifactStoreFactory(builder);
    }

    /// <summary>
    /// Registers the S3-compatible distributed artifact store factory with custom artifact options.
    /// </summary>
    public static PipelineBuilder AddS3DistributedArtifactStore(
        this PipelineBuilder builder,
        Action<S3ArtifactOptions> configureS3,
        Action<ArtifactOptions> configureArtifacts)
    {
        builder.Services.Configure(configureS3);
        builder.Services.Configure(configureArtifacts);
        return AddS3DistributedArtifactStoreFactory(builder);
    }

    /// <summary>
    /// Registers the S3-compatible distributed artifact store factory from configuration with custom artifact options.
    /// </summary>
    [RequiresUnreferencedCode("Configuration binding requires members of S3ArtifactOptions and ArtifactOptions that cannot be statically discovered.")]
    [RequiresDynamicCode("Configuration binding may require runtime code generation.")]
    public static PipelineBuilder AddS3DistributedArtifactStore(
        this PipelineBuilder builder,
        IConfigurationSection s3Section,
        IConfigurationSection artifactSection)
    {
        builder.Services.Configure<S3ArtifactOptions>(s3Section);
        builder.Services.Configure<ArtifactOptions>(artifactSection);
        return AddS3DistributedArtifactStoreFactory(builder);
    }

    private static PipelineBuilder AddS3DistributedArtifactStoreFactory(PipelineBuilder builder)
    {
        builder.Services.Configure<DistributedOptions>(options => options.RequireExplicitRunId = true);
        builder.Services.AddOptions<DistributedOptions>().ValidateOnStart();
        return builder.AddDistributedArtifactStoreFactory<S3DistributedArtifactStoreFactory>();
    }

    private static PipelineBuilder AddS3ModuleCacheServices(
        PipelineBuilder builder,
        Action<ModuleCacheOptions>? configureCache)
    {
        builder.Services.AddSingleton(serviceProvider => new S3ModuleCache(
            serviceProvider.GetRequiredService<IOptionsMonitor<S3ArtifactOptions>>()
                .Get(ModuleCacheOptionsName),
            serviceProvider.GetRequiredService<IOptions<ModuleCacheOptions>>().Value));
        return builder.AddModuleCache<S3ModuleCache>(configureCache);
    }
}
