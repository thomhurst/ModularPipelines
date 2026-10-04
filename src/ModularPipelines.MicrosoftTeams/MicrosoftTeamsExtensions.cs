using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using ModularPipelines.Attributes;

namespace ModularPipelines.MicrosoftTeams;

[ExcludeFromCodeCoverage]
public static class MicrosoftTeamsExtensions
{
    [EditorBrowsable(EditorBrowsableState.Never)]
    [ModularPipelinesIntegration]
    public static IServiceCollection RegisterMicrosoftTeamsContext(this IServiceCollection services)
    {
        services.TryAddScoped<IMicrosoftTeams, MicrosoftTeams>();
        return services;
    }
}
