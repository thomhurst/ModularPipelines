using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using ModularPipelines.Context;
using ModularPipelines.DotNet.Parsers.Trx;
using ModularPipelines.DotNet.Services;
using ModularPipelines.Engine;

namespace ModularPipelines.DotNet.Extensions;

[ExcludeFromCodeCoverage]
public static class DotNetExtensions
{
#pragma warning disable CA2255
    [ModuleInitializer]
#pragma warning restore CA2255
    public static void RegisterDotNetContext()
    {
        ModularPipelinesContextRegistry.RegisterContext(collection => RegisterDotNetContext(collection));
    }

    public static IServiceCollection RegisterDotNetContext(this IServiceCollection services)
    {
        services.TryAddScoped<ITrxParser, TrxParser>();
        services.TryAddScoped<ITrx, Trx>();

        services.TryAddScoped<IDotNet, Services.DotNet>();
        services.TryAddScoped<DotNetAdd>();
        services.TryAddScoped<DotNetList>();
        services.TryAddScoped<DotNetNuget>();
        services.TryAddScoped<DotNetNugetAdd>();
        services.TryAddScoped<DotNetNugetDisable>();
        services.TryAddScoped<DotNetNugetEnable>();
        services.TryAddScoped<DotNetNugetList>();
        services.TryAddScoped<DotNetNugetRemove>();
        services.TryAddScoped<DotNetNugetUpdate>();
        services.TryAddScoped<DotNetRemove>();
        services.TryAddScoped<DotNetSdk>();
        services.TryAddScoped<DotNetTool>();
        services.TryAddScoped<DotNetWorkload>();

        return services;
    }

    public static IDotNet DotNet(this IPipelineHookContext context) => context.ServiceProvider.GetRequiredService<IDotNet>();

    public static ITrx Trx(this IPipelineHookContext context) => context.ServiceProvider.GetRequiredService<ITrx>();
}