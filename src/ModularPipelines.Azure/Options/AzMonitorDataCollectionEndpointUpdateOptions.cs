using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("monitor", "data-collection", "endpoint", "update")]
public record AzMonitorDataCollectionEndpointUpdateOptions : AzOptions
{
    [CliOption("--description")]
    public string? Description { get; set; }

    [CliOption("--ids")]
    public string? Ids { get; set; }

    [CliOption("--kind")]
    public string? Kind { get; set; }

    [CliOption("--name")]
    public string? Name { get; set; }

    [CliOption("--public-network-access")]
    public string? PublicNetworkAccess { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CliOption("--subscription")]
    public new string? Subscription { get; set; }

    [CliOption("--tags")]
    public string? Tags { get; set; }
}