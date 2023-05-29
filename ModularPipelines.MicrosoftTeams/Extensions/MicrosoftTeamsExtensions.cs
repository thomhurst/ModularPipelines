using ModularPipelines.Context;

namespace ModularPipelines.MicrosoftTeams.Extensions;

public static class MicrosoftTeamsExtensions
{
    public static IMicrosoftTeams MicrosoftTeams(this IModuleContext context) => context.Get<MicrosoftTeams>()!;
}