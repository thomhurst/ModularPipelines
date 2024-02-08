using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;

namespace ModularPipelines.MicrosoftTeams.Models;

[ExcludeFromCodeCoverage]
internal class MicrosoftTeamsCardWrapper
{
    [JsonPropertyName("type")]
    public string? Type { get; set; }

    [JsonPropertyName("attachments")]
    public Attachment[]? Attachments { get; set; }

    public static MicrosoftTeamsCardWrapper Wrap(MicrosoftTeamsAdaptiveCard adaptiveCard)
    {
        return new MicrosoftTeamsCardWrapper
        {
            Type = "message",
            Attachments =
            [
                new Attachment
                {
                    ContentType = "application/vnd.microsoft.card.adaptive",
                    Content = adaptiveCard,
                }
            ],
        };
    }
}