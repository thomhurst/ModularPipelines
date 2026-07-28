using System.Diagnostics.CodeAnalysis;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using ModularPipelines.Attributes;
using ModularPipelines.Context;

namespace ModularPipelines.Cmd.Extensions;

[ExcludeFromCodeCoverage]
public static class CmdExtensions
{
    [ModularPipelinesIntegration]
    public static IServiceCollection RegisterCmdContext(this IServiceCollection services)
    {
        services.TryAddScoped<ICmd, Cmd>();

        return services;
    }

    public static ICmd Cmd(this IPipelineContext context) => context.Services.Get<ICmd>();
}