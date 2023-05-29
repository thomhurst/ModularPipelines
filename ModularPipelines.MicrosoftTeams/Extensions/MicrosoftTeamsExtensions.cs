using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using ModularPipelines.Context;

namespace ModularPipelines.MicrosoftTeams.Extensions;

public static class MicrosoftTeamsExtensions
{
    public static IServiceCollection RegisterMicrosoftTeamsContext(this IServiceCollection services)
    {
        services.TryAddSingleton<IMicrosoftTeams, MicrosoftTeams>();
        return services;
    }
    
    public static IMicrosoftTeams MicrosoftTeams(this IModuleContext context) => context.Get<IMicrosoftTeams>()!;
}