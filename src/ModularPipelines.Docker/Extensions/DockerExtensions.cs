using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using ModularPipelines.Context;
using ModularPipelines.Docker.Services;
using ModularPipelines.Engine;

namespace ModularPipelines.Docker.Extensions;

[ExcludeFromCodeCoverage]
public static class DockerExtensions
{
#pragma warning disable CA2255
    [ModuleInitializer]
#pragma warning restore CA2255
    public static void RegisterDockerContext()
    {
        ModularPipelinesContextRegistry.RegisterContext(collection => RegisterDockerContext(collection));
    }

    public static IServiceCollection RegisterDockerContext(this IServiceCollection services)
    {
        services.TryAddScoped<IDocker, Services.Docker>();
        services.TryAddScoped<DockerBuilder>();
        services.TryAddScoped<DockerBuildx>();
        services.TryAddScoped<DockerBuildxImagetools>();
        services.TryAddScoped<DockerCheckpoint>();
        services.TryAddScoped<DockerCompose>();
        services.TryAddScoped<DockerComposeAlpha>();
        services.TryAddScoped<DockerConfig>();
        services.TryAddScoped<DockerContainer>();
        services.TryAddScoped<DockerContext>();
        services.TryAddScoped<DockerImage>();
        services.TryAddScoped<DockerManifest>();
        services.TryAddScoped<DockerNetwork>();
        services.TryAddScoped<DockerNode>();
        services.TryAddScoped<DockerPlugin>();
        services.TryAddScoped<DockerScout>();
        services.TryAddScoped<DockerScoutCache>();
        services.TryAddScoped<DockerScoutIntegration>();
        services.TryAddScoped<DockerScoutRepo>();
        services.TryAddScoped<DockerSecret>();
        services.TryAddScoped<DockerService>();
        services.TryAddScoped<DockerStack>();
        services.TryAddScoped<DockerSwarm>();
        services.TryAddScoped<DockerSystem>();
        services.TryAddScoped<DockerTrust>();
        services.TryAddScoped<DockerTrustKey>();
        services.TryAddScoped<DockerTrustSigner>();
        services.TryAddScoped<DockerVolume>();

        return services;
    }

    public static IDocker Docker(this IPipelineHookContext context) => context.ServiceProvider.GetRequiredService<IDocker>();
}