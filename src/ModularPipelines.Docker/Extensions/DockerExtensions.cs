using System.Runtime.CompilerServices;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using ModularPipelines.Context;
using ModularPipelines.Engine;

namespace ModularPipelines.Docker.Extensions;

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
        services.TryAddTransient<IDocker, Docker>();
        return services;
    }

    public static IDocker Docker(this IModuleContext context) => context.ServiceProvider.GetRequiredService<IDocker>();
}
