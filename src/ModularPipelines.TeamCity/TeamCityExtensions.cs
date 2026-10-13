using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using ModularPipelines.Attributes;
using ModularPipelines.Context;

namespace ModularPipelines.TeamCity;

[ExcludeFromCodeCoverage]
public static class TeamCityExtensions
{
    [ModularPipelinesIntegration]
    [EditorBrowsable(EditorBrowsableState.Never)]
    public static IServiceCollection RegisterTeamCityContext(this IServiceCollection services)
    {
        services.TryAddScoped<ITeamCity, TeamCity>();
        services.TryAddScoped<ITeamCityEnvironmentVariables, TeamCityEnvironmentVariables>();
        return services;
    }
}
