using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("iot", "hub", "message-route", "delete")]
public record AzIotHubMessageRouteDeleteOptions(
[property: CliOption("--hub-name")] string HubName
) : AzOptions
{
    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CliOption("--rn")]
    public string? Rn { get; set; }

    [CliOption("--source-type")]
    public string? SourceType { get; set; }

    [CliFlag("--yes")]
    public bool? Yes { get; set; }
}