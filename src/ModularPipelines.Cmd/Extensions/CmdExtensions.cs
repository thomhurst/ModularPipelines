using System.Diagnostics.CodeAnalysis;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using ModularPipelines.Attributes;
using ModularPipelines.Context;

namespace ModularPipelines.Cmd.Extensions;

/// <summary>
/// Registers and resolves the Windows Command Prompt integration.
/// </summary>
[ExcludeFromCodeCoverage]
public static class CmdExtensions
{
    /// <summary>
    /// Registers the Command Prompt integration.
    /// </summary>
    /// <param name="services">The collection that receives the Command Prompt registration.</param>
    /// <returns>The same collection, for further service registrations.</returns>
    [ModularPipelinesIntegration]
    public static IServiceCollection RegisterCmdContext(this IServiceCollection services)
    {
        services.TryAddScoped<ICmdContext, CmdContext>();

        return services;
    }

    /// <summary>
    /// Gets the Command Prompt integration from a pipeline context.
    /// </summary>
    /// <param name="context">The pipeline context.</param>
    /// <returns>The Command Prompt context.</returns>
    public static ICmdContext Cmd(this IPipelineContext context) => context.Services.Get<ICmdContext>();
}
