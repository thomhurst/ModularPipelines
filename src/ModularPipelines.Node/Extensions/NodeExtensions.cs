using System.Runtime.CompilerServices;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using ModularPipelines.Context;
using ModularPipelines.Engine;

namespace ModularPipelines.Node.Extensions;

public static class NodeExtensions
{
#pragma warning disable CA2255
    [ModuleInitializer]
#pragma warning restore CA2255
    public static void RegisterNodeContext()
    {
        ModularPipelinesContextRegistry.RegisterContext(collection => RegisterNodeContext(collection));
    }

    public static IServiceCollection RegisterNodeContext(this IServiceCollection services)
    {
        services.TryAddScoped<INode, Node>();
        services.TryAddScoped<INvm, Nvm>();
        services.TryAddScoped<INpm, Npm>();
        services.TryAddScoped<INpx, Npx>();
        return services;
    }

    public static INode Node(this IPipelineHookContext context) => context.ServiceProvider.GetRequiredService<INode>();
}