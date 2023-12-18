using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("network", "firewall", "nat-rule", "create")]
public record AzNetworkFirewallNatRuleCreateOptions(
[property: CommandSwitch("--collection-name")] string CollectionName,
[property: CommandSwitch("--dest-addr")] string DestAddr,
[property: CommandSwitch("--destination-ports")] string DestinationPorts,
[property: CommandSwitch("--firewall-name")] string FirewallName,
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--protocols")] string Protocols,
[property: CommandSwitch("--resource-group")] string ResourceGroup,
[property: CommandSwitch("--translated-port")] string TranslatedPort
) : AzOptions
{
    [CommandSwitch("--action")]
    public string? Action { get; set; }

    [CommandSwitch("--description")]
    public string? Description { get; set; }

    [CommandSwitch("--priority")]
    public string? Priority { get; set; }

    [CommandSwitch("--source-addresses")]
    public string? SourceAddresses { get; set; }

    [CommandSwitch("--source-ip-groups")]
    public string? SourceIpGroups { get; set; }

    [CommandSwitch("--translated-address")]
    public string? TranslatedAddress { get; set; }

    [CommandSwitch("--translated-fqdn")]
    public string? TranslatedFqdn { get; set; }
}