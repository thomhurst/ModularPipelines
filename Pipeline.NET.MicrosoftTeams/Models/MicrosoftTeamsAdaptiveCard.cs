using System.Text.Json.Serialization;
using AdaptiveCards;

namespace Pipeline.NET.MicrosoftTeams.Models;

public class MicrosoftTeamsAdaptiveCard : AdaptiveCard
{
    public MicrosoftTeamsAdaptiveCard() : base(new AdaptiveSchemaVersion(1, 3))
    {
            
    }
        
    [JsonPropertyName("msTeams")]
    public MicrosoftTeamsProperties? MsTeams { get; set; }
}