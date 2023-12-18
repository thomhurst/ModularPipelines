using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("network", "firewall", "policy", "rule-collection-group", "collection", "rule", "update")]
public record AzNetworkFirewallPolicyRuleCollectionGroupCollectionRuleUpdateOptions(
[property: CommandSwitch("--collection-name")] string CollectionName,
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--policy-name")] string PolicyName,
[property: CommandSwitch("--rcg-name")] string RcgName,
[property: CommandSwitch("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CommandSwitch("--add")]
    public string? Add { get; set; }

    [CommandSwitch("--description")]
    public string? Description { get; set; }

    [CommandSwitch("--dest-addr")]
    public string? DestAddr { get; set; }

    [CommandSwitch("--dest-ipg")]
    public string? DestIpg { get; set; }

    [CommandSwitch("--destination-fqdns")]
    public string? DestinationFqdns { get; set; }

    [CommandSwitch("--destination-ports")]
    public string? DestinationPorts { get; set; }

    [BooleanCommandSwitch("--enable-tls-insp")]
    public bool? EnableTlsInsp { get; set; }

    [BooleanCommandSwitch("--force-string")]
    public bool? ForceString { get; set; }

    [CommandSwitch("--fqdn-tags")]
    public string? FqdnTags { get; set; }

    [CommandSwitch("--ip-protocols")]
    public string? IpProtocols { get; set; }

    [BooleanCommandSwitch("--no-wait")]
    public bool? NoWait { get; set; }

    [CommandSwitch("--protocols")]
    public string? Protocols { get; set; }

    [CommandSwitch("--remove")]
    public string? Remove { get; set; }

    [CommandSwitch("--set")]
    public string? Set { get; set; }

    [CommandSwitch("--source-addresses")]
    public string? SourceAddresses { get; set; }

    [CommandSwitch("--source-ip-groups")]
    public string? SourceIpGroups { get; set; }

    [CommandSwitch("--target-fqdns")]
    public string? TargetFqdns { get; set; }

    [CommandSwitch("--target-urls")]
    public string? TargetUrls { get; set; }

    [CommandSwitch("--translated-address")]
    public string? TranslatedAddress { get; set; }

    [CommandSwitch("--translated-fqdn")]
    public string? TranslatedFqdn { get; set; }

    [CommandSwitch("--translated-port")]
    public string? TranslatedPort { get; set; }

    [CommandSwitch("--web-categories")]
    public string? WebCategories { get; set; }
}