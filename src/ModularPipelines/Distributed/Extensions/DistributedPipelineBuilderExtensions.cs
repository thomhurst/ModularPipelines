using System.Diagnostics.CodeAnalysis;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace ModularPipelines.Distributed.Extensions;

/// <summary>
/// Extension methods for configuring distributed pipeline mode.
/// </summary>
public static class DistributedPipelineBuilderExtensions
{
    /// <summary>
    /// Enables distributed execution mode. When <see cref="DistributedOptions.TotalInstances"/> is greater than 1,
    /// the pipeline switches to master/worker mode. Otherwise, execution remains in-process.
    /// </summary>
    /// <returns>The pipeline builder.</returns>
    [RequiresUnreferencedCode(
        "Distributed type-erased result serialization is unsupported in trimmed applications.")]
    [RequiresDynamicCode(
        "Distributed type-erased result serialization is unsupported in Native AOT.")]
    public static PipelineBuilder AddDistributedMode(this PipelineBuilder builder, Action<DistributedOptions> configure)
    {
        builder.Services.Configure<DistributedOptions>(o =>
        {
            configure(o);
            o.Enabled = true;
        });

        return builder;
    }

    /// <summary>
    /// Enables distributed execution mode from configuration.
    /// </summary>
    /// <returns>The pipeline builder.</returns>
    [RequiresUnreferencedCode("Configuration binding requires members of DistributedOptions that cannot be statically discovered.")]
    [RequiresDynamicCode("Configuration binding may require runtime code generation.")]
    public static PipelineBuilder AddDistributedMode(this PipelineBuilder builder, IConfigurationSection section)
    {
        builder.Services.Configure<DistributedOptions>(section);

        // Also ensure Enabled is set
        builder.Services.PostConfigure<DistributedOptions>(o => o.Enabled = true);
        return builder;
    }

    /// <summary>
    /// Registers a custom distributed coordinator implementation.
    /// </summary>
    /// <returns>The pipeline builder.</returns>
    public static PipelineBuilder AddDistributedCoordinator<
        [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] TCoordinator>(
        this PipelineBuilder builder)
        where TCoordinator : class, IDistributedCoordinator
    {
        builder.Services.AddSingleton<IDistributedCoordinator, TCoordinator>();
        return builder;
    }

    /// <summary>
    /// Registers a distributed coordinator factory for async initialization.
    /// </summary>
    /// <returns>The pipeline builder.</returns>
    public static PipelineBuilder AddDistributedCoordinatorFactory<
        [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] TFactory>(
        this PipelineBuilder builder)
        where TFactory : class, IDistributedCoordinatorFactory
    {
        builder.Services.AddSingleton<IDistributedCoordinatorFactory, TFactory>();
        return builder;
    }

    /// <summary>
    /// Registers a distributed artifact store implementation.
    /// </summary>
    /// <returns>The pipeline builder.</returns>
    public static PipelineBuilder AddDistributedArtifactStore<
        [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] TStore>(
        this PipelineBuilder builder)
        where TStore : class, IDistributedArtifactStore
    {
        builder.Services.RemoveAll<IDistributedArtifactStoreFactory>();
        builder.Services.AddSingleton<IDistributedArtifactStore, TStore>();
        return builder;
    }

    /// <summary>
    /// Registers a distributed artifact store factory for async initialization.
    /// </summary>
    /// <returns>The pipeline builder.</returns>
    public static PipelineBuilder AddDistributedArtifactStoreFactory<
        [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] TFactory>(
        this PipelineBuilder builder)
        where TFactory : class, IDistributedArtifactStoreFactory
    {
        builder.Services.AddSingleton<IDistributedArtifactStoreFactory, TFactory>();
        return builder;
    }
}
