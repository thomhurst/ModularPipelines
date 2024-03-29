using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using ModularPipelines.Context;
using ModularPipelines.Engine;

namespace ModularPipelines.Cmd.Extensions;

[ExcludeFromCodeCoverage]
public static class CmdExtensions
{
#pragma warning disable CA2255
    [ModuleInitializer]
#pragma warning restore CA2255
    public static void RegisterCmdContext()
    {
        ModularPipelinesContextRegistry.RegisterContext(collection => RegisterCmdContext(collection));
    }

    public static IServiceCollection RegisterCmdContext(this IServiceCollection services)
    {
        services.TryAddScoped<ICmd, Cmd>();

        return services;
    }

    public static ICmd Cmd(this IPipelineHookContext context) => context.ServiceProvider.GetRequiredService<ICmd>();
}