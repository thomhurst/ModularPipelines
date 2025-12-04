using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("cosmosdb", "postgres", "firewall-rule", "create")]
public record AzCosmosdbPostgresFirewallRuleCreateOptions(
[property: CliOption("--cluster-name")] string ClusterName,
[property: CliOption("--end-ip-address")] string EndIpAddress,
[property: CliOption("--firewall-rule-name")] string FirewallRuleName,
[property: CliOption("--resource-group")] string ResourceGroup,
[property: CliOption("--start-ip-address")] string StartIpAddress
) : AzOptions
{
    [CliFlag("--no-wait")]
    public bool? NoWait { get; set; }
}