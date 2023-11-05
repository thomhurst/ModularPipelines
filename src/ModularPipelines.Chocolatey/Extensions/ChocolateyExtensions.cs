using System.Runtime.CompilerServices;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using ModularPipelines.Context;
using ModularPipelines.Engine;

namespace ModularPipelines.Chocolatey.Extensions;

public static class ChocolateyExtensions
{
#pragma warning disable CA2255
    [ModuleInitializer]
#pragma warning restore CA2255
    public static void RegisterChocolateyContext()
    {
        ModularPipelinesContextRegistry.RegisterContext(collection => RegisterChocolateyContext(collection));
    }
    
    public static IServiceCollection RegisterChocolateyContext(this IServiceCollection services)
    {
        services.TryAddScoped<IChoco, Choco>();
        return services;
    }

    public static IChoco Choco(this IPipelineHookContext context) => context.ServiceProvider.GetRequiredService<IChoco>();
}
