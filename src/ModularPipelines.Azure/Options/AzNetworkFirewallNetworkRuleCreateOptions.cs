using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("network", "firewall", "network-rule", "create")]
public record AzNetworkFirewallNetworkRuleCreateOptions(
[property: CommandSwitch("--collection-name")] string CollectionName,
[property: CommandSwitch("--destination-ports")] string DestinationPorts,
[property: CommandSwitch("--firewall-name")] string FirewallName,
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--protocols")] string Protocols,
[property: CommandSwitch("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CommandSwitch("--action")]
    public string? Action { get; set; }

    [CommandSwitch("--description")]
    public string? Description { get; set; }

    [CommandSwitch("--dest-addr")]
    public string? DestAddr { get; set; }

    [CommandSwitch("--destination-fqdns")]
    public string? DestinationFqdns { get; set; }

    [CommandSwitch("--destination-ip-groups")]
    public string? DestinationIpGroups { get; set; }

    [CommandSwitch("--priority")]
    public string? Priority { get; set; }

    [CommandSwitch("--source-addresses")]
    public string? SourceAddresses { get; set; }

    [CommandSwitch("--source-ip-groups")]
    public string? SourceIpGroups { get; set; }
}