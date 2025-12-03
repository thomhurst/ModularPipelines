using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("iot", "hub", "message-route", "list")]
public record AzIotHubMessageRouteListOptions(
[property: CliOption("--hub-name")] string HubName
) : AzOptions
{
    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CliOption("--source-type")]
    public string? SourceType { get; set; }
}