using System.Runtime.CompilerServices;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using ModularPipelines.Context;
using ModularPipelines.Engine;

namespace ModularPipelines.Helm.Extensions;

public static class HelmExtensions
{
#pragma warning disable CA2255
    [ModuleInitializer]
#pragma warning restore CA2255
    public static void RegisterHelmContext()
    {
        ModularPipelinesContextRegistry.RegisterContext(collection => RegisterHelmContext(collection));
    }

    public static IServiceCollection RegisterHelmContext(this IServiceCollection services)
    {
        services.TryAddTransient<IHelm, Helm>();
        return services;
    }

    public static IHelm Helm(this IPipelineContext context) => context.ServiceProvider.GetRequiredService<IHelm>();
}
