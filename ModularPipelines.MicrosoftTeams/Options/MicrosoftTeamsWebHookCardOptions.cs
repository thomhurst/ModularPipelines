using ModularPipelines.MicrosoftTeams.Models;

namespace ModularPipelines.MicrosoftTeams.Options;

public class MicrosoftTeamsWebHookCardOptions
{
    public MicrosoftTeamsAdaptiveCard Card { get; init; }
    public Uri WebHookUri { get; init; }
}