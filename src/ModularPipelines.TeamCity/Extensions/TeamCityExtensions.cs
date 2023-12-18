using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using ModularPipelines.Context;
using ModularPipelines.Engine;

namespace ModularPipelines.TeamCity.Extensions;

[ExcludeFromCodeCoverage]
public static class TeamCityExtensions
{
#pragma warning disable CA2255
    [ModuleInitializer]
#pragma warning restore CA2255
    public static void RegisterTeamCityContext()
    {
        ModularPipelinesContextRegistry.RegisterContext(collection => RegisterTeamCityContext(collection));
    }

    public static IServiceCollection RegisterTeamCityContext(this IServiceCollection services)
    {
        services.TryAddScoped<ITeamCity, TeamCity>();
        services.TryAddScoped<ITeamCityEnvironmentVariables, TeamCityEnvironmentVariables>();
        return services;
    }

    public static ITeamCity TeamCity(this IPipelineHookContext context) => context.ServiceProvider.GetRequiredService<ITeamCity>();
}