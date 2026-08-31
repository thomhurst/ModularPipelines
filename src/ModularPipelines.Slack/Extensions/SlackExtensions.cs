using System.Diagnostics.CodeAnalysis;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using ModularPipelines.Attributes;
using ModularPipelines.Context;

namespace ModularPipelines.Slack.Extensions;

[ExcludeFromCodeCoverage]
public static class SlackExtensions
{
    [ModularPipelinesIntegration]
    public static IServiceCollection RegisterSlackContext(this IServiceCollection services)
    {
        services.TryAddScoped<ISlack, Slack>();

        return services;
    }
}
