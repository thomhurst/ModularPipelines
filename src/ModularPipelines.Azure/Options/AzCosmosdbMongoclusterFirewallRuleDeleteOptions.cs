using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("cosmosdb", "mongocluster", "firewall", "rule", "delete")]
public record AzCosmosdbMongoclusterFirewallRuleDeleteOptions(
[property: CliOption("--cluster-name")] string ClusterName,
[property: CliOption("--resource-group")] string ResourceGroup,
[property: CliOption("--rule-name")] string RuleName
) : AzOptions
{
    [CliFlag("--yes")]
    public bool? Yes { get; set; }
}