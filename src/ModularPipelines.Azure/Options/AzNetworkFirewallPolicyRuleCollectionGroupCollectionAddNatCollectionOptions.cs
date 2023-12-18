using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("network", "firewall", "policy", "rule-collection-group", "collection", "add-nat-collection")]
public record AzNetworkFirewallPolicyRuleCollectionGroupCollectionAddNatCollectionOptions(
[property: CommandSwitch("--collection-priority")] string CollectionPriority,
[property: CommandSwitch("--ip-protocols")] string IpProtocols,
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--policy-name")] string PolicyName,
[property: CommandSwitch("--rcg-name")] string RcgName,
[property: CommandSwitch("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CommandSwitch("--action")]
    public string? Action { get; set; }

    [CommandSwitch("--add")]
    public string? Add { get; set; }

    [CommandSwitch("--description")]
    public string? Description { get; set; }

    [CommandSwitch("--dest-addr")]
    public string? DestAddr { get; set; }

    [CommandSwitch("--destination-ports")]
    public string? DestinationPorts { get; set; }

    [BooleanCommandSwitch("--force-string")]
    public bool? ForceString { get; set; }

    [BooleanCommandSwitch("--no-wait")]
    public bool? NoWait { get; set; }

    [CommandSwitch("--remove")]
    public string? Remove { get; set; }

    [CommandSwitch("--rule-name")]
    public string? RuleName { get; set; }

    [CommandSwitch("--set")]
    public string? Set { get; set; }

    [CommandSwitch("--source-addresses")]
    public string? SourceAddresses { get; set; }

    [CommandSwitch("--source-ip-groups")]
    public string? SourceIpGroups { get; set; }

    [CommandSwitch("--translated-address")]
    public string? TranslatedAddress { get; set; }

    [CommandSwitch("--translated-fqdn")]
    public string? TranslatedFqdn { get; set; }

    [CommandSwitch("--translated-port")]
    public string? TranslatedPort { get; set; }
}