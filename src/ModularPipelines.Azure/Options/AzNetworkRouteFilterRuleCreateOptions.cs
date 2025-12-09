using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("network", "route-filter", "rule", "create")]
public record AzNetworkRouteFilterRuleCreateOptions(
[property: CliOption("--filter-name")] string FilterName,
[property: CliOption("--name")] string Name,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CliOption("--access")]
    public string? Access { get; set; }

    [CliOption("--communities")]
    public string? Communities { get; set; }

    [CliOption("--location")]
    public string? Location { get; set; }

    [CliFlag("--no-wait")]
    public bool? NoWait { get; set; }
}