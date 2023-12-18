using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("cosmosdb", "postgres", "firewall-rule", "create")]
public record AzCosmosdbPostgresFirewallRuleCreateOptions(
[property: CommandSwitch("--cluster-name")] string ClusterName,
[property: CommandSwitch("--end-ip-address")] string EndIpAddress,
[property: CommandSwitch("--firewall-rule-name")] string FirewallRuleName,
[property: CommandSwitch("--resource-group")] string ResourceGroup,
[property: CommandSwitch("--start-ip-address")] string StartIpAddress
) : AzOptions
{
    [BooleanCommandSwitch("--no-wait")]
    public bool? NoWait { get; set; }
}

