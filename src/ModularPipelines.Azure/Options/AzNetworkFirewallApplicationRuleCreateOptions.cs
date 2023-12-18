using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("network", "firewall", "application-rule", "create")]
public record AzNetworkFirewallApplicationRuleCreateOptions(
[property: CommandSwitch("--collection-name")] string CollectionName,
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

    [CommandSwitch("--fqdn-tags")]
    public string? FqdnTags { get; set; }

    [CommandSwitch("--priority")]
    public string? Priority { get; set; }

    [CommandSwitch("--source-addresses")]
    public string? SourceAddresses { get; set; }

    [CommandSwitch("--source-ip-groups")]
    public string? SourceIpGroups { get; set; }

    [CommandSwitch("--target-fqdns")]
    public string? TargetFqdns { get; set; }
}

