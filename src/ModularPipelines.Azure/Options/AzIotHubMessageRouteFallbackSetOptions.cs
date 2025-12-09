using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("iot", "hub", "message-route", "fallback", "set")]
public record AzIotHubMessageRouteFallbackSetOptions(
[property: CliFlag("--enabled")] bool Enabled,
[property: CliOption("--hub-name")] string HubName
) : AzOptions
{
    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }
}