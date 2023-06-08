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
        ServiceContextRegistry.RegisterContext(collection => RegisterNodeContext(collection));
    }
    
    public static IServiceCollection RegisterNodeContext(this IServiceCollection services)
    {
        services.TryAddSingleton(typeof(INode<>), typeof(Node<>));
        services.TryAddSingleton(typeof(INvm<>), typeof(Nvm<>));
        services.TryAddSingleton(typeof(INpm<>), typeof(Npm<>));
        return services;
    }
    
    public static INode Node(this IModuleContext context) => (INode) context.ServiceProvider.GetRequiredService(typeof(INode<>).MakeGenericType(context.ModuleType));
}