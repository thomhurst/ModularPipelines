using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("network", "firewall", "policy", "rule-collection-group", "collection", "list")]
public record AzNetworkFirewallPolicyRuleCollectionGroupCollectionListOptions(
[property: CommandSwitch("--policy-name")] string PolicyName,
[property: CommandSwitch("--rcg-name")] string RcgName,
[property: CommandSwitch("--resource-group")] string ResourceGroup
) : AzOptions
{
}