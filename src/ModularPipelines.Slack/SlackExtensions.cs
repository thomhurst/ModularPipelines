using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using ModularPipelines.Attributes;

namespace ModularPipelines.Slack;

[ExcludeFromCodeCoverage]
public static class SlackExtensions
{
    [EditorBrowsable(EditorBrowsableState.Never)]
    [ModularPipelinesIntegration]
    public static IServiceCollection RegisterSlackContext(this IServiceCollection services)
    {
        services.TryAddScoped<ISlack, Slack>();

        return services;
    }
}
