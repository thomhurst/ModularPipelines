using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using ModularPipelines.Context;
using ModularPipelines.Engine;

namespace ModularPipelines.WinGet.Extensions;

[ExcludeFromCodeCoverage]
public static class WinGetExtensions
{
#pragma warning disable CA2255
    [ModuleInitializer]
#pragma warning restore CA2255
    public static void RegisterWinGetContext()
    {
        ModularPipelinesContextRegistry.RegisterContext(collection => RegisterWinGetContext(collection));
    }

    public static IServiceCollection RegisterWinGetContext(this IServiceCollection services)
    {
        services.TryAddScoped<IWinGet, WinGet>();

        return services;
    }

    public static IWinGet WinGet(this IPipelineHookContext context) => context.ServiceProvider.GetRequiredService<IWinGet>();
}
