using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using ModularPipelines.Context;
using ModularPipelines.Docker.Services;
using ModularPipelines.Engine;

namespace ModularPipelines.Docker.Extensions;

/// <summary>
/// Provides extension methods for integrating Docker functionality into the ModularPipelines framework.
/// </summary>
[ExcludeFromCodeCoverage]
public static class DockerExtensions
{
    /// <summary>
    /// Automatically registers the Docker context services with the ModularPipelines framework.
    /// This method is called by the module initializer and should not be called directly.
    /// </summary>
#pragma warning disable CA2255
    [ModuleInitializer]
#pragma warning restore CA2255
    public static void RegisterDockerContext()
    {
        ModularPipelinesContextRegistry.RegisterContext(collection => RegisterDockerContext(collection));
    }

    /// <summary>
    /// Registers Docker services with the dependency injection container.
    /// This includes services for running Docker commands.
    /// </summary>
    /// <param name="services">The service collection to add the Docker services to.</param>
    /// <returns>The service collection for method chaining.</returns>
    public static IServiceCollection RegisterDockerContext(this IServiceCollection services)
    {
        services.TryAddScoped<IDocker, Services.Docker>();
        services.TryAddScoped<DockerBuilder>();
        services.TryAddScoped<DockerBuildx>();
        services.TryAddScoped<DockerCompose>();
        services.TryAddScoped<DockerContainer>();
        services.TryAddScoped<DockerContext>();
        services.TryAddScoped<DockerImage>();
        services.TryAddScoped<DockerManifest>();
        services.TryAddScoped<DockerNetwork>();
        services.TryAddScoped<DockerPlugin>();
        services.TryAddScoped<DockerSwarm>();
        services.TryAddScoped<DockerSystem>();
        services.TryAddScoped<DockerTrust>();
        services.TryAddScoped<DockerVolume>();
        return services;
    }

    /// <summary>
    /// Gets the Docker service from the pipeline context.
    /// This provides access to Docker commands.
    /// </summary>
    /// <param name="context">The pipeline context.</param>
    /// <returns>The <see cref="IDocker"/> service for executing Docker commands.</returns>
    public static IDocker Docker(this IPipelineContext context) => context.Services.Get<IDocker>();
}
