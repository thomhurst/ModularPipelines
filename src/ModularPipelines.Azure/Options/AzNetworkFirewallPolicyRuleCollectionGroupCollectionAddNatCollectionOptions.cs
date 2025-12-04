using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("network", "firewall", "policy", "rule-collection-group", "collection", "add-nat-collection")]
public record AzNetworkFirewallPolicyRuleCollectionGroupCollectionAddNatCollectionOptions(
[property: CliOption("--collection-priority")] string CollectionPriority,
[property: CliOption("--ip-protocols")] string IpProtocols,
[property: CliOption("--name")] string Name,
[property: CliOption("--policy-name")] string PolicyName,
[property: CliOption("--rcg-name")] string RcgName,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CliOption("--action")]
    public string? Action { get; set; }

    [CliOption("--add")]
    public string? Add { get; set; }

    [CliOption("--description")]
    public string? Description { get; set; }

    [CliOption("--dest-addr")]
    public string? DestAddr { get; set; }

    [CliOption("--destination-ports")]
    public string? DestinationPorts { get; set; }

    [CliFlag("--force-string")]
    public bool? ForceString { get; set; }

    [CliFlag("--no-wait")]
    public bool? NoWait { get; set; }

    [CliOption("--remove")]
    public string? Remove { get; set; }

    [CliOption("--rule-name")]
    public string? RuleName { get; set; }

    [CliOption("--set")]
    public string? Set { get; set; }

    [CliOption("--source-addresses")]
    public string? SourceAddresses { get; set; }

    [CliOption("--source-ip-groups")]
    public string? SourceIpGroups { get; set; }

    [CliOption("--translated-address")]
    public string? TranslatedAddress { get; set; }

    [CliOption("--translated-fqdn")]
    public string? TranslatedFqdn { get; set; }

    [CliOption("--translated-port")]
    public string? TranslatedPort { get; set; }
}