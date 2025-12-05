using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("cosmosdb", "mongocluster", "firewall", "rule", "update")]
public record AzCosmosdbMongoclusterFirewallRuleUpdateOptions(
[property: CliOption("--cluster-name")] string ClusterName,
[property: CliOption("--end-ip-address")] string EndIpAddress,
[property: CliOption("--resource-group")] string ResourceGroup,
[property: CliOption("--rule-name")] string RuleName,
[property: CliOption("--start-ip-address")] string StartIpAddress
) : AzOptions;