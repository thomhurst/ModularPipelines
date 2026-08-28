using System.Diagnostics.CodeAnalysis;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using ModularPipelines.Attributes;
using ModularPipelines.Context;

namespace ModularPipelines.MicrosoftTeams.Extensions;

[ExcludeFromCodeCoverage]
public static class MicrosoftTeamsExtensions
{
    [ModularPipelinesIntegration]
    public static IServiceCollection RegisterMicrosoftTeamsContext(this IServiceCollection services)
    {
        services.TryAddScoped<IMicrosoftTeams, MicrosoftTeams>();
        return services;
    }

    [global::System.ComponentModel.EditorBrowsable(global::System.ComponentModel.EditorBrowsableState.Never)]

    [global::System.Obsolete("Use context.Tools.MicrosoftTeams.")]

    public static IMicrosoftTeams MicrosoftTeams(this IPipelineContext context) => context.Services.GetRequiredService<IMicrosoftTeams>();
}
