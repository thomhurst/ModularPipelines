using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace ModularPipelines.MicrosoftTeams;

[ExcludeFromCodeCoverage]
public sealed record MicrosoftTeamsWebHookCardOptions
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

    private bool PrintMembers(StringBuilder builder)
    {
        builder.Append("Card = ").Append(Card);
        builder.Append(", WebHookUri = <redacted>");
        builder.Append(", ThrowOnNonSuccessStatusCode = ").Append(ThrowOnNonSuccessStatusCode);
        return true;
    }
}
