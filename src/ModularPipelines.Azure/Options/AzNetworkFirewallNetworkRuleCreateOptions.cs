using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("network", "firewall", "network-rule", "create")]
public record AzNetworkFirewallNetworkRuleCreateOptions(
[property: CliOption("--collection-name")] string CollectionName,
[property: CliOption("--destination-ports")] string DestinationPorts,
[property: CliOption("--firewall-name")] string FirewallName,
[property: CliOption("--name")] string Name,
[property: CliOption("--protocols")] string Protocols,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CliOption("--action")]
    public string? Action { get; set; }

    [CliOption("--description")]
    public string? Description { get; set; }

    [CliOption("--dest-addr")]
    public string? DestAddr { get; set; }

    [CliOption("--destination-fqdns")]
    public string? DestinationFqdns { get; set; }

    [CliOption("--destination-ip-groups")]
    public string? DestinationIpGroups { get; set; }

    [CliOption("--priority")]
    public string? Priority { get; set; }

    [CliOption("--source-addresses")]
    public string? SourceAddresses { get; set; }

    [CliOption("--source-ip-groups")]
    public string? SourceIpGroups { get; set; }
}