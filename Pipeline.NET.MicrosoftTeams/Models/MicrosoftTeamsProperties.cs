using System.Text.Json.Serialization;

namespace Pipeline.NET.MicrosoftTeams.Models;

public class MicrosoftTeamsProperties
{
    [JsonPropertyName("width")]
    public string? Width { get; set; }
}