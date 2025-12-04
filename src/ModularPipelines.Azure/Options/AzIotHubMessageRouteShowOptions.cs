using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("iot", "hub", "message-route", "show")]
public record AzIotHubMessageRouteShowOptions(
[property: CliOption("--hub-name")] string HubName,
[property: CliOption("--rn")] string Rn
) : AzOptions
{
    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }
}