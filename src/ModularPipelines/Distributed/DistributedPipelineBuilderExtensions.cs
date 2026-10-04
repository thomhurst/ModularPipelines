using System.Diagnostics.CodeAnalysis;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using ModularPipelines.Distributed.Configuration;

namespace ModularPipelines.Distributed;

/// <summary>
/// Extension methods for configuring distributed pipeline mode.
/// </summary>
public static class DistributedPipelineBuilderExtensions
{
    private const string InstanceIndexEnvironmentVariable = "MODULARPIPELINES_INSTANCE_INDEX";
    private const string TotalInstancesEnvironmentVariable = "MODULARPIPELINES_TOTAL_INSTANCES";
    private const string MaxParallelismEnvironmentVariable = "MODULARPIPELINES_MAX_PARALLELISM";
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
            options.MaxParallelism = GetOptionalEnvironmentInt32(
                MaxParallelismEnvironmentVariable,
                options.MaxParallelism,
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

    private static int? GetOptionalEnvironmentInt32(string name, int? defaultValue, int minimum)
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
    /// Registers a custom distributed coordinator implementation. Register exactly one coordinator
    /// backend; building the pipeline fails when several are registered. The coordinator is created
    /// by dependency injection and disposed with the host when it implements <see cref="IAsyncDisposable"/>
    /// or <see cref="IDisposable"/>.
    /// </summary>
    /// <returns>The pipeline builder.</returns>
    public static PipelineBuilder AddDistributedCoordinator<
        [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] TCoordinator>(
        this PipelineBuilder builder)
        where TCoordinator : class, IDistributedMasterCoordinator
    {
        builder.Services.AddSingleton(new DistributedCoordinatorBackendRegistration(typeof(TCoordinator)));
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
    /// Registers a distributed coordinator factory for coordinators that need asynchronous setup.
    /// Registering the same factory again is a no-op; registering a different coordinator backend throws.
    /// Coordinators created by the factory are disposed with the host when they implement
    /// <see cref="IAsyncDisposable"/> or <see cref="IDisposable"/>.
    /// </summary>
    /// <returns>The pipeline builder.</returns>
    /// <exception cref="InvalidOperationException">Another coordinator factory is already registered.</exception>
    public static PipelineBuilder AddDistributedCoordinatorFactory<
        [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] TFactory>(
        this PipelineBuilder builder)
        where TFactory : class, IDistributedCoordinatorFactory
    {
        if (IsBackendFactoryRegistered<IDistributedCoordinatorFactory, TFactory>(builder.Services, "coordinator"))
        {
            return builder;
        }

        builder.Services.AddSingleton<IDistributedCoordinatorFactory, TFactory>();
        return builder;
    }

    /// <summary>
    /// Registers a distributed artifact store implementation. Repeating the same registration is
    /// a no-op; registering a different store or a factory throws.
    /// </summary>
    /// <returns>The pipeline builder.</returns>
    /// <exception cref="InvalidOperationException">Another artifact store backend is already registered.</exception>
    public static PipelineBuilder AddDistributedArtifactStore<
        [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] TStore>(
        this PipelineBuilder builder)
        where TStore : class, IDistributedArtifactStore
    {
        if (IsArtifactBackendRegistered<IDistributedArtifactStore, TStore>(builder.Services))
        {
            return builder;
        }

        builder.Services.AddSingleton<IDistributedArtifactStore, TStore>();
        return builder;
    }

    /// <summary>
    /// Registers a distributed artifact store factory for async initialization. Registering the same
    /// factory again is a no-op; registering a different factory or a direct store throws.
    /// </summary>
    /// <returns>The pipeline builder.</returns>
    /// <exception cref="InvalidOperationException">Another artifact store backend is already registered.</exception>
    public static PipelineBuilder AddDistributedArtifactStoreFactory<
        [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] TFactory>(
        this PipelineBuilder builder)
        where TFactory : class, IDistributedArtifactStoreFactory
    {
        if (IsArtifactBackendRegistered<IDistributedArtifactStoreFactory, TFactory>(builder.Services))
        {
            return builder;
        }

        builder.Services.AddSingleton<IDistributedArtifactStoreFactory, TFactory>();
        return builder;
    }

    private static bool IsArtifactBackendRegistered<TService, TImplementation>(IServiceCollection services)
    {
        var registered = false;
        foreach (var descriptor in services)
        {
            if (descriptor.IsKeyedService
                || (descriptor.ServiceType != typeof(IDistributedArtifactStore)
                    && descriptor.ServiceType != typeof(IDistributedArtifactStoreFactory)))
            {
                continue;
            }

            if (descriptor.ServiceType == typeof(TService)
                && descriptor.ImplementationType == typeof(TImplementation))
            {
                registered = true;
                continue;
            }

            var existingName = descriptor.ImplementationType?.FullName ?? "a factory delegate or instance";
            throw new InvalidOperationException(
                $"A distributed artifact store backend ({existingName}) is already registered, so {typeof(TImplementation).FullName} " +
                "cannot also be registered. Register exactly one distributed artifact store backend.");
        }

        return registered;
    }

    private static bool IsBackendFactoryRegistered<TService, TFactory>(IServiceCollection services, string backend)
    {
        var existing = services.LastOrDefault(descriptor =>
            descriptor.ServiceType == typeof(TService) && !descriptor.IsKeyedService);
        if (existing is null)
        {
            return false;
        }

        if (existing.ImplementationType == typeof(TFactory))
        {
            return true;
        }

        var existingName = existing.ImplementationType?.FullName ?? "a factory delegate or instance";
        throw new InvalidOperationException(
            $"A distributed {backend} backend ({existingName}) is already registered, so {typeof(TFactory).FullName} " +
            $"cannot also be registered. Register exactly one distributed {backend} backend.");
    }

    /// <summary>
    /// Requires one explicit shared <see cref="DistributedOptions.RunId"/> for the run and
    /// validates it at startup, rejecting the generated single-instance fallback. Backends that
    /// key shared state by run identifier call this so a missing identifier fails fast instead
    /// of silently isolating each process. The requirement is also recorded as a service that
    /// run identifier resolution consults, so an options binding registered later (for example
    /// <see cref="AddDistributedMode(PipelineBuilder, IConfigurationSection)"/> with
    /// <c>RequireExplicitRunId: false</c>) cannot switch it off.
    /// </summary>
    /// <returns>The pipeline builder.</returns>
    public static PipelineBuilder RequireExplicitRunId(this PipelineBuilder builder)
    {
        builder.Services.Configure<DistributedOptions>(options => options.RequireExplicitRunId = true);
        builder.Services.AddSingleton(ExplicitRunIdRequirement.Instance);
        builder.Services.AddOptions<DistributedOptions>().ValidateOnStart();
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

/// <summary>
/// Records a coordinator backend registered with <see cref="DistributedPipelineBuilderExtensions.AddDistributedCoordinator{TCoordinator}"/>.
/// </summary>
internal sealed record DistributedCoordinatorBackendRegistration(Type BackendType);
