using System.Diagnostics.CodeAnalysis;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;
using ModularPipelines.Caching;
using ModularPipelines.Distributed.Artifacts.S3.Artifacts;
using ModularPipelines.Distributed.Artifacts.S3.Caching;
using ModularPipelines.Extensions;

namespace ModularPipelines.Distributed.Artifacts.S3;

/// <summary>
/// Extension methods for registering the S3-compatible distributed artifact store and module cache.
/// </summary>
/// <remarks>
/// The artifact store and the module cache each have their own <see cref="S3StorageOptions"/>, so they
/// can target different buckets. Artifact settings that apply to every backend are configured through
/// <see cref="DistributedOptions"/> and module cache settings through <see cref="ModuleCacheOptions"/>.
/// </remarks>
public static class S3DistributedExtensions
{
    private const string ModuleCacheOptionsName = "ModularPipelines.S3ModuleCache";

    /// <summary>
    /// Enables a shareable S3-backed module cache without enabling distributed execution.
    /// </summary>
    /// <param name="builder">The pipeline builder.</param>
    /// <param name="configure">Configures the S3-compatible service.</param>
    /// <param name="configureCache">Optionally transforms the immutable <see cref="ModuleCacheOptions"/>.</param>
    /// <returns>The same builder instance for chaining.</returns>
    public static PipelineBuilder AddS3ModuleCache(
        this PipelineBuilder builder,
        Action<S3StorageOptions> configure,
        Func<ModuleCacheOptions, ModuleCacheOptions>? configureCache = null)
    {
        ArgumentNullException.ThrowIfNull(builder);
        ArgumentNullException.ThrowIfNull(configure);
        builder.Services.Configure(ModuleCacheOptionsName, configure);
        return AddS3ModuleCacheServices(builder, configureCache);
    }

    /// <summary>
    /// Enables a shareable S3-backed module cache from configuration without enabling distributed execution.
    /// </summary>
    /// <param name="builder">The pipeline builder.</param>
    /// <param name="section">The configuration section bound to <see cref="S3StorageOptions"/>.</param>
    /// <param name="configureCache">Optionally transforms the immutable <see cref="ModuleCacheOptions"/>.</param>
    /// <returns>The same builder instance for chaining.</returns>
    [RequiresUnreferencedCode("Configuration binding requires members of S3StorageOptions that cannot be statically discovered.")]
    [RequiresDynamicCode("Configuration binding may require runtime code generation.")]
    public static PipelineBuilder AddS3ModuleCache(
        this PipelineBuilder builder,
        IConfigurationSection section,
        Func<ModuleCacheOptions, ModuleCacheOptions>? configureCache = null)
    {
        ArgumentNullException.ThrowIfNull(builder);
        ArgumentNullException.ThrowIfNull(section);
        builder.Services.Configure<S3StorageOptions>(ModuleCacheOptionsName, section);
        return AddS3ModuleCacheServices(builder, configureCache);
    }

    /// <summary>
    /// Registers the S3-compatible distributed artifact store.
    /// Works with AWS S3, Cloudflare R2, Backblaze B2, and MinIO.
    /// </summary>
    /// <param name="builder">The pipeline builder.</param>
    /// <param name="configure">Configures the S3-compatible service.</param>
    /// <returns>The same builder instance for chaining.</returns>
    public static PipelineBuilder AddS3DistributedArtifactStore(
        this PipelineBuilder builder,
        Action<S3StorageOptions> configure)
    {
        ArgumentNullException.ThrowIfNull(builder);
        ArgumentNullException.ThrowIfNull(configure);
        builder.Services.Configure(configure);
        return AddS3DistributedArtifactStoreServices(builder);
    }

    /// <summary>
    /// Registers the S3-compatible distributed artifact store from configuration.
    /// </summary>
    /// <param name="builder">The pipeline builder.</param>
    /// <param name="section">The configuration section bound to <see cref="S3StorageOptions"/>.</param>
    /// <returns>The same builder instance for chaining.</returns>
    [RequiresUnreferencedCode("Configuration binding requires members of S3StorageOptions that cannot be statically discovered.")]
    [RequiresDynamicCode("Configuration binding may require runtime code generation.")]
    public static PipelineBuilder AddS3DistributedArtifactStore(
        this PipelineBuilder builder,
        IConfigurationSection section)
    {
        ArgumentNullException.ThrowIfNull(builder);
        ArgumentNullException.ThrowIfNull(section);
        builder.Services.Configure<S3StorageOptions>(section);
        return AddS3DistributedArtifactStoreServices(builder);
    }

    private static void AddValidation(IServiceCollection services, string name)
    {
        services.TryAddEnumerable(ServiceDescriptor.Singleton<IValidateOptions<S3StorageOptions>, S3StorageOptionsValidator>());
        services.AddOptions<S3StorageOptions>(name).ValidateOnStart();
    }

    private static PipelineBuilder AddS3DistributedArtifactStoreServices(PipelineBuilder builder)
    {
        AddValidation(builder.Services, Microsoft.Extensions.Options.Options.DefaultName);
        return builder
            .RequireExplicitRunId()
            .AddDistributedArtifactStoreFactory<S3DistributedArtifactStoreFactory>();
    }

    private static PipelineBuilder AddS3ModuleCacheServices(
        PipelineBuilder builder,
        Func<ModuleCacheOptions, ModuleCacheOptions>? configureCache)
    {
        AddValidation(builder.Services, ModuleCacheOptionsName);
        builder.Services.TryAddSingleton(serviceProvider => new S3ModuleCache(
            serviceProvider.GetRequiredService<IOptionsMonitor<S3StorageOptions>>().Get(ModuleCacheOptionsName),
            serviceProvider.GetRequiredService<IOptions<ModuleCacheOptions>>().Value));
        return builder.AddModuleCache<S3ModuleCache>(configureCache);
    }
}
