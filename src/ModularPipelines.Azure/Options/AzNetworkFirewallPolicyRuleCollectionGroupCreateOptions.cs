using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("network", "firewall", "policy", "rule-collection-group", "create")]
public record AzNetworkFirewallPolicyRuleCollectionGroupCreateOptions(
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--policy-name")] string PolicyName,
[property: CommandSwitch("--priority")] string Priority,
[property: CommandSwitch("--resource-group")] string ResourceGroup
) : AzOptions
{
    [BooleanCommandSwitch("--no-wait")]
    public bool? NoWait { get; set; }
}