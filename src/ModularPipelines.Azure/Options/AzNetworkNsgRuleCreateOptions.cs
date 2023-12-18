using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("network", "nsg", "rule", "create")]
public record AzNetworkNsgRuleCreateOptions(
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--nsg-name")] string NsgName,
[property: CommandSwitch("--priority")] string Priority,
[property: CommandSwitch("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CommandSwitch("--access")]
    public string? Access { get; set; }

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

    [BooleanCommandSwitch("--no-wait")]
    public bool? NoWait { get; set; }

    [CommandSwitch("--protocol")]
    public string? Protocol { get; set; }

    [CommandSwitch("--source-address-prefixes")]
    public string? SourceAddressPrefixes { get; set; }

    [CommandSwitch("--source-asgs")]
    public string? SourceAsgs { get; set; }

    [CommandSwitch("--source-port-ranges")]
    public string? SourcePortRanges { get; set; }
}