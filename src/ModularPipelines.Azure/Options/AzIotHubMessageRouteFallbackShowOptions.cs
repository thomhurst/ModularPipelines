using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("iot", "hub", "message-route", "fallback", "show")]
public record AzIotHubMessageRouteFallbackShowOptions(
[property: CliOption("--hub-name")] string HubName
) : AzOptions
{
    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }
}