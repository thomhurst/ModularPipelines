using System.Diagnostics.CodeAnalysis;

namespace ModularPipelines.MicrosoftTeams;

[ExcludeFromCodeCoverage]
public record MicrosoftTeamsWebHookCardOptions
(
    MicrosoftTeamsAdaptiveCard Card,
    Uri WebHookUri
)
{
    /// <summary>
    /// Gets whether an unsuccessful webhook response throws. Defaults to true.
    /// Set to false to inspect the returned response status and body instead.
    /// </summary>
    public bool ThrowOnNonSuccessStatusCode { get; init; } = true;
}