using ModularPipelines.MicrosoftTeams.Models;

namespace ModularPipelines.MicrosoftTeams.Options;

public record MicrosoftTeamsWebHookCardOptions
(
    MicrosoftTeamsAdaptiveCard Card,
    Uri WebHookUri
);