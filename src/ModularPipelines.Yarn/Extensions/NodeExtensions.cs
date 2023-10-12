using System.Runtime.CompilerServices;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using ModularPipelines.Context;
using ModularPipelines.Engine;

namespace ModularPipelines.Yarn.Extensions;

public static class YarnExtensions
{
#pragma warning disable CA2255
    [ModuleInitializer]
#pragma warning restore CA2255
    public static void RegisterYarnContext()
    {
        ModularPipelinesContextRegistry.RegisterContext(collection => RegisterYarnContext(collection));
    }

    public static IServiceCollection RegisterYarnContext(this IServiceCollection services)
    {
        services.TryAddScoped<IYarn, Yarn>();
        return services;
    }

    public static IYarn Yarn(this IPipelineHookContext context) => context.ServiceProvider.GetRequiredService<IYarn>();
}
