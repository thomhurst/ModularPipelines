using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using ModularPipelines.Attributes;
using ModularPipelines.Context;

namespace ModularPipelines.Node.Extensions;

public static class NodeExtensions
{
    [ModularPipelinesIntegration]
    public static IServiceCollection RegisterNodeContext(this IServiceCollection services)
    {
        services.TryAddScoped<INode, Node>();
        services.TryAddScoped<INvm, Nvm>();
        services.TryAddScoped<INpm, Npm>();
        services.TryAddScoped<INpx, Npx>();
        return services;
    }

    [global::System.ComponentModel.EditorBrowsable(global::System.ComponentModel.EditorBrowsableState.Never)]

    [global::System.Obsolete("Use context.Tools.Get<INode>().")]

    public static INode Node(this IPipelineContext context) => context.Services.GetRequiredService<INode>();
}
