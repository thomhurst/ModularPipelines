using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using ModularPipelines.Context;
using ModularPipelines.Engine;

namespace ModularPipelines.Slack.Extensions;

[ExcludeFromCodeCoverage]
public static class SlackExtensions
{
#pragma warning disable CA2255
    [ModuleInitializer]
#pragma warning restore CA2255
    public static void RegisterSlackContext()
    {
        ModularPipelinesContextRegistry.RegisterContext(collection => RegisterSlackContext(collection));
    }

    public static IServiceCollection RegisterSlackContext(this IServiceCollection services)
    {
        services.TryAddScoped<ISlack, Slack>();

        return services;
    }

    public static ISlack Slack(this IPipelineHookContext context) => context.ServiceProvider.GetRequiredService<ISlack>();
}