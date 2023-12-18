using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("sf", "managed-cluster", "network-security-rule", "add")]
public record AzSfManagedClusterNetworkSecurityRuleAddOptions(
[property: CommandSwitch("--cluster-name")] string ClusterName,
[property: CommandSwitch("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CommandSwitch("--access")]
    public string? Access { get; set; }

    [CommandSwitch("--description")]
    public string? Description { get; set; }

    [CommandSwitch("--dest-addr-prefixes")]
    public string? DestAddrPrefixes { get; set; }

    [CommandSwitch("--dest-port-ranges")]
    public string? DestPortRanges { get; set; }

    [CommandSwitch("--direction")]
    public string? Direction { get; set; }

    [CommandSwitch("--name")]
    public string? Name { get; set; }

    [CommandSwitch("--priority")]
    public string? Priority { get; set; }

    [CommandSwitch("--protocol")]
    public string? Protocol { get; set; }

    [CommandSwitch("--source-addr-prefixes")]
    public string? SourceAddrPrefixes { get; set; }

    [CommandSwitch("--source-port-ranges")]
    public string? SourcePortRanges { get; set; }
}