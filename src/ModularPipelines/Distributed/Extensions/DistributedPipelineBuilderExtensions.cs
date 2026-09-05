using System.Diagnostics.CodeAnalysis;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using ModularPipelines.Distributed.Configuration;

namespace ModularPipelines.Distributed.Extensions;

/// <summary>
/// Extension methods for configuring distributed pipeline mode.
/// </summary>
public static class DistributedPipelineBuilderExtensions
{
    private const string InstanceIndexEnvironmentVariable = "MODULARPIPELINES_INSTANCE_INDEX";
    private const string TotalInstancesEnvironmentVariable = "MODULARPIPELINES_TOTAL_INSTANCES";
    private const string RoleEnvironmentVariable = "MODULARPIPELINES_ROLE";

    /// <summary>
    /// Enables distributed execution mode and reads settings from the standard
    /// <c>MODULARPIPELINES_*</c> environment variables.
    /// </summary>
    /// <returns>The pipeline builder.</returns>
    [RequiresUnreferencedCode(
        "Distributed type-erased result serialization is unsupported in trimmed applications.")]
    [RequiresDynamicCode(
        "Distributed type-erased result serialization is unsupported in Native AOT.")]
    public static PipelineBuilder AddDistributedMode(this PipelineBuilder builder)
    {
        return builder.AddDistributedMode(options =>
        {
            options.InstanceIndex = GetEnvironmentInt32(
                InstanceIndexEnvironmentVariable,
                options.InstanceIndex,
                minimum: 0);
            options.TotalInstances = GetEnvironmentInt32(
                TotalInstancesEnvironmentVariable,
                options.TotalInstances,
                minimum: 1);
            options.RunId = Environment.GetEnvironmentVariable(RunIdResolver.EnvironmentVariable)
                            ?? options.RunId;
            options.Role = GetEnvironmentRole(options.Role);
        });
    }

    /// <summary>
    /// Enables distributed execution mode.
    /// </summary>
    /// <returns>The pipeline builder.</returns>
    [RequiresUnreferencedCode(
        "Distributed type-erased result serialization is unsupported in trimmed applications.")]
    [RequiresDynamicCode(
        "Distributed type-erased result serialization is unsupported in Native AOT.")]
    public static PipelineBuilder AddDistributedMode(this PipelineBuilder builder, Action<DistributedOptions> configure)
    {
        builder.Services.TryAddSingleton<DistributedModeRegistration>();
        builder.Services.Configure<DistributedOptions>(o =>
            configure(o));
        builder.Services.PostConfigure<DistributedOptions>(EnableDistributedMode);

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
        builder.Services.TryAddSingleton<DistributedModeRegistration>();
        builder.Services.Configure<DistributedOptions>(section);

        // Also ensure Enabled is set
        builder.Services.PostConfigure<DistributedOptions>(EnableDistributedMode);
        return builder;
    }

    private static int GetEnvironmentInt32(string name, int defaultValue, int minimum)
    {
        var value = Environment.GetEnvironmentVariable(name);
        if (value is null)
        {
            return defaultValue;
        }

        if (!int.TryParse(value, out var parsed) || parsed < minimum)
        {
            throw new InvalidOperationException(
                $"Environment variable {name} must be an integer greater than or equal to {minimum}.");
        }

        return parsed;
    }

    private static DistributedRole GetEnvironmentRole(DistributedRole defaultValue)
    {
        var value = Environment.GetEnvironmentVariable(RoleEnvironmentVariable);
        if (value is null)
        {
            return defaultValue;
        }

        if (!Enum.TryParse<DistributedRole>(value, ignoreCase: true, out var role))
        {
            throw new InvalidOperationException(
                $"Environment variable {RoleEnvironmentVariable} must be Auto, Master, or Worker.");
        }

        return role;
    }

    /// <summary>
    /// Registers a custom distributed coordinator implementation.
    /// </summary>
    /// <returns>The pipeline builder.</returns>
    public static PipelineBuilder AddDistributedCoordinator<
        [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] TCoordinator>(
        this PipelineBuilder builder)
        where TCoordinator : class, IDistributedMasterCoordinator
    {
        builder.Services.RemoveAll<IDistributedMasterCoordinator>();
        builder.Services.RemoveAll<IDistributedWorkerCoordinator>();
        builder.Services.AddSingleton<TCoordinator>();
        builder.Services.AddSingleton<IDistributedMasterCoordinator>(serviceProvider =>
            serviceProvider.GetRequiredService<TCoordinator>());
        builder.Services.AddSingleton<IDistributedWorkerCoordinator>(serviceProvider =>
            serviceProvider.GetRequiredService<TCoordinator>());
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

    private static void EnableDistributedMode(DistributedOptions options)
    {
        options.Enabled = true;
    }
}

internal sealed class DistributedModeRegistration
{
}
