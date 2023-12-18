using System.Diagnostics.CodeAnalysis;
using ModularPipelines.MicrosoftTeams.Models;

namespace ModularPipelines.MicrosoftTeams.Options;

[ExcludeFromCodeCoverage]
public record MicrosoftTeamsWebHookCardOptions
(
    MicrosoftTeamsAdaptiveCard Card,
    Uri WebHookUri
);