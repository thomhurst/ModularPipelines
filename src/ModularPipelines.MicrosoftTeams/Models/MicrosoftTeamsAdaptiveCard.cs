using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;
using AdaptiveCards;

namespace ModularPipelines.MicrosoftTeams.Models;

[ExcludeFromCodeCoverage]
public class MicrosoftTeamsAdaptiveCard : AdaptiveCard
{
    public MicrosoftTeamsAdaptiveCard() : base(new AdaptiveSchemaVersion(1, 3))
    {
    }

    [JsonPropertyName("msTeams")]
    public MicrosoftTeamsProperties? MsTeams { get; set; }
}