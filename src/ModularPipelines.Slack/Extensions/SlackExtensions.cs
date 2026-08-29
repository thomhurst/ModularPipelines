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

    [global::System.ComponentModel.EditorBrowsable(global::System.ComponentModel.EditorBrowsableState.Never)]

    [global::System.Obsolete("Use context.Tools.Get<ISlack>().")]

    public static ISlack Slack(this IPipelineContext context) => context.Services.GetRequiredService<ISlack>();
}
