using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("network", "nsg", "rule", "update")]
public record AzNetworkNsgRuleUpdateOptions : AzOptions
{
    [CliOption("--access")]
    public string? Access { get; set; }

    [CliOption("--add")]
    public string? Add { get; set; }

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

    [CliFlag("--force-string")]
    public bool? ForceString { get; set; }

    [CliOption("--ids")]
    public string? Ids { get; set; }

    [CliOption("--name")]
    public string? Name { get; set; }

    [CliFlag("--no-wait")]
    public bool? NoWait { get; set; }

    [CliOption("--nsg-name")]
    public string? NsgName { get; set; }

    [CliOption("--priority")]
    public string? Priority { get; set; }

    [CliOption("--protocol")]
    public string? Protocol { get; set; }

    [CliOption("--remove")]
    public string? Remove { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CliOption("--set")]
    public string? Set { get; set; }

    [CliOption("--source-address-prefixes")]
    public string? SourceAddressPrefixes { get; set; }

    [CliOption("--source-asgs")]
    public string? SourceAsgs { get; set; }

    [CliOption("--source-port-ranges")]
    public string? SourcePortRanges { get; set; }

    [CliOption("--subscription")]
    public new string? Subscription { get; set; }
}