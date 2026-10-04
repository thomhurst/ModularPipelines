using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;

namespace ModularPipelines.MicrosoftTeams;

[ExcludeFromCodeCoverage]
public class MicrosoftTeamsProperties
{
    [JsonPropertyName("width")]
    public string? Width { get; set; }
}