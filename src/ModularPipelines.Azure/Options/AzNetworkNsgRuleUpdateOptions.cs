using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("network", "nsg", "rule", "update")]
public record AzNetworkNsgRuleUpdateOptions : AzOptions
{
    [CommandSwitch("--access")]
    public string? Access { get; set; }

    [CommandSwitch("--add")]
    public string? Add { get; set; }

    [CommandSwitch("--description")]
    public string? Description { get; set; }

    [CommandSwitch("--destination-address-prefixes")]
    public string? DestinationAddressPrefixes { get; set; }

    [CommandSwitch("--destination-asgs")]
    public string? DestinationAsgs { get; set; }

    [CommandSwitch("--destination-port-ranges")]
    public string? DestinationPortRanges { get; set; }

    [CommandSwitch("--direction")]
    public string? Direction { get; set; }

    [BooleanCommandSwitch("--force-string")]
    public bool? ForceString { get; set; }

    [CommandSwitch("--ids")]
    public string? Ids { get; set; }

    [CommandSwitch("--name")]
    public string? Name { get; set; }

    [BooleanCommandSwitch("--no-wait")]
    public bool? NoWait { get; set; }

    [CommandSwitch("--nsg-name")]
    public string? NsgName { get; set; }

    [CommandSwitch("--priority")]
    public string? Priority { get; set; }

    [CommandSwitch("--protocol")]
    public string? Protocol { get; set; }

    [CommandSwitch("--remove")]
    public string? Remove { get; set; }

    [CommandSwitch("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CommandSwitch("--set")]
    public string? Set { get; set; }

    [CommandSwitch("--source-address-prefixes")]
    public string? SourceAddressPrefixes { get; set; }

    [CommandSwitch("--source-asgs")]
    public string? SourceAsgs { get; set; }

    [CommandSwitch("--source-port-ranges")]
    public string? SourcePortRanges { get; set; }

    [CommandSwitch("--subscription")]
    public new string? Subscription { get; set; }
}