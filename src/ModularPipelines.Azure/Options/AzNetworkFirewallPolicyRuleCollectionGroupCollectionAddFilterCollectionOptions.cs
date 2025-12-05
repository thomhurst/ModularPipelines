using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("network", "firewall", "policy", "rule-collection-group", "collection", "add-filter-collection")]
public record AzNetworkFirewallPolicyRuleCollectionGroupCollectionAddFilterCollectionOptions(
[property: CliOption("--collection-priority")] string CollectionPriority,
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

    [CliOption("--dest-ipg")]
    public string? DestIpg { get; set; }

    [CliOption("--destination-fqdns")]
    public string? DestinationFqdns { get; set; }

    [CliOption("--destination-ports")]
    public string? DestinationPorts { get; set; }

    [CliFlag("--enable-tls-insp")]
    public bool? EnableTlsInsp { get; set; }

    [CliFlag("--force-string")]
    public bool? ForceString { get; set; }

    [CliOption("--fqdn-tags")]
    public string? FqdnTags { get; set; }

    [CliOption("--ip-protocols")]
    public string? IpProtocols { get; set; }

    [CliFlag("--no-wait")]
    public bool? NoWait { get; set; }

    [CliOption("--protocols")]
    public string? Protocols { get; set; }

    [CliOption("--remove")]
    public string? Remove { get; set; }

    [CliOption("--rule-name")]
    public string? RuleName { get; set; }

    [CliOption("--rule-type")]
    public string? RuleType { get; set; }

    [CliOption("--set")]
    public string? Set { get; set; }

    [CliOption("--source-addresses")]
    public string? SourceAddresses { get; set; }

    [CliOption("--source-ip-groups")]
    public string? SourceIpGroups { get; set; }

    [CliOption("--target-fqdns")]
    public string? TargetFqdns { get; set; }

    [CliOption("--target-urls")]
    public string? TargetUrls { get; set; }

    [CliOption("--web-categories")]
    public string? WebCategories { get; set; }
}