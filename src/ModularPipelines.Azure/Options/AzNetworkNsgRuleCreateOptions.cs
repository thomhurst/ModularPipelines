using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("network", "nsg", "rule", "create")]
public record AzNetworkNsgRuleCreateOptions(
[property: CliOption("--name")] string Name,
[property: CliOption("--nsg-name")] string NsgName,
[property: CliOption("--priority")] string Priority,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CliOption("--access")]
    public string? Access { get; set; }

    [CliOption("--description")]
    public string? Description { get; set; }

    [CliOption("--destination-address-prefixes")]
    public string? DestinationAddressPrefixes { get; set; }

    [CliOption("--destination-asgs")]
    public string? DestinationAsgs { get; set; }

    [CliOption("--destination-port-ranges")]
    public string? DestinationPortRanges { get; set; }

    [CliOption("--direction")]
    public string? Direction { get; set; }

    [CliFlag("--no-wait")]
    public bool? NoWait { get; set; }

    [CliOption("--protocol")]
    public string? Protocol { get; set; }

    [CliOption("--source-address-prefixes")]
    public string? SourceAddressPrefixes { get; set; }

    [CliOption("--source-asgs")]
    public string? SourceAsgs { get; set; }

    [CliOption("--source-port-ranges")]
    public string? SourcePortRanges { get; set; }
}