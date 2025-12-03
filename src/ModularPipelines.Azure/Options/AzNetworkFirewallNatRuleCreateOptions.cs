using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("network", "firewall", "nat-rule", "create")]
public record AzNetworkFirewallNatRuleCreateOptions(
[property: CliOption("--collection-name")] string CollectionName,
[property: CliOption("--dest-addr")] string DestAddr,
[property: CliOption("--destination-ports")] string DestinationPorts,
[property: CliOption("--firewall-name")] string FirewallName,
[property: CliOption("--name")] string Name,
[property: CliOption("--protocols")] string Protocols,
[property: CliOption("--resource-group")] string ResourceGroup,
[property: CliOption("--translated-port")] string TranslatedPort
) : AzOptions
{
    [CliOption("--action")]
    public string? Action { get; set; }

    [CliOption("--description")]
    public string? Description { get; set; }

    [CliOption("--priority")]
    public string? Priority { get; set; }

    [CliOption("--source-addresses")]
    public string? SourceAddresses { get; set; }

    [CliOption("--source-ip-groups")]
    public string? SourceIpGroups { get; set; }

    [CliOption("--translated-address")]
    public string? TranslatedAddress { get; set; }

    [CliOption("--translated-fqdn")]
    public string? TranslatedFqdn { get; set; }
}