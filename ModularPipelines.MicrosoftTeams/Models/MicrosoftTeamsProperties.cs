using System.Text.Json.Serialization;

namespace ModularPipelines.MicrosoftTeams.Models;

public class MicrosoftTeamsProperties
{
    [JsonPropertyName( "width" )]
    public string? Width { get; set; }
}
